using System;
using System.Collections.Generic;

using Main.Scripts.Terminal;
using Michsky.DreamOS;
using UnityEngine;
using UnityEngine.Events;
using NetworkManager = Main.Scripts.Network.NetworkManager;
using UnityEngine.SceneManagement;

public class OsSystem : MonoBehaviour
{
    private CommanderManager commanderApp;

    

    //private ComputerManager computerManager;
    public FileManager folders;
    private string _argument = "";
    private String[] _scenes = {"Home","LevelOne","LevelTwo","LevelThree","City"};
    private string response = "";
    // create a dicionary of commands
    private Dictionary<string, Action> _commands;

    public Dictionary<string, Action> Commands
    {
        get => _commands;
        set => _commands = value;
    }
    
    public CommanderManager CommanderApp
    {
        get => commanderApp;
        set => commanderApp = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        //FolderSystem folders = gameObject.AddComponent<FolderSystem>();
        commanderApp =  gameObject.GetComponentInChildren<CommanderManager>();
        commanderApp.returnedText = "";
        Transform currentTransform = commanderApp.transform;

        _commands = new Dictionary<string, Action>()
        {
            { "ls",ls },
            { "cd",cd },
            { "rm",rm },
            { "nano",nano },
            { "mkdir",mkdir },
            { "ifconfig",ifconfig },
            { "ping",ping },
            { "nmap",nmap },
            { "tcpdump",tcpdump },
            { "arp",arp },
            { "ettercap",ettercap },
            { "executefile",executefile },
            { "submit",submit },
            { "level",level },
            { "help",help },
            { "explode",explode },       
            { "ftp",ftp },
            { "open",open },
            { "close",close },
            { "e",e },
            
        };
        
        
        // Loop through all the parents of the GameObject
        folders = GetComponent<FileManager>();
        foreach (var key in _commands)
        {
            key.Value.Invoke();
        }
        // explode();
        // ls();
        // cd();
        // rm();
        // nano();
        // mkdir();
        // ifconfig();
        // ping();
        // nmap();
        // tcpdump();
        // arp();
        // ettercap();
        // executefile();
        // submit();
        // level();
        // ftp();
        // help();
    }

