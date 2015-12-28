using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication2.Thumb
{
    class MyThumbModel: System.Windows.Controls.Primitives.Thumb, INotifyPropertyChanged
    {

        private LinearGradientBrush nodecolor;
        public LinearGradientBrush NodeColor
        {
            get { return nodecolor; }
            set
            {
                nodecolor = value;
                NotifyPropertyChanged("NodeColor");
            }
        }

        private int nodeid;
        public int NodeID
        {
            get { return nodeid; }
            set
            {
                nodeid = value;
                NotifyPropertyChanged("NodeID");
            }
        }

        private string nodenr;
        public string NodeNr
        {
            get { return nodenr; }
            set
            {
                nodenr = value;
                NotifyPropertyChanged("NodeNr");
            }
        }
        

        public MyThumbModel(int id, string nr, ThumbTypes type) : base()
        {
            NodeColor = GetByType(type);
            NodeID = id;
            NodeNr = nr;
        }

        private LinearGradientBrush GetByType(ThumbTypes type)
        {
            GradientStopCollection coll = new GradientStopCollection();
            switch (type)
            {
                case ThumbTypes.Cordinatore:
                    coll.Add(new GradientStop(Color.FromRgb(68, 215, 68), 0.0));
                    coll.Add(new GradientStop(Color.FromRgb(12, 39, 12), 1.0));
                    break;
                case ThumbTypes.Node:
                    coll.Add(new GradientStop(Color.FromRgb(0, 169, 215), 0.0));
                    coll.Add(new GradientStop(Color.FromRgb(0, 29, 36), 1.0));
                    break;
                case ThumbTypes.None:
                    coll.Add(new GradientStop(Color.FromRgb(3, 3, 3), 0.0));
                    coll.Add(new GradientStop(Color.FromRgb(200, 182, 76), 1.0));
                    break;
                default:
                    return null;
            }
            return new LinearGradientBrush(coll); ;
        }

        public override string ToString()
        {
            return Name;
        }

        #region INotifyPropertyChanged Handler
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
