using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace External_Game_Hacking_Template.Windows.Structs
{
    /// <summary>
    ///     https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-taginput
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Input
    {
        [FieldOffset(0)] public Enums.SendInputEventType type;
        [FieldOffset(4)] public MouseInput mi;
        [FieldOffset(4)] public KeyboardInput ki;
        [FieldOffset(4)] public HardwareInput hi;
    }

}
