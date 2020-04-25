using External_Game_Hacking_Template.Windows.Enums;
using External_Game_Hacking_Template.Windows.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace External_Game_Hacking_Template.Events
{
   public class MouseMessageEventArgs : EventArgs
    {
        public Point Position { get; private set; }
        public MouseMessage MessageType { get; private set; }

        public MouseMessageEventArgs(Point position, MouseMessage msg)
        {
            Position = position;
            MessageType = msg;
        }
    }
}
