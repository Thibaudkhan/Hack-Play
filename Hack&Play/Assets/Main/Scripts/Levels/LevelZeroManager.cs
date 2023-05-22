using System.Collections;
using System.Collections.Generic;
using Main.Scripts.Network;
using UnityEngine;

public class LevelZeroManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject routerPrefab;
    public ParticleSystem explosionEffect;

    [SerializeField]
    private List<GameObject> computers = new List<GameObject>();
    private string[][] listOfwebsites = { new string[] { "https://google.com","57.142.5.12","Google" } };


    void Start()
    {
        // Create router and set properties
        Router router = Instantiate(routerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Router>();
        router.name = "Router";
        // Create computers and set properties
        var i = 0;
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
                
            }
            OsSystem osSystem = computer.AddComponent<OsSystem>();
            FileManager fileManager = computer.AddComponent<FileManager>();
            osSystem.folders = fileManager;

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
