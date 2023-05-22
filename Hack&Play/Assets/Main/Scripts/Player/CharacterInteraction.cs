using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    public GameObject computer;
    //public KeyCode interactKey = KeyCode.E;
    public float interactionDistance = 3.0f;
    
    public bool isUsingComputer = false;
    [SerializeField]
    private GameObject interactions;

    private bool canInteract = false;
    private void Start()
    {
    }

    void Update()
    {
        //check if player press tab
        // if (Input.GetKeyDown(KeyCode.Tab) && canInteract)
        // {
        //     Debug.Log("cocu");
        //     computer.GetComponent<ComputerManager>().UseComputer();
        //     isUsingComputer = !isUsingComputer;
        //
        // }
        
        
        // Check if the interact key is pressed and the player is close enough to the computer object
        // if (Input.GetButtonDown("Iteraction") && canInteract && !isUsingComputer)
        // {
        //     Debug.Log("Interacting with computer");
        //     interactions.SetActive(false);
        //
        //     // Set the "kid" property on the computer object to true
        //     computer.GetComponent<ComputerManager>().UseComputer();
        //     isUsingComputer = !isUsingComputer;
        //
        // }
    }

    void OnTriggerEnter(Collider other)
    {

        // Check if the player entered the trigger zone of the computer object
        // if (other.CompareTag("Computer"))
        // {
        //     // get gameobject by name
        //     interactions.SetActive(true);
        //     computer = other.gameObject;
        //     // Set the canInteract flag to true so the player can interact with the computer
        //     canInteract = true;
        // }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player exited the trigger zone of the computer object
        // if (other.gameObject == computer)
        // {
        //     // Set the canInteract flag to false so the player cannot interact with the computer
        //     canInteract = false;
        //     interactions.SetActive(false);
        //
        // }
    }
}

