using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApplication2.Actions
{
    internal class RemoveNode : IQRFAction
    {
        TextBlock text1 = new TextBlock();
        TextBlock text2 = new TextBlock();

        int selectedNode;

        public RemoveNode(int selectednode)
        {
            selectedNode = selectednode;
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,01,01,FF,FF", (selectednode).ToString("x2"))));
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:00,00,00,05,FF,FF,{0}", (selectednode).ToString("x2"))));
        }

        public override void AllDone()
        {
        }

        public override void SetView()
        {
            StackPanel stack = new StackPanel();
           

            text1.Margin = new System.Windows.Thickness(15, 0, 15, 0);
            text1.Padding = new System.Windows.Thickness(15);
            text1.FontSize = 20;
            text1.Text = string.Format("Removeing node {0} bond." ,selectedNode);
            text2.Margin = new System.Windows.Thickness(15, 0, 15, 0);
            text2.Padding = new System.Windows.Thickness(15);
            text2.FontSize = 20;
            text2.Text = string.Format("Removeing node {0} bond from cordinator.", selectedNode);


            stack.Children.Add(text1);
            stack.Children.Add(text2);


            text1.Text.Insert(0, " V");
            text2.Text.Insert(0, " X");

            AddItemsToView(stack);
        }
    }
}
