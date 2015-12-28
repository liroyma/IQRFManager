using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using WpfApplication2.Actions;

namespace WpfApplication2
{
    internal class Network : IQRFAction
    {
        string StringMessage;

        public Network(int selectednode)
        {
            StringMessage = string.Format("Node {0} Netwotk: ", selectednode);
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,01,00,FF,FF", (selectednode).ToString("x2"))));
        }

        public override void Start()
        {
            base.Start();
        }

        public override void AllDone()
        {
        }

        public override void SetView()
        {
        }
    }
}