using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WpfApplication2
{
    public class Message
    {
        public DateTime Time { get; set; }
        public MessageType MessageType { get; set; }
        public int DataLength { get; set; }
        public string MessageDPA { get; set; }
        public string MessageHeader { get; set; }
        public Brush MessageColor { get; set; }
        public string Commant { get; set; }


        public string DataString { get; set; }
        public string DataDec { get; set; }
        public string DataHex { get; set; }
        public byte[] Data { get; set; }
        public byte PCMD { get; set; }
        public byte PNUM { get; set; }

        public Message(MessageType type)
        {
            Time = DateTime.Now;
            MessageType = type;
            SetMessageColorAndDPA(type);
        }

        private void SetMessageColorAndDPA(WpfApplication2.MessageType type)
        {
            switch (type)
            {
                case WpfApplication2.MessageType.Tx:
                    MessageColor = Brushes.Green;
                    MessageDPA = "Request";
                    return;
                case WpfApplication2.MessageType.Rx:
                    MessageColor = Brushes.Blue;
                    MessageDPA = "Request";
                    return;
                case WpfApplication2.MessageType.Rxa:
                    MessageColor = Brushes.Purple;
                    MessageDPA = "Request";
                    return;
            }
        }

        public byte[] GetFullMessage(string str)
        {
            List<byte> aa = new List<byte>();
            string[] res = str.Split(':');
            aa.AddRange(GetByteFromString(string.Format("{0}", res[0]).ToUpper()));
            if (res.Length > 1)
            {
                if (OnlyHexInString(res[1]))
                {
                    string[] moredata = res[1].Split(',');
                    aa.Add((byte)moredata.Length);
                    aa.AddRange(GetByteFromString(":"));
                    foreach (var item in moredata)
                    {
                        aa.Add(Convert.ToByte(item, 16));
                    }
                }
                else
                {
                    aa.Add((byte)res[1].Length);
                    aa.AddRange(GetByteFromString(":"));
                    aa.AddRange(GetByteFromString(res[1]));
                }
            }
            aa.AddRange(GetByteFromString("\r"));
            return aa.ToArray();
        }

        public byte[] GetByteFromString(string str)
        {

            List<byte> d = new List<byte>();
            foreach (var item in str)
            {
                d.Add(BitConverter.GetBytes(item).First());
            }
            return d.ToArray();
        }

        public bool OnlyHexInString(string test)
        {
            test = test.Replace(",", "");
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        public string PrintHex(byte[] data)
        {
            string str = "";
            for (int i = 0; i < data.Length; i++)
            {
                str += string.Format("{0:x2} ", data[i]).ToUpper();
            }
            return str;
        }

        public string GetStringFromByteArray(byte[] data)
        {
            string str = "";
            foreach (var item in data)
            {
                str += Convert.ToChar(item);
            }
            return str;
        }
    }
}
