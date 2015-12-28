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
using WpfApplication2.Thumb;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for myThumb.xaml
    /// </summary>
    public partial class myThumb : UserControl
    {
        public List<LineGeometry> EndLines { get; private set; }
        public List<LineGeometry> StartLines { get; private set; }
        public int PerentID { get; private set; }
        public int ZoneID { get; private set; }
        MyThumbModel Model;

        public int id { get; set; }
        public string nr { get; set; }
        public ThumbTypes type { get; set; }

        public myThumb(int id,string nr, int zone, ThumbTypes type)
        {
            InitializeComponent();
            this.id = id;
            this.nr = nr;
            this.type = type;
            this.ZoneID = zone;
            StartLines = new List<LineGeometry>();
            EndLines = new List<LineGeometry>();
            PerentID = -1;
        }

        public myThumb(int id, string nr, int zone, int perenid, ThumbTypes type)
        {
            InitializeComponent();
            this.id = id;
            this.nr = nr;
            this.type = type;
            this.ZoneID = zone;
            PerentID = perenid;
            StartLines = new List<LineGeometry>();
            EndLines = new List<LineGeometry>();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Model = new MyThumbModel(id, nr, type);
            this.DataContext = Model;
        }

        public override string ToString()
        {
            return "id: " + id + ",  nr: " + nr + ", PerentID: " + PerentID + ", ZoneID: " + ZoneID;
        }
    }

    public enum ThumbTypes
    {
        Node,
        Cordinatore,
        None
    }
}