    private void explode()
    {
        CommanderManager.CommandItem item = createItem("explode", "explode");
        commanderApp.returnedText = "";
        
        item.onProcessEvent.AddListener(delegate
        {
            if (CheckingArgument())
            {
                ComputerManager computer = NetworkManager.GetComputer(_argument);

                computer.Explode();
                commanderApp.returnedText= "Boom";
            }
           

        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }
    
    private void submit()
    {
        CommanderManager.CommandItem item = createItem("submit", "submit");
        commanderApp.returnedText = "";
        
        item.onProcessEvent.AddListener(delegate
        {

            if (CheckingArgument())
            {
                commanderApp.returnedText = checkResultLevel() ? "Success" : "Failed";
            }


        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }
    
    private void level()
    {
        CommanderManager.CommandItem item = createItem("level", "level");
        commanderApp.returnedText = "";
        
        item.onProcessEvent.AddListener(delegate
        {

            if (CheckingArgument())
            {

                commanderApp.returnedText =  "Loading...";
                SceneManager.LoadScene(_scenes[Int32.Parse(_argument)]);

            }


        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }
    
    private void help()
    {
        CommanderManager.CommandItem item = createItem("help", "help");
        commanderApp.returnedText = "";
        
        item.onProcessEvent.AddListener(delegate
        {

            CheckingArgument();
            Debug.Log("Len "+commanderApp.commands.Count);
            // transoform CommanderManager.CommandItem into a string 
            foreach (var command in  commanderApp.commands)
            {
                commanderApp.returnedText += command.command + "\n";
            }

        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
        
    }


    public void ls()
    {
        CommanderManager.CommandItem item = createItem("ls", "ls");
        commanderApp.returnedText = "";
        
        item.onProcessEvent.AddListener(delegate
        {
            Debug.Log("ls");
            CheckingArgument();

            folders = GetComponent<FileManager>();
            //commanderApp = gameObject.GetComponentInChildren<CommanderManager>();
            //Debug.Log("commanderApp"+commanderApp.arguments[0]);
            commanderApp.returnedText = folders.ListCurrentFolderContent(_argument);

        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    } 
    
    public void cd()
    {
        CommanderManager.CommandItem item = createItem("cd", "cd");

        item.onProcessEvent.AddListener(delegate
        {
            if (CheckingArgument())
            {
                commanderApp.returnedText = folders.Navigate(_argument);

            }

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
            if (CheckingArgument())
            {
                
                ComputerManager computer = GetComponentInParent<ComputerManager>();
                
                pingResponse = computer.Ping(_argument);

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
            if (CheckingArgument())
            {
                commanderApp.returnedText = folders.Remove(_argument);
            }

            folders.Navigate(_argument);
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
            if (CheckingArgument())
            {
                commanderApp.returnedText =  folders.CreateFile(_argument,"");

            }

        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }    
    public void mkdir()
    {
        CommanderManager.CommandItem item = createItem("mkdir", "mkdir");


        item.onProcessEvent.AddListener(delegate
        {
            if (CheckingArgument())
            {
                commanderApp.returnedText =  folders.CreateFolder(_argument);

            }
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

            string ipAddress = router.GetIPAddress(computer);
            string macAdress = computer.MacAddress;

            commanderApp.returnedText = GetIfconfigOutput(ipAddress, macAdress);
        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }
    
    public void nmap()
    {
        CommanderManager.CommandItem item = createItem("nmap", "nmap");

        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText = "closed";
            
            if (CheckingArgument())
            {
                Nmap nmap = gameObject.AddComponent<Nmap>();
                nmap.Main(commanderApp.arguments);
                commanderApp.returnedText = string.Join("\n", nmap.DiscoverHosts());
            }
            
        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }
    
    private void tcpdump()
    {
        CommanderManager.CommandItem item = createItem("tcpdump", "tcpdump");

        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText = "closed";
            
            if (CheckingArgument())
            {
                WireShark wireShark = gameObject.AddComponent<WireShark>();
                if (_argument ==  "")
                {   
                    _argument = "log1.txt";
                }
                commanderApp.returnedText = wireShark.ShowPacketLog(_argument);
            }
            
        });

        commanderApp.commands.Add(item); // Add new item to the manager

    }
    
    public void ettercap()
    {
        CommanderManager.CommandItem item = createItem("ettercap", "ettercap");

        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText = "closed";
            
            if (CheckingArgument())
            {
                Debug.Log("entering ettercap");
                ComputerManager computer = GetComponentInParent<ComputerManager>();

                Ettercap.Main(commanderApp.arguments,computer);
                commanderApp.returnedText = "Ettercap started 2";
            }
            
        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    } 
    public void e()
    {
        CommanderManager.CommandItem item = createItem("e", "e");

        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText = "closed";

            if (CheckingArgument())
            {
                Debug.Log("entering ettercap");
                ComputerManager computer = GetComponentInParent<ComputerManager>();
                string[] a =  { "-M","arp:remote","/192.168.1.2/192.168.1.3/" };
                Ettercap.Main(a,computer);
                commanderApp.returnedText = "Ettercap started 2";
            }
            
        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }
    public void arp()
    {
        CommanderManager.CommandItem item = createItem("arp", "arp");

        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText = "closed";
            
            if (CheckingArgument())
            {
                ComputerManager computer = GetComponentInParent<ComputerManager>();
                Debug.Log("entering arp");
                commanderApp.returnedText =  computer.Arp.ExecuteCommand(commanderApp.arguments);
            }
            
        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }
    public void ftp()
    {
        CommanderManager.CommandItem item = createItem("ftp", "ftp");

        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText = "closed";
            
            if (CheckingArgument())
            {
                ComputerManager computer = GetComponentInParent<ComputerManager>();

                FtpClient ftpClient = gameObject.AddComponent<FtpClient>();
                ftpClient.user = computer;
                commanderApp.returnedText =  ftpClient.ExecuteCommand(commanderApp.arguments);
            }
            
        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }
    public void open()
    {
        CommanderManager.CommandItem item = createItem("open", "open");

        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText = "connection failed";
            
            if (CheckingArgument())
            {
                ComputerManager computer = GetComponentInParent<ComputerManager>();
                
                Debug.Log("entering arp");
                
                // parse arguments to get the username and password. the argument is actually username:username password:password
                string username = commanderApp.arguments[0].Split(':')[1];
                string password = commanderApp.arguments[1].Split(':')[1];
                string ipSender = commanderApp.arguments[3].Split(':')[1];
                
                NetworkManager.ConnectToComputer(username,password,computer,ipSender);
                commanderApp.returnedText =  "connection established";
            }
            
        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }
    public void close()
    {
        CommanderManager.CommandItem item = createItem("close", "close");

        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText = "something went wrong";
            
            if (CheckingArgument())
            {
                ComputerManager computer = GetComponentInParent<ComputerManager>();
                Debug.Log("entering arp");
                string ipReceiver = commanderApp.arguments[0].Split(':')[1];
                string ipSender = commanderApp.arguments[1].Split(':')[1];
                NetworkManager.DisconnectToComputer(ipReceiver,ipSender);
                commanderApp.returnedText =  "connection closed";

            }
            
        });

        commanderApp.commands.Add(item); // Add new item to the manager

        commanderApp.AddToHistory("Your text here", false, 0);
    }
    
    private void executefile()
    {
        CommanderManager.CommandItem item = createItem("sh", "sh");

        item.onProcessEvent.AddListener(delegate
        {
            commanderApp.returnedText = "closed";
            Debug.Log("argument 0 "+ commanderApp.arguments[0]);

            if (CheckingArgument())
            {
                
                if (folders.CheckIfFileExists(commanderApp.arguments[0]))
                {
                    // get only the file name and the extension

                    string filename = folders.GetFileName(commanderApp.arguments[0]);
                    Debug.Log("filename: "+ filename);
                    switch (filename)
                    {
                        case string str when str.StartsWith("DoS.sh"):
                            Debug.Log("argument 1 "+ commanderApp.arguments[1]);
                            ComputerManager computer = NetworkManager.GetComputer(commanderApp.arguments[1]);
                            if(computer != null)
                            {
                                commanderApp.returnedText = "Executing DoS.sh";
                                computer.Explode();
                            }
                        break;

                    }
                }

               
            }
            
        });

        commanderApp.commands.Add(item); // Add new item to the manager

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

    
    public List<string> GetIps(string ipAddress)
    {
        List<string> ips = new List<string>();

        if (ipAddress.EndsWith("/24") || ipAddress.EndsWith(".*") ) // Search for all IPs in the same subnet
        {
            string subnetPrefix = ipAddress.Substring(0, ipAddress.LastIndexOf('.'));
            ips.Add(NetworkManager.routingTable.Keys.ToString());
            //network.GetComputer(subnetPrefix);
        }
        else // Search for a specific IP
        {
            ips.Add(NetworkManager.GetComputer(ipAddress).name); 

        }

        return ips;
    }

    private bool CheckingArgument()
    {
        if (commanderApp.arguments != null && commanderApp.arguments.Length > 0)
        {
            
             if(commanderApp.arguments[0].StartsWith("argument:"))
                 _argument = commanderApp.arguments.Length < 2 ? "":commanderApp.arguments[1];
             else
                _argument = commanderApp.arguments[0];
        }
        else
        {
            Debug.Log("dans le else");

            _argument = "";

        }

        return true;
    }

    private bool checkResultLevel()
    {
        var arg = commanderApp.arguments[0];
        string current_scene =  _scenes[Int32.Parse(_argument)+1];

        switch (arg)
        {
            case "0":
                SceneManager.LoadScene(current_scene);

                return true;
            case "1":
                if (commanderApp.arguments[1] == "admin")
                {
                    SceneManager.LoadScene(current_scene);

                    return true;
                }
                return false;
            case "2":
                
                if ( NetworkManager.GetComputer(commanderApp.arguments[1]) == null)
                {   
                    SceneManager.LoadScene(current_scene);
                    return true;
                }
                return false;
            case "3":
                if (commanderApp.arguments[1] == "admin")
                {
                    SceneManager.LoadScene(current_scene);

                    return true;
                }
                return false;
        }
        return false;
    }


    
}
