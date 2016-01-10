using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Connection ConnectionModel;

        public MainWindow()
        {
            ConnectionModel = new Connection();
            InitializeComponent();
            this.DataContext = ConnectionModel;
            IQRFAction.SetPanel(Container);
            //ConnectionModel.SetStackPanel(Container);
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            foreach (var item in StackPanel1.Children)
            {
                if(item is Expander)
                {
                    if((Expander)item != (Expander)sender)
                    {
                        ((Expander)item).IsExpanded = false;
                    }
                }
            }
        }
    }

    public class BooleanAndConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            foreach (object value in values)
            {
                if ((value is bool) && (bool)value == false)
                {
                    return false;
                }
            }
            return true;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("BooleanAndConverter is a OneWay converter.");
        }
    }
}
