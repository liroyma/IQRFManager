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
    /// Interaction logic for NetworkInfo.xaml
    /// </summary>
    public partial class NetworkInfo : UserControl
    {
        public byte ntwADDR { get; set; }
        public byte ntwVRN { get; private set; }
        public byte ntwZIN { get; private set; }
        public byte ntwDID { get; private set; }
        public byte ntwPVRN { get; private set; }
        public int ntwUSERADDRESS { get; private set; }
        public int ntwID { get; private set; }
        public byte ntwVRNFNZ { get; private set; }
        public byte ntwCFG { get; private set; }
        public bool isBonded { get; private set; }

        public NetworkInfo(List<byte> data)
        {
            InitializeComponent();
            this.DataContext = this;
            ntwADDR = data[0];
            ntwVRN = data[1];
            ntwZIN = data[2];
            ntwDID = data[3];
            ntwPVRN = data[4];
            ntwUSERADDRESS = int.Parse(BitConverter.ToString(new byte[] { data[5], data[6] }).Replace("-", ""), System.Globalization.NumberStyles.HexNumber);
            ntwID = int.Parse(BitConverter.ToString(new byte[] { data[7], data[8] }).Replace("-", ""), System.Globalization.NumberStyles.HexNumber);
            ntwVRNFNZ = data[9];
            ntwCFG = data[10];
            string flags = ByteToEigthBits(data[11]);
            isBonded = flags[0] == '1';
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


    }
}
