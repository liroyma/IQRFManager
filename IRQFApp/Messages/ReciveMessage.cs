using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApplication2
{
    public class ReciveMessage : Message
    {
        public byte[] onlyData;
        public bool SameSize { get; set; }

        public ReciveMessage(List<byte> data) :
            base(MessageType.Rx)
        {
            data.Remove(data.First());
            data.Remove(data.Last());
            string tempstring = GetStringFromByteArray(data.ToArray());
            int index = tempstring.IndexOf(":");
            if (index == -1)
            {
                Commant = tempstring;
                return;

            }
            MessageHeader = tempstring.Substring(0, index);
            byte tempdatalengrh=0;
            if (index > 2)
            {
                tempdatalengrh = data[index - 1];
                MessageHeader = tempstring.Substring(0, index-1);
            }
            SetType();
            data.RemoveRange(0, index + 1);
            onlyData = data.ToArray();

            Data = onlyData;
            if (onlyData.Length > 4)
            {
                PCMD = onlyData[3];
                PNUM = onlyData[2];
            }
            SetDataByHeader(data, tempstring.Substring(index + 1));
            SameSize = tempdatalengrh == DataLength;
        }

        private void SetDataByHeader(List<byte> data,string after)
        {
            switch (MessageHeader)
            {
                case "DS":
                    Commant = after;
                    break;
                case "DR":
                    DataLength = data.Count;
                    DataString = string.Join(",", data);
                    DataHex = PrintHex(data.ToArray());
                    DataDec = string.Join(" ", data);
                    break;
                case "S"://Get Status --> <S:[spi_status][CR]
                    Commant = string.Join(",", data);
                    break;
                case "B"://Connectivity Indication  --> <B:OK[CR]
                    Commant = after;
                    break;
                case "R"://Reset USB Device  --> <R:OK[CR]
                    Commant = after;
                    break;
                case "RT"://Reset TR Module  --> <RT:OK[CR]
                    Commant = after;
                    break;
                case "I":
                    string[] ss = after.Split('#');
                    Commant = string.Format("device type: {0}\nversion: {1}\nserial number: {2}",ss[0],ss[1],ss[2]);
                    break;
                case "IT"://Get TR Module Info  --> <IT:[module_info][CR]
                    DataString = string.Join(",", data);
                    DataHex = PrintHex(data.ToArray());
                    DataDec = string.Join(" ", data);
                    Commant = string.Format("Module ID: {7}\nOS version: {0}.{1:D2}{6}\nTR type: {2}\nOS build: \n{3}\n{4}\n{5:D4}", 
                        FromSubByte(data[4], 0, 4), 
                        FromSubByte(data[4], 4, 4),
                        GetTRseries(FromSubByte(data[5], 0, 4)),
                        GetFFC(FromSubByte(data[5], 4, 1)),
                        GetMCUtype(FromSubByte(data[5], 5, 3)),
                        string.Format("{1:x2}{0:x2}", data[6], data[7]),
                        addD?"D":"",
                        string.Format("{0:x2}{1:x2}{2:x2}{3:x2}", data[0], data[1], data[2], data[3]).ToUpper()
                        );
                    break;
            }
        }
        bool addD = false;
        static int FromSubByte(byte byt,int start,int size)
        {
            string x = Convert.ToString(byt, 2);
            for (int i = x.Length; i < 8; i++)
            {
                x = x.Insert(0, "0");
            }
            return Convert.ToInt32(x.Substring(start, size), 2);
        }

        private string GetTRseries(int number)
        {
            switch(number)
            {
                case 0:
                    return "TR-52D";
                case 1:
                    return "TR-58D-RJ";
                case 2:
                    return "TR-72D";
                case 3:
                    return "TR-53D";
                case 8:
                    return "TR-54D";
                case 9:
                    return "TR-55D";
                case 10:
                    return "TR-56D";
                case 11:
                    return "TR-76D";
            }
            return "";
        }

        private string GetMCUtype(int number)
        {
            switch (number)
            {
                case 3:
                    addD = false;
                    return "PIC16F886";
                case 4:
                    addD = true;
                    return "PIC16LF1938";
            }
            return "";
        }

        private string GetFFC(int number)
        {
            switch (number)
            {
                case 0:
                    return "FCC not certified";
                case 1:
                    return "FCC certified";
            }
            return "";
        }

        private void SetType()
        {
            if (MessageHeader == "DR")
            {
                MessageType = MessageType.Rxa;
                MessageColor = Brushes.Purple;
                MessageDPA = "Asynchronous";
            }
            else
            {
                MessageType = MessageType.Rx;
                MessageColor = Brushes.Blue;
                MessageDPA = "Response";
            }
        }

    }

    public enum MessageType
    {
        Tx,
        Rx,
        Rxa
    }
}
