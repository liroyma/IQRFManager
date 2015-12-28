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
    /// Interaction logic for TXInfo.xaml
    /// </summary>
    public partial class TXInfo : UserControl
    {
        public string ModuleType { get; set; }
        public string FCCID { get; set; }
        public string MCUtype { get; set; }
        public string ModuleID { get; set; }
        public string OSversion { get; set; }


        public string Rssi { get; set; }
        public string SupplyVoltage { get; set; }
        public bool insufficientOsBuild { get; set; }
        public bool CustomDPAHandler { get; set; }
        public bool SPI { get; set; }
        public bool UART { get; set; }

        bool addD = false;

        public TXInfo(List<byte> data)
        {
            InitializeComponent();
            this.DataContext = this;
            ModuleID = string.Format("{3:x2}{2:x2}{1:x2}{0:x2}", data[0], data[1], data[2], data[3]).ToUpper();
            OSversion = string.Format("{0}.{1:D2}{2} ({3})", FromSubByte(data[4], 0, 4), FromSubByte(data[4], 4, 4), addD ? "D" : "", string.Format("{1:x2}{0:x2}", data[6], data[7]));
            ModuleType = string.Format("DC{0}x", GetTRseries(FromSubByte(data[5], 0, 4)));
            FCCID = GetFFC(FromSubByte(data[5], 4, 1));
            MCUtype = GetMCUtype(FromSubByte(data[5], 5, 3));

            Rssi = data[8].ToString();
            SupplyVoltage = data[9].ToString();
            string flags = ByteToEigthBits(data[10]);
            insufficientOsBuild = flags[0] == 1;
            UART = flags[1] == 1;
            SPI = !UART;
            CustomDPAHandler = flags[2] == 1;
        }

        int FromSubByte(byte byt, int start, int size)
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
            switch (number)
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
                    return "PIC 16F886";
                case 4:
                    addD = true;
                    return "PIC 16LF1938";
            }
            return "";
        }

        private string GetFFC(int number)
        {
            switch (number)
            {
                case 0:
                    //return "FCC not certified";
                    return "-";
                case 1:
                    return "FCC certified";
            }
            return "";
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

    [ValueConversion(typeof(bool), typeof(bool))]
    public class YesNoBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var boolValue = value is bool && (bool)value;

            return boolValue ? "Yes" : "No";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null && value.ToString() == "Yes";
        }
    }
}
