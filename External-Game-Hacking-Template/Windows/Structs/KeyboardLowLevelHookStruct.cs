using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace External_Game_Hacking_Template.Windows.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardLowLevelHookStruct
    {
        public int vkCode;
        public int scanCode;
        public int flags;
        public int time;
        public IntPtr dwExtraInfo;
    }
}
