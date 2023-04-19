using System.Collections;
using System.Collections.Generic;
using Main.Scripts.Network;
using UnityEngine;

public class Router : MonoBehaviour
{
    // The list of computers connected to this router
    private List<ComputerManager> computers = new List<ComputerManager>();

    // The mapping of port numbers to computers
    private Dictionary<int, ComputerManager> portMappings = new Dictionary<int, ComputerManager>();

    // The network manager that manages this router
    public NetworkManager networkManager;

    private Dictionary<string, ComputerManager> macToComputer = new Dictionary<string, ComputerManager>();

    
    public void AddMacComputer(ComputerManager computer)
    {
        if (computer != null && !macToComputer.ContainsKey(computer.MacAddress))
        {
            macToComputer.Add(computer.MacAddress, computer);
        }
    }


    
    public void RemoveMacComputer(ComputerManager computer)
    {
        macToComputer.Remove(computer.MacAddress);
    }

    public bool HasMacAddress(string macAddress)
    {
        return macToComputer.ContainsKey(macAddress);
    }

    public ComputerManager GetMacComputer(string macAddress)
    {
        if (HasMacAddress(macAddress))
        {
            return macToComputer[macAddress];
        }
        return null;
    }
    
    // Add a computer to a port on this router
    public void AddComputerToPort(int port, ComputerManager computer)
    {

        // Check if the port is already occupied
        if (portMappings.ContainsKey(port))
        {
            Debug.LogError("Port " + port + " is already occupied by " + portMappings[port].name);
            return;
        }

        // Add the computer to the list of connected computers
        computers.Add(computer);
        AddMacComputer(computer);

        // Add the port mapping
        portMappings.Add(port, computer);

        networkManager.AssignIpAddress(computer);
    }

    // Remove a computer from a port on this router
    public void RemoveComputerFromPort(int port)
    {
        if (portMappings.ContainsKey(port))
        {
            ComputerManager computer = portMappings[port];
            portMappings.Remove(port);
            computers.Remove(computer);
            networkManager.RemoveComputerFromRoutingTable(computer);
            Debug.Log("Computer " + computer.name + " removed from port " + port);
        }
        else
        {
            Debug.LogError("No computer connected to port " + port);
        }
    }
    
    public string GetIPAddress(ComputerManager computer)
    {
        return networkManager.GetIpAddress(computer);
    }

    public ComputerManager GetComputer(string ipAddress)
    {
        return networkManager.GetComputer(ipAddress);
    }


    // Send a message from one computer to another
    public void SendMessage(int sourcePort, int destinationPort, string message)
    {
        if (portMappings.ContainsKey(destinationPort))
        {
            ComputerManager recipient = portMappings[destinationPort];
            ComputerManager sender = portMappings[sourcePort];

            recipient.ReceiveMessage(sender.IpAddress, message);
        }
        else
        {
            Debug.LogError("Destination port " + destinationPort + " is not connected to any computer.");
        }
    }
    

    public void SendPacket(Packet packet)
    {
        string destinationIpAddress = packet.receiverIpAddress;
        Debug.Log("Sending packet to " + destinationIpAddress);
        // Get the ComputerManager corresponding to the destination IP address
        ComputerManager recipient = GetComputer(destinationIpAddress);
        string macAddress = recipient.MacAddress;
        // Check if the MAC address of the recipient is known
        if (macToComputer.ContainsKey(macAddress) )
        {
            // Send the packet to the recipient
            recipient.ReceivePacket(packet);
        }
    }

}
