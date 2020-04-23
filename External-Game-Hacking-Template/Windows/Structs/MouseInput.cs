using System;
using System.Runtime.InteropServices;

namespace External_Game_Hacking_Template.Windows.Structs
{
    /// <summary>
    ///     https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-mouseinput
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseInput
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public Enums.MouseEventFlags dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }
}