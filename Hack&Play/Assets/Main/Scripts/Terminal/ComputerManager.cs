using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class ComputerManager : MonoBehaviour
{
    private bool isUsingComputer = false;

    public string name;
    public NetworkManager networkManager;

    public  string folderPath;
    private string ipAddress;
    private string macAddress; // nouvelle propriété

    public string FolderPath
    {
        get { return folderPath; }
    } 
    public string IpAddress
    {
        get { return ipAddress; }
        set { ipAddress = value; }
    }
    // public ComputerManager(string name,NetworkManager networkManager)
    // {
    //     this.name = name;
    //     this.networkManager = networkManager;
    //
    // }

    

// Start is called before the first frame update
    void Start()
    {
        folderPath = Application.streamingAssetsPath +"/OsData/"+ name + "/Os/";
        macAddress = GenerateMACAddress();
        Debug.Log("first EUhhh"+name);
    }

    // Update is called once per frame

    public void UseComputer()
    {
        isUsingComputer = !isUsingComputer;
        
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isUsingComputer); // or false
        }
    }

    void ExitComputer()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false); // or false
        }
    }
    
    
    private static string GenerateMACAddress()
    {
        Random random = new Random();
        byte[] mac = new byte[6];
        random.NextBytes(mac);
        mac[0] = (byte)(mac[0] & 0xFC); // ensure a unicast address
        mac[0] = (byte)(mac[0] | 0x02); // set the "locally administered" bit
        return string.Join(":", mac.Select(b => b.ToString("X2")));
    }

    public void ReceiveMessage(string senderIp,string message)
    {
        // Check if the message is for this computer
        if (senderIp == ipAddress)
        {
            // Display the message
            Debug.Log("Message received from " + senderIp + ": " + message);
        }

    }


}
