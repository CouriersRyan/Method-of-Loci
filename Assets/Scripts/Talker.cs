using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : Interactor
{
    private InkManager _ink;

    protected override void Start()
    {
        base.Start();
        _ink = FindObjectOfType<InkManager>();
        GameManager.Instance.primaryPlayer = this;
    }
    protected override void Interact(RaycastHit hit)
    {
        _ink.DisplayNextLine();
    }
}
