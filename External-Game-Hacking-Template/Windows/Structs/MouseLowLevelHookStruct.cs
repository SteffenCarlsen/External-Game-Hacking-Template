using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace External_Game_Hacking_Template.Windows.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MouseLowLevelHookStruct
    {
        public Point pt;
        public int mouseData;
        public int flags;
        public int time;
        public IntPtr dwExtraInfo;
    }

}
