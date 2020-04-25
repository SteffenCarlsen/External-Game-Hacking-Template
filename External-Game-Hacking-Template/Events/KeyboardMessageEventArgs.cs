using External_Game_Hacking_Template.Windows.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace External_Game_Hacking_Template.Events
{
    public class KeyboardMessageEventArgs : EventArgs
    {
        public int VirtKeyCode { get; private set; }
        public KeyboardMessage MessageType { get; private set; }

        public KeyboardMessageEventArgs(int vkCode, KeyboardMessage msg)
        {
            VirtKeyCode = vkCode;
            MessageType = msg;
        }
    }
}
