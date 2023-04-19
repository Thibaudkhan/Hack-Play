using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Network;
using UnityEngine;
using Random = System.Random;

public class ComputerManager : MonoBehaviour
{
    private bool isUsingComputer = false;

    public string name;
    public Router router;
    
    public  string folderPath;
    private string ipAddress;
    private string macAddress; // nouvelle propriété

    private float minResponseTime = float.MaxValue;
    private float maxResponseTime = float.MinValue;
    private float totalResponseTime;
    private int receivedResponses;
    private Packet currentPacket;
    public string FolderPath
    {
        get { return folderPath; }
    } 
    public string IpAddress
    {
        get { return ipAddress; }
    }
    public string MacAddress
    {
        get { return macAddress; }
    }

    public Packet CurrentPacket
    {
        get => currentPacket;
    }

    private void Awake()
    {
        macAddress = GenerateMACAddress();
        Debug.Log("macAddress"+macAddress);
    }


    // Start is called before the first frame update
    void Start()
    {
        folderPath = Application.streamingAssetsPath +"/OsData/"+ name + "/Os/";
        Debug.Log("first EUhhh"+name);
        currentPacket = new Packet(senderIpAddress: "", receiverIpAddress: "",
            type: "");
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
    
    public string Ping(string ipAddressReceiver)
    {
        ComputerManager recipient = router.GetComputer(ipAddressReceiver);
        Debug.Log("recipient :"+recipient.name);
        if (recipient != null)
        {
            int numberOfPings = 4;
            float timeout = 1f;

            for (int i = 0; i < numberOfPings; i++)
            {
                SendPing(ipAddressReceiver);
                float elapsedTime = 0;

                while (elapsedTime < timeout)
                {
                    elapsedTime += Time.deltaTime;
                }
            }

            Packet packet = recipient.CurrentPacket;
            receivedResponses = packet.ReceivedResponses;
            totalResponseTime = packet.TotalResponseTime;

            minResponseTime = packet.MinResponseTime;
            maxResponseTime = packet.MaxResponseTime;
            float averageResponseTime = recipient.GetAvgPingResponseTime();
            float packetLossPercentage = recipient.GetPacketLossPercentage();

            return $"Ping statistics for {ipAddressReceiver}:\n" +
                   $"    Packets: Sent = {numberOfPings}, Received = {receivedResponses}, Lost = {numberOfPings - receivedResponses} ({packetLossPercentage}% loss),\n" +
                   $"Approximate round trip times in milli-seconds:\n" +
                   $"    Minimum = {minResponseTime}ms, Maximum = {maxResponseTime}ms, Average = {averageResponseTime}ms";
        }
        return "Recipient not found: " + ipAddressReceiver;
    }

    private void SendPing(string ipAddressReceiver)
    {
        Packet packet = new Packet(senderIpAddress:ipAddress, receiverIpAddress:ipAddressReceiver, type:"PING");
        router.SendPacket(packet);
    }

    public void ReceivePacket(Packet packet)
    {

        if (packet.type == "PING")
        {
            Packet packetToSend = packet;
            packetToSend.type = "PING_RESPONSE";
            packetToSend.receiverMacAddress = macAddress;
            //packetToSend.receiverIpAddress = ipAddress;
            SendPacket(packetToSend);
        }
        else if (packet.type == "PING_RESPONSE")
        {
            float responseTime = Time.time - packet.timestamp;
            if (responseTime >= 0)
            {
                if (responseTime < minResponseTime)
                {
                    minResponseTime = responseTime;
                    currentPacket.MinResponseTime = minResponseTime;

                }
                if (responseTime > maxResponseTime)
                {
                    maxResponseTime = responseTime;
                    currentPacket.MaxResponseTime = maxResponseTime;

                }
                totalResponseTime += responseTime;
                receivedResponses++;

                currentPacket.ReceivedResponses = receivedResponses;
                currentPacket.TotalResponseTime = totalResponseTime;
            }
        }
    }

    private void SendPacket(Packet packet)
    {
        router.SendPacket(packet);
    }
    public float GetMinPingResponseTime()
    {
        return minResponseTime;
    }

    public float GetMaxPingResponseTime()
    {
        return maxResponseTime;
    }

    public float GetAvgPingResponseTime()
    {
        if (receivedResponses > 0)
            return totalResponseTime / receivedResponses;
        return 0;
    }

    public float GetPacketLossPercentage()
    {
        if (receivedResponses > 0)
            return ((1 - ((float)receivedResponses / 4)) * 100);
        return 100;
    }






    
    
    


}
