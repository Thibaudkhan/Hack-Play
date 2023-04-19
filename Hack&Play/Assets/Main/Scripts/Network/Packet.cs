namespace Main.Scripts.Network
{
    public class Packet
    {
        public string senderIpAddress;
        public string receiverIpAddress;
        public string message;
        public string type;
        public string senderMacAddress;
        public string receiverMacAddress;
        private float minResponseTime;
        private float maxResponseTime;
        private float totalResponseTime;
        private int receivedResponses;

        public Packet(string senderIpAddress, string receiverIpAddress, string message ="", string senderMacAddress = "", string receiverMacAddress = "",string type = "")
        {
            this.senderIpAddress = senderIpAddress;
            this.receiverIpAddress = receiverIpAddress;
            this.message = message;
            this.senderMacAddress = senderMacAddress;
            this.receiverMacAddress = receiverMacAddress;
            this.type = type;
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
    }

}