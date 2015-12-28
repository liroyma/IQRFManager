using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApplication2.Actions
{
    class USBInfo : IQRFAction
    {
        public USBInfo(int nodenmber)
        {
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("I")));
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("IT")));
        }
        
        public override void AllDone()
        {
        }

        public override void GetUSBAnswer(ReciveMessage message)
        {
            DontWaitForMore = true;
            base.GetUSBAnswer(message);
        }

        public override void SetView()
        {
        }
    }
}
