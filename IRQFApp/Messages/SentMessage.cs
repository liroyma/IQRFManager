using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApplication2
{
    public class SentMessage : Message
    {
        public string FullString { get; set; }
        public byte[] FullData { get; set; }
        public bool HaveData { get; set; }


        public SentMessage(string header)
            : base(MessageType.Tx)
        {
            MessageHeader = header;
            HaveData = false;
            DataLength = 0;
            FullString = string.Format(">{0}\r", header);
            FullData = null;
            Data = null;
        }

        public SentMessage(string header, string data)
            : base(MessageType.Tx)
        {
            MessageHeader = header;
            HaveData = true;
            Data = GetByteFromString1(data);
            PCMD = Data[3];
            PNUM = Data[2];
            DataLength = Data.Length;
            DataString = data.ToUpper();
            DataDec = string.Join(" ", Data);
            DataHex = PrintHex(Data);
            FullData = ConvertMessageToByteArray();
            FullString = null;
        }

        public byte[] ConvertMessageToByteArray()
        {
            List<byte> temp = new List<byte>();
            temp.AddRange(GetByteFromString(">" + MessageHeader));
            temp.Add((byte)DataLength);
            temp.AddRange(GetByteFromString(":"));
            temp.AddRange(Data);
            temp.AddRange(GetByteFromString("\r"));
            return temp.ToArray();
        }

        public byte[] GetByteFromString1(string str)
        {
            List<byte> d = new List<byte>();
            foreach (var item in str.Split(','))
            {
                d.Add((byte)Convert.ToInt32(item, 16));
            }
            return d.ToArray();
        }

        public static SentMessage CreateMessage(string str)
        {
            string[] split = str.Split(':');
            return split.Length > 1 ? new SentMessage(split[0].ToUpper(), split[1]) : new SentMessage(split[0].ToUpper());
        }
    }


    public enum GroupType
    {
        Ganeral = 1,
        Coordinator = 2,
        Node = 3,
        None = 99
    }
}
