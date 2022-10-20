using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Serialization;

// Two interactions. Main player camera interacts with people to talk to them, the other one interacts with objects around
// a house to form sentences and memories out of objects and emotions laid down in the Method of Loci.
// As you talk with people on your journey you discover the logic behind the object's placement and the house in your mind
// and what it represents.
// Interactor is the parent class of the two interactions, one on the player swimming through water and another wandering
// through the house.
// Based off the Swimmer.

public class Interactor : MonoBehaviour
{
    [SerializeField] protected Camera cam;
    protected PlayerInput _input;
    [SerializeField] protected LayerMask targets;

    protected virtual void Start()
    {
        _input = GetComponent<PlayerInput>();
        InputUser.PerformPairingWithDevice(Keyboard.current, _input.user);
        InputUser.PerformPairingWithDevice(Mouse.current, _input.user);
        _input.user.ActivateControlScheme("KeyboardMouse");
    }

    void OnInteract(InputValue value)
    {
        Interaction();
    }

    protected void Interaction()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 50.0f, targets))
        {
            Interact(hit);
        }
    }

    protected virtual void Interact(RaycastHit hit)
    {
        throw new NotImplementedException();
    }

    public virtual void DisablePlayerInputs()
    {
        _input.DeactivateInput();
    }
    public virtual void EnablePlayerInputs()
    {
        _input.ActivateInput();
    }

    public bool CheckPlayerInputs()
    {
        return _input.inputIsActive;
    }

    public void TogglePlayerInputs()
    {
        if (CheckPlayerInputs())
        {
            DisablePlayerInputs();
        }
        else
        {
            EnablePlayerInputs();
        }
    }
}