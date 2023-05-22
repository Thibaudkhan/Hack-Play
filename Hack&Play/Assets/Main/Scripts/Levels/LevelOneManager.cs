using System;
using System.Collections.Generic;
using Main.Scripts.Network;
using Unity.VisualScripting;
using UnityEngine;

public class LevelOneManager : MonoBehaviour
{
    public GameObject computerPrefab;
    public GameObject routerPrefab;
    public GameObject websitePrefab;

    [SerializeField]
    private List<Vector3> computersPosition = new List<Vector3>();

    //new Vector3(-1.2F, 1.33F, -5.45F))
    //new Vector3(-1.2F, 1.33F, 0)),
    [SerializeField]
    private List<GameObject> computers = new List<GameObject>();
    private string[][] listOfwebsites = { new string[] { "https://google.com","57.142.5.12","Google" } };
    
    private List<GameObject> listOfGOwebsites = new List<GameObject>();


    void Start()
    {
        // Create router and set properties
        Router router = Instantiate(routerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Router>();
        router.name = "Router";
        // Create computers and set properties
        var i = 0;
        Debug.Log("how many compuyter "+computers.Count);
        foreach (var computer in computers)
        {
            Debug.Log(computer.name);
            ComputerManager computerManager = computer.AddComponent<ComputerManager>();
            
            computerManager.name = computer.name;
            computerManager.tag = "Computer";
            computerManager.router = router;
            computerManager.gameObject.name =computer.name;
            computerManager.folderPath = Application.streamingAssetsPath +"/OsData/"+ computer.name + "/Os/";
            if (computer.name != "Hacker")
            {
                AIController aiController = computer.AddComponent<AIController>();
                aiController.computerManager = computerManager;
                aiController.computerManager.macMicroship = "intel";

            }
            else
            {
                computerManager.macMicroship = "amd";

            }
            //OsSystem osSystem = computer.AddComponent<OsSystem>();
            computerManager.osSystem = computer.AddComponent<OsSystem>();

            FileManager fileManager = computer.AddComponent<FileManager>();
            computerManager.osSystem.folders = fileManager;

            router.AddComputerToPort(i,computerManager);

            
            
            i++;
        }
        
        

        foreach (var web in listOfwebsites)
        {
            
            Website website = new Website();
            ComputerManager goComputerManager = new ComputerManager();
            goComputerManager.name = web[2];
            goComputerManager.IpAddress = web[1];
            website.ComputerManager = goComputerManager;
            website.ComputerManager.router = router;
            website.Url = web[0];

            NetworkManager.listOfGOwebsites.Add(website);
        }
        

    }
}