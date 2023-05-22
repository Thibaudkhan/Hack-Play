using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using NetworkManager = Main.Scripts.Network.NetworkManager;


public static class Ettercap 
{
    // Start is called before the first frame update
    public static string[] arguments;
    private static Dictionary<string, Action> typeAttackFunctions;
    private static Dictionary<string, Action> networkattackFunctions;
    private static string[] attacks;
    private static string ipHacker;
    private static string ipCible;
    private static string ipRouter;
    private static AttackFunctions attackFunctions;

    static Ettercap()
    {
    }
    
    public static void Start()
    {
        attackFunctions = new AttackFunctions();
        typeAttackFunctions = new Dictionary<string, Action>
        {
            { "arp", attackFunctions.ARPAttack },
            { "dns", attackFunctions.DNSAttack },
            { "dhcp", attackFunctions.DHCPAttack },
            { "ssh", attackFunctions.SSHAttack }
        };
        networkattackFunctions = new Dictionary<string, Action>
        {
            { "remote", attackFunctions.RemoteAttack },
            { "locale", attackFunctions.LocaleAttack },
            { "gateway", attackFunctions.GatewayAttack },
            { "unicast", attackFunctions.UnicastAttack },
            { "broadcast", attackFunctions.BroadcastAttack }
        };
    }

    public static void Main(string[] arg,ComputerManager computerHacker)
    {
        Debug.Log("Statring Ettercap");
        Debug.Log(arg.Length);

        arguments = arg;
        if(arguments.Length == 0)
        {
            Debug.Log("No arguments");
            return;
        }
        string[] ips  =  arguments[arguments.Length - 1].Split('/');
        Debug.Log("argu "+arguments[arguments.Length - 1]);
        if( ips.Length < 3)
        {
            Debug.Log("Missing arguments");
            return;
        }
        string ip1 = ips[1];
        string ip2 = ips[2];

        Start();

        
        for (int i = 0;  i < arguments.Length;i++)
        {
            switch (arguments[i])
            {
                case "-M":
                    // parse the string after the -M to get the attack type and the target
                    
                    attacks = arguments[i + 1].Split(':');
                    attackFunctions.IPCible = ip1;
                    attackFunctions.IPRouter = ip2;
                    attackFunctions.ComputerHacker = computerHacker;
                    Debug.Log(" ip router "+ ip2 + "ip cible "+ ips[0] + " ip 2 " +ips[2]+" ip 3 " +ips[3]+ " all arg "+arguments[arguments.Length - 1]);
                    ExecuteAttack(attacks[0]);
                    ExecuteAttack(attacks[1]);
                    break;
            }
                
        }
    }

    private static void ExecuteAttack(string argument)
    {
        Debug.Log("ExecuteAttack "+argument);

        if (typeAttackFunctions.ContainsKey(argument))
        {
            typeAttackFunctions[argument].Invoke();
        }
        else
        {
            Console.WriteLine("Type d'attaque non pris en charge : " + argument);
        }
    }
    
}

public  class AttackFunctions
{
    public string IPHacker
    {
        get => ipHacker;
        set => ipHacker = value;
    }

    public string IPRouter
    {
        get => ipRouter;
        set => ipRouter = value;
    }

    public string IPCible
    {
        get => ipCible;
        set => ipCible = value;
    }
    public ComputerManager ComputerHacker
    {
        get => computerHacker;
        set => computerHacker = value;
    }

    string ipHacker;
    string ipRouter;
    string ipCible;
    ComputerManager computerHacker;


    public  void ARPAttack()
    {
        Debug.Log("ARPAttack cible "+ipCible+" router "+ipRouter+ " count "+NetworkManager.computers.Count);
        ComputerManager computerCible = NetworkManager.GetComputer(ipCible);
        if(computerCible == null)
            return;
        
        computerCible.Arp.SetARPEntry(ipRouter,computerHacker.MacAddress);
        // Logique pour l'attaque ARP
    }

    public  void DNSAttack()
    {
        // Logique pour l'attaque DNS
    }

    public  void DHCPAttack()
    {
        // Logique pour l'attaque DHCP
    }

    public  void SSHAttack()
    {
        // Logique pour l'attaque SSH
    }
    
    public  void RemoteAttack()
    {
        // Logique pour l'attaque ARP
    }

    public  void LocaleAttack()
    {
        // Logique pour l'attaque DNS
    }

    public  void GatewayAttack()
    {
        // Logique pour l'attaque DHCP
    }

    public  void UnicastAttack()
    {
        // Logique pour l'attaque SSH
    }

    public  void BroadcastAttack()
    {
        throw new NotImplementedException();
    }
}


