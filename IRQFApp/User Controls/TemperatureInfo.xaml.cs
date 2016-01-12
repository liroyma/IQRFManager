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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TemperatureInfo : UserControl
    {

        public byte tmpFah { get; private set; }

        public byte tmpc;
        public byte tmpCel
        {
            get { return tmpc; }
            private set
            {
                tmpc = value;
                tmpFah = (byte)(((value * 9) / 5) + 32);
            }
        }

        public TemperatureInfo(List<byte> data)
        {
            InitializeComponent();
            this.DataContext = this;
            tmpCel = data[0];
        }
    }
}
