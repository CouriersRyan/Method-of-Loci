using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPicker : Interactor
{
    private InkManager _ink;
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.secondaryPlayer = this;
        _ink = FindObjectOfType<InkManager>();
        DisablePlayerInputs();
    }
    protected override void Interact(RaycastHit hit)
    {
        Debug.Log("Hit!");
        _ink.MakeChoice(hit.collider.GetComponent<Item>().Choice);
    }
    
    public override void DisablePlayerInputs()
    {
        base.DisablePlayerInputs();
        cam.enabled = false;
    }
    public override void EnablePlayerInputs()
    {
        base.EnablePlayerInputs();
        cam.enabled = true;
    }
}
