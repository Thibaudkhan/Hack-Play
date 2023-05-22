using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Main.Scripts.Network;
using Michsky.DreamOS;
using UnityEngine;
using NetworkManager = Main.Scripts.Network.NetworkManager;
using Random = System.Random;

public class ComputerManager : MonoBehaviour
{
    private bool isUsingComputer = false;
    private Arp arpCommand;

    public string name;
    public Router router;
    
    private ParticleSystem explosionEffect;
    public OsSystem osSystem;
    public bool isConnectedFtp = false;
    public string lastFtpIp = "";
    
    public  string folderPath;
    private string ipAddress;
    private string macAddress; // nouvelle propriété
    public string macMicroship = ""; // nouvelle propriété

    private float minResponseTime = float.MaxValue;
    private float maxResponseTime = float.MinValue;
    private float totalResponseTime;
    private int receivedResponses;
    private Packet currentPacket;
    

    public List<int> ports = new List<int> { 21, 22, 23, 25, 53, 69, 80, 88, 110, 119, 123, 135, 137, 138, 139, 143, 161,
            162, 389, 443, 445, 465, 514, 530, 548, 554, 587, 631, 636, 873, 990, 993, 995, 1080, 1194, 1433, 1434, 1701, 1723, 1755, 1900,
            2000, 2049, 2121, 2181, 2222, 3128, 3268, 3269, 3306, 3389, 3690, 4000, 4333, 4500, 4567, 4662, 4672, 4848, 5000, 5060, 5104, 5190,
            5222, 5223, 5269, 5432, 5555, 5631, 5632, 5800, 5900, 6000, 6001, 6112, 6346, 6666, 6667, 6668, 6669, 6881, 6882, 6883, 6884, 6885, 6886,
            6887, 6888, 6889, 8000, 8008, 8080, 8081, 8443, 8888, 9100, 9418, 9999, 10000, 32768, 32769, 49152, 49153, 49154, 49155, 49156, 49157 };

    private Dictionary<int, bool> openPorts = new Dictionary<int, bool>();
    private string _username = "admin";
    private string _password = "admin";
        

    public string FolderPath
    {
        get { return folderPath; }
    } 
    public string IpAddress
    {
        set { ipAddress = value; }
        get { return ipAddress; }
    }
    public string MacAddress
    {
        get { return macAddress; }
    }
    public string MacMicroship
    {
        get { return macMicroship; }
    }

    public Packet CurrentPacket
    {
        get => currentPacket;
    }  
    public Arp Arp
    {
        get => arpCommand;
    }

    private void Awake()
    {
        macAddress = GenerateMACAddress();
        GenerateNameMac();
        Debug.Log("macAddress"+macAddress);

    }


