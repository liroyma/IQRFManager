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
    public partial class RedLedView : UserControl
    {
        private GradientStopCollection red_col;
        private GradientStopCollection white_col;
        private LinearGradientBrush Red;
        private LinearGradientBrush White;

        public LinearGradientBrush LedColor { get; private set; }
        public string tmpstate;

        public string tmpState
        {
            get { return tmpstate; }
            private set
            {
                tmpstate = value;
                LedColor = (tmpstate.Equals("ON") ? Red : White);
            }
        }

        public RedLedView(List<byte> data, byte pcmd)
        {
            InitializeComponent();
            this.DataContext = this;

            red_col = new GradientStopCollection();
            red_col.Add(new GradientStop(Color.FromRgb(245, 17, 17), 0.0));
            red_col.Add(new GradientStop(Color.FromRgb(189, 49, 49), 1.0));
            Red = new LinearGradientBrush(red_col);

            white_col = new GradientStopCollection();
            white_col.Add(new GradientStop(Color.FromRgb(255, 255, 255), 0.0));
            white_col.Add(new GradientStop(Color.FromRgb(228, 228, 228), 1.0));
            White = new LinearGradientBrush(white_col);

            switch (pcmd)
            {
                case 0:
                    tmpState = "OFF";
                    break;
                case 1:
                    tmpState = "ON";
                    break;
                case 2:
                    tmpState = (data[0] > 0 ? "ON" : "OFF");
                    break;
            }
        }
    }
}
