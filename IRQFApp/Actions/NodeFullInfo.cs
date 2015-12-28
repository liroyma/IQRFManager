using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfApplication2.User_Controls;

namespace WpfApplication2.Actions
{
    class NodeFullInfo : IQRFAction
    {
        public NodeFullInfo(int nodenumber)
        {
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,02,00,FF,FF", (nodenumber).ToString("x2"))));//HandleRTInfo
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,02,02,FF,FF", (nodenumber).ToString("x2"))));//HandelRTConfigutation
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,01,00,FF,FF", (nodenumber).ToString("x2"))));//HandleNTWInfo
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