    // Start is called before the first frame update
    void Start()
    {
        arpCommand = gameObject.AddComponent<Arp>();
        // print the name of all the children gameobject
        foreach (Transform child in transform)
        {
            if (child.name == "Explosion")
            {
                explosionEffect = child.GetComponent<ParticleSystem>();
                explosionEffect.Stop();
            }
        }
        
        
        //explosionEffect = GetComponentInChildren<ParticleSystem>(true);

        //explosionEffect = GetComponentInChildren<ParticleSystem>();
        //.Stop();
        Debug.Log("first EUhhh"+name);
        currentPacket = new Packet(senderIpAddress: "", receiverIpAddress: "");
        foreach (int port in ports)
        {
            
            AddOpenPort(port);
            if (openPorts.Count >= 10)
            {
                break;
            }
        }
        ipAddress = NetworkManager.GetIpAddress(this);
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

    private void GenerateNameMac()
    {
        if (macMicroship == "")
            macMicroship = "router";
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
        Packet packet = new Packet(senderIpAddress:ipAddress, receiverIpAddress:ipAddressReceiver, type:PacketType.PING);
        router.SendPacket(packet);
    }

    public void ReceivePacket(Packet packet)
    {

        if (packet.type == PacketType.PING)
        {
            Packet packetToSend = packet;
            packetToSend.type = PacketType.PING_RESPONSE;
            packetToSend.receiverMacAddress = macAddress;
            //packetToSend.receiverIpAddress = ipAddress;
            SendPacket(packetToSend);
        }
        else if (packet.type == PacketType.HTTP_REQUEST)
        {
            
            packet.Content =
                "GET /index.html HTTP/1.1 \n Host: www.example.com \n User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3  \n Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,;q=0.8 \n Accept-Language: en-US,en;q=0.8";
            string requestContent = packet.Content;
            if (requestContent.Contains("GET /fakepage.html HTTP/1.1"))
            {
                string responseContent = "HTTP/1.1 200 OK\r\nContent-Type: text/html\r\n\r\n<html><head><title>Fake Page</title></head><body><h1></h1></body></html>";
                //GET /index.html HTTP/1.1\r\nHost: www.example.com\r\nUser-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3\r\nAccept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8\r\nAccept-Encoding: gzip, deflate, sdch, br\r\nAccept-Language: en-US,en;q=0.8\r\nConnection: keep-alive\r\n\r\n

                Packet responsePacket = new Packet(senderIpAddress: ipAddress, receiverIpAddress: packet.senderIpAddress, type: PacketType.HTTP_RESPONSE, content: responseContent);
                SendPacket(responsePacket);
            }
        }
        else if (packet.type == PacketType.PING_RESPONSE)
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
        else if (packet.type == PacketType.FTP)
        {
            Debug.Log("FTP received");

            string ipCible = packet.receiverIpAddress;
            ComputerManager computerCible = NetworkManager.GetComputer(ipCible);
            
            foreach (CommanderManager.CommandItem command in computerCible.osSystem.CommanderApp.commands)
            {
                if (command.command == packet.Methode)
                {
                    Debug.Log("invoking a command");

                    computerCible.osSystem.CommanderApp.arguments = packet.Content.Split(',');
                    // stocke the resp
                    command.onProcessEvent.Invoke();

                    break;
                }
            }
            
            packet.type = PacketType.FTP_RESPONSE;
            //senderIpAddress : 192.168.1.1 receiverIpAddress :192.168.1.2 ipCible 192.168.1.2
            Debug.Log("senderIpAddress : "+packet.senderIpAddress+" receiverIpAddress :"+packet.receiverIpAddress+" ipCible "+ipCible);
            packet.receiverIpAddress = packet.senderIpAddress;
            packet.senderIpAddress = ipCible;
            packet.Content = computerCible.osSystem.CommanderApp.returnedText;
            SendPacket(packet);

        }else if (packet.type == PacketType.FTP_RESPONSE)
        {
            currentPacket = packet;
                
        }
    }
    public void SendHttpRequest(Website website,string message,string methode)
    {
        //Debug.Log("SendHttpRequest"+IpAddress);
        ComputerManager websiteComputerManager = website.ComputerManager;

        string receiverIpAddress = websiteComputerManager.IpAddress;
        
        Packet packet = new Packet(senderIpAddress:ipAddress, receiverIpAddress:receiverIpAddress, type:PacketType.HTTP_REQUEST);
        packet.Url = website.FullUrl;
        packet.message = message;
        //packet.data = url;
        router.SendPacket(packet,websiteComputerManager);
    }
    
    public bool SendFTPRequest(Packet packet)
    {
        if (arpCommand.ArpTable.Count > 0)
        {
            Debug.Log("SendFTPRequest");
            string ipRouter = NetworkManager.GetIpAddress(router.ComputerManager);
            string mac = arpCommand.ArpTable[ipRouter];
            ComputerManager midm = router.GetMacComputer(mac);
            Debug.Log("midm"+midm.name);
            string filename= "/"+midm.name + "log.txt";
            SendPacketToComputer(packet, midm,filename);
            //midm.SendPacketToComputer(packet,midm,filename);
        }else if (!router.SendPacket(packet))
        {
            return false;
        }
        return true;

    }

    public void SendPacket(Packet packet)
    {
        router.SendPacket(packet);
    }
    public void SendPacketToComputer(Packet packet,ComputerManager computerManager,string filename)
    {
        Debug.Log("SendPacketToComputer");
        string filePath = Application.streamingAssetsPath + filename;
        string logMessage = $"Sender: {packet.senderIpAddress}\n" +
                            $"Receiver: {packet.receiverIpAddress}\n" +
                            $"Message: {packet.message}\n" +
                            $"Url: {packet.Url}\n" +
                            $"Methode: {packet.Methode}\n" +
                            $"Type: {packet.type}\n\n";


        File.AppendAllText(filePath, logMessage);
        router.SendPacket(packet,computerManager,filename);
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




    // Ajouter un port
    public void AddOpenPort(int port)
    {
        if (!openPorts.ContainsKey(port))
        {
            openPorts.Add(port, true);
        }
    }

    // Supprimer un port
    public void RemoveOpenPort(int port)
    {
        if (openPorts.ContainsKey(port))
        {
            openPorts.Remove(port);
        }
    }

    // Vérifier si un port est ouvert
    public bool IsPortOpen(int port)
    {
        if (openPorts.ContainsKey(port))
        {
            return openPorts[port];
        }
        return false;
    }
    
    public string CheckOpenPorts(int? port = null)
    {
        string result = "";

        if (port == null)
        {
            foreach (var openPort in openPorts)
            {
                result += "Port " + openPort + " is open\n";
            }
        }
        else
        {
            if (openPorts.Keys.Contains<int>(port.Value))
            {
                result = "Port " + port.Value + " is open";
            }
            else
            {
                result = "Port " + port.Value + " is closed";
            }
        }

        return result;
    }
    
    
    public String OpenPort(int port)
    {
        // Check if the port is already open
        if (openPorts.ContainsKey(port) && openPorts[port])
        {
            return "Port {port} is already open.";
        }


        openPorts[port] = true;
        return "Port {port} is now open.";
    }


    public void Explode()
    {
        explosionEffect.Play();
        float stopDelay = explosionEffect.main.duration;
        Invoke("StopParticleSystem", stopDelay);

    }
    
    private void StopParticleSystem()
    {
        explosionEffect.Stop();
        Destroy(gameObject);
    }


    private void OnDestroy()
    {
        NetworkManager.RemoveComputerFromRoutingTable(this);
    }

    public bool CheckCredentials(string username, string password)
    {
        Debug.Log("CheckCredentials"+isConnectedFtp);
        if (isConnectedFtp)
            return false;
        
        Debug.Log("Checkusername"+ _username);

        if(username != _username || password != _password)
            return false;
        

        return true;
    }
}
