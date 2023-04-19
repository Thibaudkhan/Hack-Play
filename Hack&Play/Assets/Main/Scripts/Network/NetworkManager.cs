using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    // The IP address prefix
    public string ipAddressPrefix = "192.168.1.";

    // The next available IP address suffix
    private int nextIpAddressSuffix = 1;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Generate a new IP address suffix
    private int GenerateIpAddressSuffix()
    {
        int suffix = nextIpAddressSuffix;
        nextIpAddressSuffix++;
        return suffix;
    }
    
    public void RemoveComputerFromRoutingTable(ComputerManager computer)
    {
        // Remove the computer from the routing table
        if (routingTable.ContainsValue(computer))
        {
            var key = routingTable.FirstOrDefault(x => x.Value == computer).Key;
            routingTable.Remove(key);
        }
    }


    // Assign an IP address to a computer
    public string AssignIpAddress(ComputerManager computer)
    {
        // Generate a new IP address suffix
        int suffix = GenerateIpAddressSuffix();

        // Build the IP address string
        string ipAddress = ipAddressPrefix + suffix.ToString();

        // Add the computer and IP address to the dictionaries
        computers.Add(computer);
        ipAddresses.Add(computer, ipAddress);
        routingTable.Add(ipAddress, computer);

        return ipAddress;
    }

    // Get the IP address of a computer
    public string GetIpAddress(ComputerManager computer)
    {
        if (ipAddresses.ContainsKey(computer))
        {
            return ipAddresses[computer];
        }
        return "";
    }

    // Get the computer that corresponds to an IP address
    public ComputerManager GetComputer(string ipAddress)
    {

        ipAddress = ipAddress.Trim();
        if (routingTable.ContainsKey(ipAddress))
        {
            return routingTable[ipAddress];
        }
        return null;
    }

    // Send a message from one computer to another
    public void SendMessage(ComputerManager sender, string recipientIpAddress, string message)
    {
        ComputerManager recipient = GetComputer(recipientIpAddress);

        if (recipient != null)
        {
            recipient.ReceiveMessage(sender.IpAddress, message);
        }
        else
        {
            Debug.LogError("Recipient not found: " + recipientIpAddress);
        }
    }
}
