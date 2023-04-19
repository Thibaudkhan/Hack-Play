using System.Collections.Generic;
using UnityEngine;

public class LevelOneManager : MonoBehaviour
{
    public NetworkManager networkManager;
    public GameObject computerPrefab;
    public GameObject routerPrefab;

    private readonly List<(string name, Vector3 position)> computers = new List<(string, Vector3)>
    {
        ("Hacker", new Vector3(-1.2F, 1.33F, -5.45F)),
        ("Cible1", new Vector3(-1.2F, 1.33F, 0)),
    };

    void Start()
    {
        // Create router and set properties
        Router router = Instantiate(routerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Router>();
        router.name = "Router";
        router.networkManager = networkManager;
        // Create computers and set properties
        var i = 0;
        foreach (var (name, position) in computers)
        {
            ComputerManager computerManager = Instantiate(computerPrefab, position, transform.rotation).AddComponent<ComputerManager>();
            computerManager.name = name;
            computerManager.tag = "Computer";
            computerManager.router = router;
            computerManager.gameObject.name = $"Computer {name}";
            router.AddComputerToPort(i,computerManager);

            foreach (Transform child in computerManager.transform)
            {
                child.gameObject.SetActive(false);
            }
            // get component child by name
            //computerManager.transform.Find("DreamOS Canvas").gameObject.SetActive(false);
            
            
            i++;
        }
    }
}