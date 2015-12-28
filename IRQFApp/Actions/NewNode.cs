using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfApplication2.User_Controls;

namespace WpfApplication2.Actions
{
    internal class NewNode : IQRFAction
    {
        NewNodeView newnode;

        public NewNode()
        {
            newnode = new NewNodeView();
            SetTimeInterval(NewNodeView.WaitTime);
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:00,00,00,04,FF,FF,00,00")));
        }

        public override void Start()
        {
            newnode.TimerStart();
            base.Start();
        }
        
        public override void AllDone()
        {

        }

        public override UIElement CreateNodeBondView(List<byte> data)
        {
            if (data.Count == 0)
                newnode.SetFail();
            else
                newnode.SetSuccess(data);
            DontADDElement = true;
            return null;
        }

        public override void SetView()
        {
            AddItemsToView(newnode);
        }
    }
}
