using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace External_Game_Hacking_Template.Windows.Structs
{
    /// <summary>
    ///     https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-hardwareinput
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HardwareInput
    {
        public int uMsg;
        public short wParamL;
        public short wParamH;
    }
}
