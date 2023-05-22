using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Main.Scripts.Network;
using Unity.VisualScripting;
using UnityEngine;

public class Router : MonoBehaviour
{
    // The list of computers connected to this router
    private List<ComputerManager> computers = new List<ComputerManager>();

    private ComputerManager _computerManager;

    public ComputerManager ComputerManager
    {
        get => _computerManager;
        set => _computerManager = value;
    }

    // The mapping of port numbers to computers
    private Dictionary<int, ComputerManager> portMappings = new Dictionary<int, ComputerManager>();

    // The network manager that manages this router

    private Dictionary<string, ComputerManager> macToComputer = new Dictionary<string, ComputerManager>();
    private StreamWriter streamWriter;
    //private string filePath = Application.streamingAssetsPath +"log.txt";
    private string filePath;

    void Start()
    {
        _computerManager = gameObject.AddComponent<ComputerManager>();

        //OsSystem osSystem = _computerManager.AddComponent<OsSystem>();
        //FileManager fileManager = _computerManager.AddComponent<FileManager>();
        //osSystem.folders = fileManager;
        //NetworkManager.AssignIpAddress(_computerManager);
        AddComputerToPort(19,_computerManager);

    }

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
        NetworkManager.AssignIpAddress(computer);
    }

    // Remove a computer from a port on this router
    public void RemoveComputerFromPort(int port)
    {
        if (portMappings.ContainsKey(port))
        {
            ComputerManager computer = portMappings[port];
            portMappings.Remove(port);
            computers.Remove(computer);
            NetworkManager.RemoveComputerFromRoutingTable(computer);
            Debug.Log("Computer " + computer.name + " removed from port " + port);
        }
        else
        {
            Debug.LogError("No computer connected to port " + port);
        }
    }
    
    public string GetIPAddress(ComputerManager computer)
    {
        return NetworkManager.GetIpAddress(computer);
    }

    public ComputerManager GetComputer(string ipAddress)
    {
        // return NetworkManager.GetComputer(ipAddress)
        return NetworkManager.GetComputer(ipAddress);
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
    

    // ReSharper disable Unity.PerformanceAnalysis
    public bool SendPacket(Packet packet,ComputerManager computerManager = null,string file ="/log1.txt")
    {
        Debug.Log("send packet "+packet.receiverIpAddress);
        bool response = false;
        // Get the ComputerManager corresponding to the destination IP address
        ComputerManager recipient = GetComputer(packet.receiverIpAddress);
        if(recipient != null){
            string macAddress = recipient.MacAddress;
            // Check if the MAC address of the recipient is known
            if (macToComputer.ContainsKey(macAddress) )
            {
                //filePath = Application.streamingAssetsPath + "/" + macAddress + ".txt";
                response = true;
            }
        }
        if(recipient == null){
            recipient = computerManager;
            response = true;
        }
        Debug.Log("send ReceivePacket");

        filePath = Application.streamingAssetsPath + file;
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
        }

        recipient.ReceivePacket(packet);
        string logMessage = $"Sender: {packet.senderIpAddress}\n" +
                            $"Receiver: {packet.receiverIpAddress}\n" +
                            $"Message: {packet.message}\n" +
                            $"Url: {packet.Url}\n" +
                            $"Methode: {packet.Methode}\n" +
                            $"Type: {packet.type}\n\n";


        File.AppendAllText(filePath, logMessage);

        return response;
    }

}
