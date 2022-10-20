using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance != null) return _instance;

            throw new NullReferenceException();
        }
    }

    
    public Interactor primaryPlayer;
    public Interactor secondaryPlayer;
    
    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        transition.material.SetFloat("_RectScaleOuter", 3);
        transition.material.SetFloat("_RectScale", -0.2f);
    }

    public Player current = Player.Primary;
    public void TogglePlayers(Player state = Player.Switch)
    {
        switch (state)
        {
            case Player.Switch:
                primaryPlayer.TogglePlayerInputs();
                secondaryPlayer.TogglePlayerInputs();
                break;
            case Player.Primary:
                if (current != Player.Primary)
                {
                    StartCoroutine(ScreenTransition(false));
                    current = Player.Primary;
                }
                break;
            case Player.Secondary:
                if (current != Player.Secondary)
                {
                    StartCoroutine(ScreenTransition(true));
                    current = Player.Secondary;
                }
                break;
        }
        
    }

    [SerializeField] private RawImage transition;
    IEnumerator ScreenTransition(bool toSecondary)
    {
        var primaryEdge = 3.0f;
        var primarySpace = -0.2f;
        var secondaryEdge = 0.2f;
        var secondarySpace = 0.5f;

        if (toSecondary)
        {
            primaryPlayer.DisablePlayerInputs();
            float transitionA = 0f;
            while (transitionA < 1)
            {
                transition.material.SetFloat("_RectScaleOuter",Mathf.Lerp(primaryEdge, secondaryEdge, transitionA));
                transitionA += 0.02f;
                yield return new WaitForSeconds(0.02f);
            }
            
            secondaryPlayer.EnablePlayerInputs();
            
            float transitionB = 0f;
            while (transitionB < 1)
            {
                transition.material.SetFloat("_RectScale",Mathf.Lerp(primarySpace, secondarySpace, transitionB));
                transitionB += 0.02f;
                yield return new WaitForSeconds(0.02f);
            }
        }
        else
        {
            float transitionB = 0f;
            while (transitionB < 1)
            {
                transition.material.SetFloat("_RectScale",Mathf.Lerp(secondarySpace, primarySpace, transitionB));
                transitionB += 0.02f;
                yield return new WaitForSeconds(0.02f);
            }
            
            secondaryPlayer.DisablePlayerInputs();
            
            float transitionA = 0f;
            while (transitionA < 1)
            {
                transition.material.SetFloat("_RectScaleOuter",Mathf.Lerp(secondaryEdge, primaryEdge, transitionA));
                transitionA += 0.02f;
                yield return new WaitForSeconds(0.02f);
            }
            primaryPlayer.EnablePlayerInputs();
        }
    }
}

public enum Player
{
    Primary,
    Secondary,
    Switch
}
