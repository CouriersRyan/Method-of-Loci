Shader "Unlit/TransitionFog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RectScale ("Rect Scale", Float) = 0.5
        _RectScaleOuter ("Rect Scale Outer", Float) = 0.2
    }
    SubShader
    {
         Tags {"RenderType"="Transparent" "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            float rectangle (float2 uv, float2 scale) {
                float2 s = scale * 0.5;
                float2 shaper = float2(step(-s.x, uv.x), step(-s.y, uv.y));
                shaper *= float2(1-step(s.x, uv.x), 1-step(s.y, uv.y));
                return shaper.x * shaper.y;
            }

            float rand3D (float3 uv3)
            {
                return frac(sin(dot(uv3.xyz, float3(12.9898, 78.233, 84.394))) * 43758.5453123);
            }

            // modified to take in an extra coordinate z, representing time
            float noise (float2 uv, float z) {
                float2 ipos = floor(uv);
                float2 fpos = frac(uv);
                float iz = floor(z);
                float fz = frac(z);
                
                float o  = rand3D(float3(ipos, iz));
                float x  = rand3D(float3(ipos, iz) + float3(1, 0, 0));
                float y  = rand3D(float3(ipos, iz) + float3(0, 1, 0));
                float xy = rand3D(float3(ipos, iz) + float3(1, 1, 0));
                float oz  = rand3D(float3(ipos, iz) + float3(0, 0, 1));
                float xz  = rand3D(float3(ipos, iz) + float3(1, 0, 1));
                float yz  = rand3D(float3(ipos, iz) + float3(0, 1, 1));
                float xyz = rand3D(float3(ipos, iz) + float3(1, 1, 1));

                float2 smooth = smoothstep(0, 1, fpos);
                float smoothz = smoothstep(0, 1, fz);
                
                return lerp (
                    lerp(
                        lerp(o,  x, smooth.x),
                        lerp(y, xy, smooth.x), smooth.y),
                    lerp(
                        lerp(oz, xz, smooth.x),
                        lerp(yz, xyz, smooth.x), smooth.y),
                smoothz);
            }

            float fractal_noise (float2 uv, int n, float time) {
                float fn = 0;
                // fractal noise is created by adding together "octaves" of a noise
                // an octave is another noise value that is half the amplitude and double the frequency of the previously added noise
                // below the uv is multiplied by a value double the previous. multiplying the uv changes the "frequency" or scale of the noise becuase it scales the underlying grid that is used to create the value noise
                // the noise result from each line is multiplied by a value half of the previous value to change the "amplitude" or intensity or just how much that noise contributes to the overall resulting fractal noise.

                for(int j = 0; j < n; j++)
                {
                    fn += (1.0 / pow(2, j + 1)) * noise(uv * pow(2, j), time * pow(2, j));
                }
                
                return fn;
            }
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _RectScale;
            float _RectScaleOuter;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = (i.uv * 2 - 1);
                uv.x *= 8;
                uv.y *= 4.5;
                
                
                // sample the texture
                fixed4 col = tex2D(_MainTex, uv);
                float noise = lerp( 0.5f, 1, smoothstep(0, 1, 1 - pow(fractal_noise(uv, 4, _Time.y), 2)));
                float rect = rectangle(uv, float2(16*_RectScale, 9*_RectScale));
                float rectEdge = rectangle(uv, float2(16 * (_RectScale + _RectScaleOuter), 9*(_RectScale + _RectScaleOuter)));
                float smoothX = smoothstep(8 * _RectScale, 8 * (_RectScale + _RectScaleOuter), abs(uv.x));
                float smoothY = smoothstep(4.5f * _RectScale, 4.5f * (_RectScale + _RectScaleOuter), abs(uv.y));
                float smooth = max(smoothX, smoothY);
                
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return float4(col.rgb, noise) * smooth;
            }
            ENDCG
        }
    }
}
