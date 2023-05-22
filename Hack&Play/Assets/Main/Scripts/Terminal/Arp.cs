using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arp : MonoBehaviour
{
    private Dictionary<string, string> arpTable;
    private string response = ""; 
    public Dictionary<string, string> ArpTable
    {
        get => arpTable;
    }




    private void Start()
    {
        arpTable = new Dictionary<string, string>();
    }

    public string ExecuteCommand(string[] args)
    {
        if (args.Length == 0)
        {
            Debug.Log("No arguments provided.");
            return response;
        }

        Debug.Log("commande " + args[0]);
        string command = args[0];
        switch (command)
        {
            case "-a":
                PrintTable();
                break;
            case "-d":
                if (args.Length < 2)
                {
                    response ="Missing IP address argument for -d command.";
                    return response;
                }
                string ipAddress = args[1];
                RemoveEntry(ipAddress);
                break;
            case "-s":
                if (args.Length < 3)
                {
                    response = "Missing IP address or MAC address argument for -s command.";
                    return response;
                }
                string newIpAddress = args[1];
                string newMacAddress = args[2];
                SetARPEntry(newIpAddress, newMacAddress);
                break;
            case "-n":
                ARPNeighbor();
                break;
            case "-v":
                ARPLocal();
                break;
            default:
                Debug.Log("Invalid command.");
                break;
        }
        return response;
    }

    public void SetARPEntry(string ipAddress, string macAddress)
    {
        arpTable[ipAddress] = macAddress;
        response =$"ARP entry set: IP Address = {ipAddress}, MAC Address = {macAddress}";
    }

    public void RemoveEntry(string ipAddress)
    {
        if (arpTable.ContainsKey(ipAddress))
        {
            arpTable.Remove(ipAddress);
            response = $"ARP entry removed: IP Address = {ipAddress}";
        }
        else
        {
            response = $"No ARP entry found for IP Address = {ipAddress}";
        }
    }

    public void ClearTable()
    {
        arpTable.Clear();
        response = "ARP table cleared.";
    }

    public void PrintTable()
    {
        response = $"IP Address:  - MAC Address: ";
        foreach (var entry in arpTable)
        {
            response += $"IP Address: {entry.Key} - MAC Address: {entry.Value}";
        }
    }

    public void ARPNeighbor()
    {
        // Logic for ARP neighbor functionality
        response = "Executing ARP neighbor functionality.";
    }

    public void ARPLocal()
    {
        // Logic for ARP local functionality
        response = "Executing ARP local functionality.";
    }
}
