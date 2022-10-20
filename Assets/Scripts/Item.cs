using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string choice;
    [SerializeField] private GameObject[] dependents; 

    public string Choice
    {
        get
        {
            if (dependents != null)
            {
                foreach (var dependent in dependents)
                {
                    if(dependent != null) dependent.SetActive(true);
                }
            }
            return choice;
        }
    }

    private void Start()
    {
        gameObject.layer = 6;
    }
}
