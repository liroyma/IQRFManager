using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public class MyNode
    {
        public bool IsBond;
        public bool IsDiscoverd;
        public int NodeIndex;
        public int NodeID;
        public int NodePerent;
        public int NodeVRN;
        public int NodeZone = 99;
        public SentMessage message;

        public MyNode(int index)
        {
            NodeIndex = index;
            NodeID = index+1;
            message = SentMessage.CreateMessage(string.Format("DS:{0},00,01,00,FF,FF", (NodeID).ToString("x2")));
        }

        public string SetInfo(byte[] data)
        {
            NodeVRN = data[1];
            NodeZone = data[2];
            NodePerent = data[4];
            return "ID " + NodeID + ", VRN: " + NodeVRN + ", Zone: " + NodeZone + ", Perent: " + NodePerent;
        }

    }
    
}
