using System.Collections.Generic;

namespace Main.Scripts.Network
{
    
    // create an enum of differtent Type  like FTP STP HTTP 
    
    public enum PacketType
    {
        FTP,
        FTP_RESPONSE,
        STP,
        HTTP,
        HTTP_RESPONSE,
        HTTP_REQUEST,
        ARP,
        ICMP,
        TCP,
        UDP,
        DNS,
        PING,
        PING_RESPONSE
    }
    
    // create an enum of request type
    public enum HTTPRequestType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    
    public class Packet
    {
        public string senderIpAddress;
        public string receiverIpAddress;
        public string message;
        public PacketType type;
        public string senderMacAddress;
        public string receiverMacAddress;
        private string content;
        public string[] item;


        private float minResponseTime;
        private float maxResponseTime;
        private float totalResponseTime;
        private int receivedResponses;
        private string url;
        private string methode = "";
        private string command = "";
        private string[] jsonData;


        public Packet(string senderIpAddress, string receiverIpAddress, string message ="", string senderMacAddress = "", string receiverMacAddress = "",string content = "",PacketType type = default)
        {
            this.senderIpAddress = senderIpAddress;
            this.receiverIpAddress = receiverIpAddress;
            this.message = message;
            this.senderMacAddress = senderMacAddress;
            this.receiverMacAddress = receiverMacAddress;
            this.type = type;
            this.content = content;
        }

        public string Content
        {
            get => content;
            set => content = value;
        }
        public string[] JsonData
        {
            get => jsonData;
            set => jsonData = value;
        }
        
        public float timestamp { get; set; }

        public float MinResponseTime
        {
            get => minResponseTime;
            set => minResponseTime = value;
        }

        public float MaxResponseTime
        {
            get => maxResponseTime;
            set => maxResponseTime = value;
        }

        public float TotalResponseTime
        {
            get => totalResponseTime;
            set => totalResponseTime = value;
        }

        public int ReceivedResponses
        {
            get => receivedResponses;
            set => receivedResponses = value;
        }
        public string Url
        {
            get => url;
            set => url = value;
        }
        public string Methode
        {
            get => methode;
            set => methode = value;
        }
    }

}