using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    // The list of computers in the network
    public List<ComputerManager> computers = new List<ComputerManager>();

    // The IP addresses assigned to each computer
    public Dictionary<ComputerManager, string> ipAddresses = new Dictionary<ComputerManager, string>();

    // The routing table that maps IP addresses to computers
    public Dictionary<string, ComputerManager> routingTable = new Dictionary<string, ComputerManager>();

    
    
    // Start is called before the first frame update
    void Start()
    {
        // Set the singleton instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        // Assign IP addresses to each computer and add them to the routing table
        foreach (ComputerManager computer in computers)
        {
            string ipAddress = GenerateIPAddress();
            ipAddresses.Add(computer, ipAddress);
            routingTable.Add(ipAddress, computer);
        }
    }

    // Generate a random IP address
    private string GenerateIPAddress()
    {
        string ipAddress = "";

        for (int i = 0; i < 4; i++)
        {
            int octet = Random.Range(0, 256);
            ipAddress += octet.ToString();

            if (i < 3)
            {
                ipAddress += ".";
            }
        }

        return ipAddress;
    }

    // Get the IP address of a computer
    public string GetIPAddress(ComputerManager computer)
    {
        if (ipAddresses.ContainsKey(computer))
        {
            return ipAddresses[computer];
        }
        else
        {
            return "";
        }
    }

    // Get the computer that corresponds to an IP address
    public ComputerManager GetComputer(string ipAddress)
    {
        if (routingTable.ContainsKey(ipAddress))
        {
            return routingTable[ipAddress];
        }
        else
        {
            return null;
        }
    }

    // Send a message from one computer to another
    public void SendMessage(ComputerManager sender, string recipientIPAddress, string message)
    {
        ComputerManager recipient = GetComputer(recipientIPAddress);

        if (recipient != null)
        {
            recipient.ReceiveMessage(sender.IpAddress, message);
        }
        else
        {
            Debug.LogError("Recipient not found: " + recipientIPAddress);
        }
    }
}
