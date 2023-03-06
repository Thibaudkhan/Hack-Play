using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerManager : MonoBehaviour
{
    private bool isUsingComputer = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseComputer()
    {
        if(isUsingComputer)
        {
            ExitComputer();
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true); // or false
            }
        }

    }

    void ExitComputer()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false); // or false
        }
    }
    

}
