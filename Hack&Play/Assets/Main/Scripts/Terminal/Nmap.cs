using System;
using System.Collections.Generic;
using System.Net;
using Main.Scripts.Network;
using UnityEngine;

namespace Main.Scripts.Terminal
{
    public class Nmap : MonoBehaviour
    {
        public string[] arguments;
        private string _ip;
        private bool _readPorts = true;
        private bool _arpPing = true;

        private int _numberOfPorts = 100;

        public void Main(string[] getArguments)
        {
            arguments = getArguments;
            Debug.Log("Starting Nmap");

            if(arguments.Length == 0)
            {
                Debug.Log("No arguments");
                return;
            }
            // loop in list argument and check if it is a valid argument and chek if a value start with 192.168.1
            foreach (string argument in arguments)
            {
                Debug.Log("The argument "+argument);
                if (argument.StartsWith("192."))
                {
                    _ip = argument;
                }
                
                if (argument == "-sn")
                {
                    
                    _readPorts = false;
                    Debug.Log("The _readPorts "+_readPorts);

                }
                if (argument == "--disable-arp-ping")
                {
                    
                    _arpPing = false;

                }

                if (argument.StartsWith("--top-ports="))
                {
                   // split argument to gat infromation after the = and get the value after the =
                    string[] splitArgument = argument.Split('=');
                    _numberOfPorts = Int32.Parse(splitArgument[1]);
                }
            }
            Debug.Log("The IP "+_ip+_readPorts);


        }
        

        // Discover all hosts on the network
        public List<string> DiscoverHosts()
        {
            List<string> listOfIps = new List<string>();
            Debug.Log("The IP "+_ip);
            List<string> formatResponse = new List<string>(); 
             // Search for all IPs in the same subnet
            Debug.Log("here"+NetworkManager.routingTable.Keys + _readPorts);
            foreach (string subnet in NetworkManager.routingTable.Keys)
            {

                Debug.Log("not continuing");

                formatResponse.Add(FormatResponse(subnet));

                
                if (subnet == _ip)
                {
                    listOfIps.Add(subnet);
                }
                if (_ip.EndsWith("/24") || _ip.EndsWith(".*"))
                {
                    listOfIps.Add(subnet);
                }
                
                if (_readPorts)
                {
                    // transfrom the open port in to a string and add it to the list
                    var portStrings = DiscoverOpenPorts(subnet, NetworkManager.GetComputer(subnet));

                    formatResponse.AddRange(portStrings);

                }

                

            }

            return listOfIps.Count > 0 ? formatResponse : new List<string>(){ "closed" };


        }
        
        private string FormatResponse(string subnet)
        {
         /*
         * Nmap scan report for 192.168.0.10
           Host is up (0.0061s latency).
           MAC Address: 80:1F:12:58:69:25 (Microchip Technology)

         */
         ComputerManager computer = NetworkManager.GetComputer(subnet);
         return "Nmap scan report for "+subnet+"\nHost is up (0.0061s latency).\nMAC Address: "+ computer.MacAddress+" ("+computer.MacMicroship+")\n";
        } 

        // Discover open ports on a host
        public List<string> DiscoverOpenPorts(string ipAddress, ComputerManager computer)
        {
            Debug.Log("enter discover open ports");
            List<string> openPorts = new List<string>();
            // Check if the ports are open
            openPorts.Add("PORT     STATE    SERVICE");
            foreach (var port in computer.ports)
            {
                openPorts.Add(FormatResponsePort(port, computer.IsPortOpen(port)));
            }
            
            return openPorts;
        }
        
        private string FormatResponsePort(int port, bool isOpen)
        {
             return port+"/tcp   "+(isOpen ? "open     " : "filtered")+" ftp";
        } 

        // Send data to a port on a host
        public void SendData(string ipAddress, int port, string data)
        {
            Debug.Log($"Sending data to {ipAddress}:{port}: {data}");
        }

        // Receive data from a port on a host
        public string ReceiveData(string ipAddress, int port)
        {
            return $"Received data from {ipAddress}:{port}: hello world";
        }
        
        
        
        
        
    }
    
   

}