namespace Main.Scripts.Network
{
using System.Collections.Generic;
using UnityEngine;

public class Router : ComputerManager
{
    // List of dictionaries for each port on the router
    private List<Dictionary<string, ComputerManager>> portList = new List<Dictionary<string, ComputerManager>>();
    private NetworkManager networkManager;

    // public Router (string name, NetworkManager networkManager) : base(name, networkManager)
    // {
    //     this.networkManager = networkManager;
    // }
    
    void Start()
    {
        // Initialize the port list with empty dictionaries
        for (int i = 0; i < 4; i++)
        {
            portList.Add(new Dictionary<string, ComputerManager>());
        }
    }

    // Method to add a computer to a port
    public void AddComputerToPort(int port, ComputerManager computer)
    {
        // Generate a unique IP address for the computer
        string ipAddress = GenerateIPAddress();

        // Add the computer to the dictionary for the specified port
        portList[port].Add(ipAddress, computer);

        // Add the computer and IP address to the routing table
        networkManager.ipAddresses.Add(computer, ipAddress);
        networkManager.routingTable.Add(ipAddress, computer);
    }

    // Method to remove a computer from a port
    public void RemoveComputerFromPort(int port, ComputerManager computer)
    {
        // Get the IP address of the computer
        string ipAddress = networkManager.ipAddresses[computer];

        // Remove the computer from the dictionary for the specified port
        portList[port].Remove(ipAddress);

        // Remove the computer and IP address from the routing table
        networkManager.ipAddresses.Remove(computer);
        networkManager.routingTable.Remove(ipAddress);
    }

    // Method to get the computer associated with an IP address
    public ComputerManager GetComputerForIPAddress(string ipAddress)
    {
        // Check if the IP address is in the routing table
        if (networkManager.routingTable.ContainsKey(ipAddress))
        {
            return networkManager.routingTable[ipAddress];
        }
        else
        {
            Debug.LogError("IP address not found: " + ipAddress);
            return null;
        }
    }

    // Method to make a connection between two IP addresses
    public void Connect(string ipAddress1, string ipAddress2)
    {
        // Get the computers associated with the IP addresses
        ComputerManager computer1 = GetComputerForIPAddress(ipAddress1);
        ComputerManager computer2 = GetComputerForIPAddress(ipAddress2);

        // Check if both computers are connected to the same router
        if (computer1 is Router && computer2 is Router && computer1 == computer2)
        {
            // Get the ports for each computer
            int port1 = GetPortForIPAddress(ipAddress1);
            int port2 = GetPortForIPAddress(ipAddress2);

            // Connect the two ports together
            portList[port1].Add(ipAddress2, computer2);
            portList[port2].Add(ipAddress1, computer1);

            Debug.Log("Connected " + ipAddress1 + " and " + ipAddress2);
        }
        else
        {
            Debug.LogError("Cannot connect " + ipAddress1 + " and " + ipAddress2 +
                           ". Computers are not connected to the same router.");
        }
    }

    // Method to generate a unique IP address
    private string GenerateIPAddress()
    {
        // Generate a random IP address in the format "xxx.xxx.xxx.xxx"
        string ipAddress = "";

        for (int i = 0; i < 4; i++)
        {
            ipAddress += UnityEngine.Random.Range(0, 255);
            if (i < 3)
            {
                ipAddress += ".";
            }
        }

        // Check if the IP address is already in use
        if (networkManager.routingTable.ContainsKey(ipAddress))
        {
            // If the IP address is already in use, generate a new one
            return GenerateIPAddress();
        }
        else
        {
            return ipAddress;
        }
    }

// Method to get the port associated with an IP address
    private int GetPortForIPAddress(string ipAddress)
    {
        // Check each dictionary in the port list for the IP address
        for (int i = 0; i < portList.Count; i++)
        {
            if (portList[i].ContainsKey(ipAddress))
            {
                return i;
            }
        }

        // If the IP address is not found, return -1
        return -1;
    }
}


}