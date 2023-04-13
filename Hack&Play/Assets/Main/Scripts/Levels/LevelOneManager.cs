using System.Collections;
using System.Collections.Generic;
using Main.Scripts.Network;
using UnityEngine;

public class LevelOneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public NetworkManager networkManager;
    public GameObject computerPrefab;
    public GameObject routerPrefab;

    void Start() {
        
        // Create a computer
        //Vector3 positionHack = new Vector3(0, 0, 0);
         GameObject computerHacker = Instantiate(computerPrefab, new Vector3(-1.2F, 1.33F, -5.45F), transform.rotation);
         ComputerManager computerManagerHacker = computerHacker.AddComponent<ComputerManager>();
         computerManagerHacker.networkManager = networkManager;
         computerManagerHacker.name = "Hacker";
         computerManagerHacker.tag = "Computer";
         computerManagerHacker.gameObject.name = "Computer Hacker";
         // find child  game object in computerManagerHacker
         //GameObject canvaHack = computerManagerHacker.gameObject.transform.Find("DreamOS Canvas").gameObject;
         //canvaHack.SetActive(false);
         foreach (Transform child in computerManagerHacker.transform)
         {
             child.gameObject.SetActive(false);
         }
        
        //
        GameObject computerCible1= Instantiate(computerPrefab, new Vector3(-1.2F, 1.33F, 0), transform.rotation);
        ComputerManager computerManagerCible1 = computerCible1.AddComponent<ComputerManager>();
        computerManagerCible1.networkManager = networkManager;
        computerManagerCible1.name = "Cible1";
        computerManagerCible1.tag = "Computer";
        GameObject canvaCible1 = computerManagerCible1.gameObject.transform.Find("DreamOS Canvas").gameObject;
        canvaCible1.SetActive(false);
        foreach (Transform child in computerManagerCible1.transform)
        {
            child.gameObject.SetActive(false);
        }
        
        // Create a router
        GameObject routerObject = Instantiate(routerPrefab);
        Router routerManager = routerObject.GetComponent<Router>();
        routerManager.networkManager = networkManager;
        routerManager.name = "Router";

    }
}
