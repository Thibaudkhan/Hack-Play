using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    public GameObject computer;
    public KeyCode interactKey = KeyCode.E;
    public float interactionDistance = 3.0f;

    private bool canInteract = false;

    void Update()
    {
        // Check if the interact key is pressed and the player is close enough to the computer object
        if (Input.GetButtonDown("Iteraction") && canInteract)
        {
            Debug.Log("Interacting with computer");
            
            // Set the "kid" property on the computer object to true
            computer.GetComponent<ComputerManager>().UseComputer();

        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object " +other.name+" entered "+ other.gameObject);

        // Check if the player entered the trigger zone of the computer object
        if (other.gameObject == computer)
        {
            Debug.Log("Player entered computer trigger zone");
            // Set the canInteract flag to true so the player can interact with the computer
            canInteract = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player exited the trigger zone of the computer object
        if (other.gameObject == computer)
        {
            // Set the canInteract flag to false so the player cannot interact with the computer
            canInteract = false;
        }
    }
}

