using System.Collections;
using System.Collections.Generic;
using Michsky.DreamOS;
using UnityEngine;
using UnityEngine.Events;

public class OsSystem : MonoBehaviour
{
    private CommanderManager commanderApp;
    private ComputerManager computerManager;
    public FileManager folders;
    // Start is called before the first frame update
    void Start()
    {
        //FolderSystem folders = gameObject.AddComponent<FolderSystem>();
        commanderApp = GameObject.Find("Commander").GetComponent<CommanderManager>();
        Debug.Log("start os system");
        folders = GameObject.Find("Folde").GetComponent<FileManager>();

        ls();
        cd();
        rm();
        nano();
        mkdir();
        ifconfig();
        ping();
    }


    public void ls()
    {
        CommanderManager.CommandItem item = createItem("ls", "ls");
        folders = GameObject.Find("Folde").GetComponent<FileManager>();
        commanderApp = GameObject.Find("Commander").GetComponent<CommanderManager>();

        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText = folders.ListCurrentFolderContent(commanderApp.argument);
            Debug.Log("artg :"+commanderApp.argument);

        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    } 
    
    public void cd()
    {
        CommanderManager.CommandItem item = createItem("cd", "cd");

        item.onProcessEvent.AddListener(delegate
        {
            
            commanderApp.returnedText = folders.Navigate(commanderApp.argument);
        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    } 
    
    public void ping()
    {
        CommanderManager.CommandItem item = createItem("ping", "ping");

        item.onProcessEvent.AddListener(delegate
        {
            string pingResponse = "ping: unknown host";
            if (commanderApp.argument != "")
            {
                Debug.Log("argument "+commanderApp.argument);
                ComputerManager computer = GetComponentInParent<ComputerManager>();
                pingResponse = computer.Ping(commanderApp.argument);

            }

            commanderApp.returnedText = pingResponse;

        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    } 
    
    public void rm()
    {
        CommanderManager.CommandItem item = createItem("rm", "rm");


        item.onProcessEvent.AddListener(delegate
        {
            if (commanderApp.argument != "")
            {
                commanderApp.returnedText = folders.Remove(commanderApp.argument);
            }
            folders.Navigate(commanderApp.argument);
            //commanderApp.returnedText = folders.Navigate();

        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }    
    public void nano()
    {
        CommanderManager.CommandItem item = createItem("nano", "nano");


        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText =  folders.CreateFile(commanderApp.argument,"");

        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }    
    public void mkdir()
    {
        CommanderManager.CommandItem item = createItem("mkdir", "mkdir");


        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText =  folders.CreateFolder(commanderApp.argument);
        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }
    
    public void ifconfig()
    {
        CommanderManager.CommandItem item = createItem("ifconfig", "ifconfig");
        Debug.Log("Pas listner");

        item.onProcessEvent.AddListener(delegate
        {
            ComputerManager computer = GetComponentInParent<ComputerManager>();
            
            
            
            Router router = GameObject.Find("Router").GetComponent<Router>();

            // get ComputerManager from the parent
            // Get the IP address of this computer
            string ipAddress = router.GetIPAddress(computer);
            
            //string ipAddress = network.GetIPAddress(computer);
            string macAdress = computer.MacAddress;

            // Set the returned text to the IP address
            //commanderApp.returnedText = ipAddress+"GetIfconfigOutput(ipAddress, macAdress)"+macAdress;
            commanderApp.returnedText = GetIfconfigOutput(ipAddress, macAdress);
        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }

    

    private CommanderManager.CommandItem createItem(string name,string command,string feedbackText = "null",float feedbackDelay = 0.1f,float onProcessDelay = 1f)
    {
        CommanderManager.CommandItem item = new CommanderManager.CommandItem();
        item.commandName = name; // Not important - only for editor
        item.command = command; // Actual command - user needs to type this
        item.feedbackText = feedbackText; // Feedback text
        item.feedbackDelay = feedbackDelay;
        item.onProcessDelay = onProcessDelay;
        item.isNewCommand = true;
        // item.onProcessEvent = null;
        if (item.onProcessEvent == null)
            item.onProcessEvent = new UnityEvent();
        
        return item;        
    }
    
    public string GetIfconfigOutput(string ipAddress, string macAddress)
    {
        string ifconfigOutput = "";

        // Add eth0 interface info
        ifconfigOutput += "eth0      Link encap:Ethernet  HWaddr " + macAddress + "\n";
        ifconfigOutput += "           inet addr:" + ipAddress + "  Bcast:192.168.14.255  Mask:255.255.255.0\n";
        ifconfigOutput += "           inet6 addr: fe80::207:e9ff:fed5:e05d/48 Scope:Global\n";
        ifconfigOutput += "           inet6 addr: fe80::207:e9ff:fed5:e05d/64 Scope:Link\n";
        ifconfigOutput += "           UP BROADCAST RUNNING MULTICAST  MTU:1500  Metric:1\n";
        ifconfigOutput += "           RX packets:346293248 errors:0 dropped:0 overruns:0 frame:0\n";
        ifconfigOutput += "           TX packets:1089423722 errors:0 dropped:0 overruns:0 carrier:0\n";
        ifconfigOutput += "           collisions:0 txqueuelen:1000 \n";
        ifconfigOutput += "           RX bytes:1501808809 (1.3 GiB)  TX bytes:4184566400 (3.8 GiB)  \n\n";

        // Add loopback interface info
        ifconfigOutput += "lo        Link encap:Local Loopback  \n";
        ifconfigOutput += "           inet addr:127.0.0.1  Mask:255.0.0.0\n";
        ifconfigOutput += "           inet6 addr: ::1/128 Scope:Host\n";
        ifconfigOutput += "           UP LOOPBACK RUNNING  MTU:16436  Metric:1\n";
        ifconfigOutput += "           RX packets:0 errors:0 dropped:0 overruns:0 frame:0\n";
        ifconfigOutput += "           TX packets:0 errors:0 dropped:0 overruns:0 carrier:0\n";
        ifconfigOutput += "           collisions:0 txqueuelen:0 \n";
        ifconfigOutput += "           RX bytes:0 (0.0 b)  TX bytes:0 (0.0 b)\n";

        return ifconfigOutput;
    }


    
}
