using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApplication2.Helpers;

namespace WpfApplication2.Actions
{
    internal class Discovery : IQRFAction
    {
        List<MyNode> Nodes;
        List<myThumb> thumbs;
        int _TotalDicovery = 0;
        int TotalDicovery
        {
            get { return _TotalDicovery; }
            set
            {
                _TotalDicovery = value;
                text1.Text = "Discovered Nodes: " + _TotalDicovery;
            }
        }
        int _TotalNodes = 0;
        int TotalNodes
        {
            get { return _TotalNodes; }
            set
            {
                _TotalNodes = value;
                text2.Text = "Bonded Nodes: " + _TotalNodes;
            }
        }

        TextBlock text1 = new TextBlock();
        TextBlock text2 = new TextBlock();
        Canvas myCanvas = new Canvas();

        public Discovery()
        {
            myThumb myThumb1 = new myThumb(0, "C", 0, ThumbTypes.Cordinatore);
            myCanvas.Children.Add(myThumb1);
            thumbs = new List<myThumb>() { myThumb1 };
            ReSet();
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:00,00,00,00,FF,FF")));
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:00,00,00,07,FF,FF,07,00")));
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:00,00,00,02,FF,FF")));
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:00,00,00,01,FF,FF")));
        }
        

        void ReSet()
        {
            Nodes = new List<MyNode>();
            for (int i = 0; i < 239; i++)
            {
                Nodes.Add(new MyNode(i));
            }
        }

        public override void SetView()
        {
            DockPanel dock = new DockPanel();
            dock.LastChildFill = true;
            dock.Background = System.Windows.Media.Brushes.LightGray;
            WrapPanel wrapp = new WrapPanel();
            wrapp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            wrapp.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            DockPanel.SetDock(wrapp, Dock.Top);

            text1.Margin = new System.Windows.Thickness(15, 0, 15, 0);
            text1.Padding = new System.Windows.Thickness(15);
            text1.FontSize = 20;

            text2.Margin = new System.Windows.Thickness(15, 0, 15, 0);
            text2.Padding = new System.Windows.Thickness(15);
            text2.FontSize = 20;
            wrapp.Children.Add(text1);
            wrapp.Children.Add(text2);

            ScrollViewer scroll = new ScrollViewer();
            DockPanel.SetDock(scroll, Dock.Top);
            scroll.Background = System.Windows.Media.Brushes.White;
            scroll.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

            scroll.Content = myCanvas;

            dock.Children.Add(wrapp);
            dock.Children.Add(scroll);

            AddItemsToView(dock);
        }

        public override void AllDone()
        {
            foreach (var item in Nodes.FindAll(z => z.IsBond && !z.IsDiscoverd))
            {
                AddNode(item, ThumbTypes.None);
            }
        }

        public override UIElement CreateNumberOfNodesView(List<byte> data)
        {
            TotalNodes = data[0];
            DontADDElement = true;
            return null;
        }

        public override UIElement CreateDiscoversNodesView(List<byte> data)
        {
            DontADDElement = true;
            string x = string.Empty;
            foreach (var item in data)
            {
                x += Utility.ByteToEigthBits(item, false);
            }
            x = x.Remove(0, 1);
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].IsDiscoverd = x[i] != '0';
            }
            foreach (var item in Nodes.FindAll(z => z.IsBond && z.IsDiscoverd))
            {
                ActionMessages.Add(item.message);
            }
            return null;
        }

        public override UIElement CreateNumberOfBondedsView(List<byte> data)
        {
            DontADDElement = true;
            string x = string.Empty;
            foreach (var item in data)
            {
                x += Utility.ByteToEigthBits(item, false);
            }
            x = x.Remove(0, 1);
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].IsBond = x[i] != '0';
            }
            return null;
        }

        public override UIElement CreateDiscovertBondedsView(List<byte> data)
        {
            TotalDicovery = data[0];
            DontADDElement = true;
            return null;
        }

        public override UIElement CreateNTWView(List<byte> data)
        {
            DontADDElement = true;
            MyNode _node = Nodes.Find(x => x.NodeID == data[0]);
            _node.SetInfo(data.ToArray());
            AddNode(_node, ThumbTypes.Node);
            DontADDElement = true;
            return null;
        }

        private void UpdateLines(myThumb thumb)
        {
            double left = Canvas.GetLeft(thumb);
            double top = Canvas.GetTop(thumb);

            double thumbWidth = 140;
            double thumbHeight = 140;

            for (int i = 0; i < thumb.StartLines.Count; i++)
                thumb.StartLines[i].StartPoint = new System.Windows.Point(left + thumbWidth / 2, top + thumbHeight / 2);

            for (int i = 0; i < thumb.EndLines.Count; i++)
                thumb.EndLines[i].EndPoint = new System.Windows.Point(left + thumbWidth / 2, top + thumbHeight / 2);
        }

        private void ConnectThumbs(myThumb Thumb1)
        {
            if (Thumb1.type == ThumbTypes.Node)
            {
                myThumb perent = thumbs.Where(x => x.id == Thumb1.PerentID).First();
                System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
                path.Stroke = System.Windows.Media.Brushes.Black;
                path.StrokeThickness = 2;
                System.Windows.Media.LineGeometry line = new System.Windows.Media.LineGeometry();
                path.Data = line;
                myCanvas.Children.Add(path);
                perent.StartLines.Add(line);
                Thumb1.EndLines.Add(line);
            }
        }

        private bool CounerEqelToThimstCount(int count, int[] thumbsLineCount)
        {
            int sum = 0;
            foreach (var item in thumbsLineCount)
            {
                sum += item;
            }
            return sum == count;
        }

        public void AddNode(MyNode node, ThumbTypes type)
        {
            myThumb Thumb;
            if (type == ThumbTypes.None)
                Thumb = new myThumb(node.NodeID, "", node.NodeZone, node.NodePerent, type);
            else
                Thumb = new myThumb(node.NodeID, node.NodeVRN.ToString(), node.NodeZone, node.NodePerent, type);
            myCanvas.Children.Add(Thumb);
            thumbs.Add(Thumb);
            UpdateView();
        }

        private void UpdateView()
        {
            double lineWidth = myCanvas.ActualWidth;
            double ThumbWidth = thumbs[0].ActualWidth;
            double ThumbHeight = thumbs[0].ActualHeight;

            var gouped = thumbs.GroupBy(x => x.ZoneID);
            int row = 0;
            foreach (var item in gouped)
            {
                int now = 0;
                foreach (var thumb in item.ToList())
                {
                    Canvas.SetZIndex(thumb, 1);
                    Canvas.SetTop(thumb, row * ThumbHeight);
                    Canvas.SetLeft(thumb, (lineWidth / (item.Count() + 1) * (now++ + 1)) - (ThumbWidth / 2));
                }
                row++;
            }

            foreach (var item in thumbs)
            {
                ConnectThumbs(item);
            }

            foreach (var item in thumbs)
            {
                UpdateLines(item);
            }


            myCanvas.Height = gouped.Count() * ThumbHeight;

            //form.Height = myCanvas.Height+150;
            myCanvas.Width = lineWidth;

        }
    }
}
