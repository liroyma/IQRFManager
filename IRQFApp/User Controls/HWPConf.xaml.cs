using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication2.User_Controls
{
    /// <summary>
    /// Interaction logic for HWPConf.xaml
    /// </summary>
    public partial class HWPConf : UserControl
    {
        public bool CustomDPA { get; set; }
        public bool Interface { get; set; }
        public bool Autoexec { get; set; }
        public bool Routing { get; set; }
        public bool IOSetup { get; set; }
        public bool PeerToPeer { get; set; }

        public int MainRFChannelA { get; set; }
        public int MainRFChannelB { get; set; }
        public int RFoutputpower { get; set; }
        public int RFsignalfilter { get; set; }
        public int Timeout { get; set; }
        public int Baudrate { get; set; }
        public int nonzero { get; set; }
        public int MainRFA { get; set; }
        public int MainRFB { get; set; }

        public HWPConf(List<byte> data)
        {
            InitializeComponent();
            this.DataContext = this;

            data = XORLIstWithByte(data, 52);
            string bit5 = ByteToEigthBits(data[5]);
            CustomDPA = bit5[0] == '1';
            Interface = bit5[1] == '1';
            Autoexec = bit5[2] == '1';
            Routing = bit5[3] != '1';
            IOSetup = bit5[4] == '1';
            PeerToPeer = bit5[5] == '1';

            MainRFChannelA = data[6];
            MainRFChannelB = data[7];
            RFoutputpower = data[8];
            RFsignalfilter = data[9];
            Timeout = data[10];
            Baudrate = GetUARTBOUDRate(data[11]);
            nonzero = data[12];
            MainRFA = data[17];
            MainRFB = data[18];
            Console.WriteLine(int.Parse(BitConverter.ToString(new byte[] { data[30], data[31] }).Replace("-", ""), System.Globalization.NumberStyles.HexNumber));
            Console.WriteLine(int.Parse(BitConverter.ToString(new byte[] { data[32], data[33] }).Replace("-", ""), System.Globalization.NumberStyles.HexNumber));
            Console.WriteLine(int.Parse(BitConverter.ToString(new byte[] { data[30], data[31], data[32], data[33] }).Replace("-", ""), System.Globalization.NumberStyles.HexNumber));
        }

        public int byteArrayToInt(params byte[] b)
        {
            if (b.Length == 4)
                return b[0] << 24 | (b[1] & 0xff) << 16 | (b[2] & 0xff) << 8
                    | (b[3] & 0xff);
            else if (b.Length == 2)
                return 0x00 << 24 | 0x00 << 16 | (b[0] & 0xff) << 8 | (b[1] & 0xff);

            return 0;
        }

        private List<byte> XORLIstWithByte(List<byte> list, byte b)
        {
            List<byte> newlist = new List<byte>();
            foreach (var item in list)
            {
                newlist.Add((byte)(item ^ b));
            }
            return newlist;
        }

        private string ByteToEigthBits(byte item)
        {
            char[] arr = Convert.ToString(item, 2).ToCharArray();

            Array.Reverse(arr);
            string x = new string(arr);

            for (int i = x.Length; i < 8; i++)
            {
                x += "0";
            }


            return x;
        }

        private int GetUARTBOUDRate(int number)
        {
            switch(number)
            {
                case 0:
                    return 1200;
                case 1:
                    return 2400;
                case 2:
                    return 4800;
                case 3:
                    return 9600;
                case 4:
                    return 19200;
                case 5:
                    return 38400;
                case 6:
                    return 57600;
                case 7:
                    return 115200;
                default:
                    return 0;
            }
        }
    }
}
