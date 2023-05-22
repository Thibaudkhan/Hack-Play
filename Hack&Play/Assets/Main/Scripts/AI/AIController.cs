using System.Collections;
using System.Collections.Generic;
using Michsky.DreamOS;
using UnityEditor.Experimental;
using UnityEngine;
using NetworkManager = Main.Scripts.Network.NetworkManager;

public class AIController : MonoBehaviour
{
    public ComputerManager computerManager;
    List<string> websites = new List<string> {"http://www.google.com/", "http://www.youtube.com/", "http://www.facebook.com/"};
    private List<string> ftpCommand = new List<string> { "open","ls","close" };
    int currentIndex = 0;
    float delayBetweenRequests = 15f; // in seconds
    float timeUntilNextRequest = 10f;
    private int currentNavigationLevel = 0;
    private string currentSubpageUrl = "";
    private string currentBaseUrl = "http://www.life.com/";
    private List<GameObject> listOfWebsite;

    // Start is called before the first frame update

    void Update()
    {
        timeUntilNextRequest -= Time.deltaTime;
        if (timeUntilNextRequest <= 0f)
        {
            UsingInternet();
            UsingFTP();
            timeUntilNextRequest = delayBetweenRequests;
        }
    }

    private void UsingFTP()
    {
        string message = "";
        int index = NetworkManager.computers.FindIndex(x => x == computerManager);

        if (index != 1)
            return;
        
        string computerCible = NetworkManager.GetIpAddress(NetworkManager.computers[ NetworkManager.computers.Count - (index+1)]);

        foreach (CommanderManager.CommandItem command in computerManager.osSystem.CommanderApp.commands)
        {
            if (command.command == "ftp")
            {
                Debug.Log("invoking a command");
        
                // get the index in the list of the computer with the computer manager

                if (computerManager.isConnectedFtp)
                {
                    currentSubpageUrl = ftpCommand[Random.Range(1, ftpCommand.Count)];
                    computerManager.osSystem.CommanderApp.arguments = new []{
                        currentSubpageUrl
                    };
                }
                else
                {
                    computerManager.osSystem.CommanderApp.arguments = new []{
                        ftpCommand[0],
                        computerCible,
                        "admin",
                        "admin"
                    };

                }
                
                // stocke the resp
                command.onProcessEvent.Invoke();

                break;
            }
        }
       
    }

    private void UsingInternet()
    {
        string message = "";

        
        foreach (var website  in NetworkManager.listOfGOwebsites)
        {
            string subpageUrl = "";
            string methode = "GET";

            if (currentNavigationLevel == 0)
            {
                // Navigate to the login page
                subpageUrl = "login";
                // write a message type login and password
                message = "login: admin\npassword: admin";
                currentNavigationLevel++;
                methode = "POST";
            }
            else if (currentNavigationLevel <= 3)
            {
                // Navigate to a random subpage
                List<string> subpages = new List<string> { "page1", "page2", "page3", "page4", "page5" };
                currentSubpageUrl = subpages[Random.Range(0, subpages.Count)];
                subpageUrl = currentSubpageUrl;
                currentNavigationLevel++;
            }
            else
            {
                // Log out and return to the login page
                subpageUrl = "logout";
                currentNavigationLevel = 0;
            }
            website.FullUrl = website.Url +"/"+subpageUrl;
            computerManager.SendHttpRequest(website,message,methode);
        }
       


    }
    
}
