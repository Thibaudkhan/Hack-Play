using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main.Scripts.Network
{
    public static class NetworkManager 
    {
        // The list of computers in the network
        public static List<ComputerManager> computers = new List<ComputerManager>();

        // The IP addresses assigned to each computer
        public static Dictionary<ComputerManager, string> ipAddresses = new Dictionary<ComputerManager, string>();

        // The routing table that maps IP addresses to computers
        public static Dictionary<string, ComputerManager> routingTable = new Dictionary<string, ComputerManager>();

        // The IP address prefix
        public static string ipAddressPrefix = "192.168.1.";

        // The next available IP address suffix
        private static int nextIpAddressSuffix = 1;

        
        public static List<Website> listOfGOwebsites = new List<Website>();

        // Generate a new IP address suffix
        private static int GenerateIpAddressSuffix()
        {
            int suffix = nextIpAddressSuffix;
            nextIpAddressSuffix++;
            return suffix;
        }
    
        public static void RemoveComputerFromRoutingTable(ComputerManager computer)
        {
            // Remove the computer from the routing table
            if (routingTable.ContainsValue(computer))
            {
                var key = routingTable.FirstOrDefault(x => x.Value == computer).Key;
                routingTable.Remove(key);
            }
        }


        // Assign an IP address to a computer
        public static string AssignIpAddress(ComputerManager computer)
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
        public static string GetIpAddress(ComputerManager computer)
        {
                Debug.Log("computer namez"+computer.name);
            if (ipAddresses.ContainsKey(computer))
            {
                return ipAddresses[computer];
            }
            return "";
        }

        // Get the computer that corresponds to an IP address
        public static ComputerManager GetComputer(string ipAddress)
        {

            ipAddress = ipAddress.Trim();
            if (routingTable.ContainsKey(ipAddress))
            {
                return routingTable[ipAddress];
            }
            return null;
        }

        // Send a message from one computer to another
        public static void SendMessage(ComputerManager sender, string recipientIpAddress, string message)
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

        public static void ConnectToComputer(string username, string password,ComputerManager cible,string ipSender)
        {
            Debug.Log("ConnectToComputer"+username+ password);
            if (!cible.CheckCredentials(username, password))
            {
                return;
            }
            Debug.Log("after check");

            cible.isConnectedFtp = true;
            string ipReceiver = GetIpAddress(cible);
            ComputerManager sender = GetComputer(ipSender);
            sender.isConnectedFtp = true;
            sender.lastFtpIp = ipReceiver;
            Debug.Log("current guy ip "+ipSender+" the cible"+sender.lastFtpIp);
            
            
        }
        
        public static void DisconnectToComputer(string ipReceiver,string ipSender)
        {
            ComputerManager sender = GetComputer(ipSender);
            ComputerManager receiver = GetComputer(ipReceiver);
            Debug.Log("Dico ToComputer"); 
            
            if(receiver.isConnectedFtp && sender.isConnectedFtp)
            {
                receiver.isConnectedFtp = false;
                sender.isConnectedFtp = false;
                sender.lastFtpIp = "";
            }
            
        }

       

    }
}
