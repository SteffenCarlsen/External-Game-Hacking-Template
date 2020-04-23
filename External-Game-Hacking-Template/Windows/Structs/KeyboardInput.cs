using System;
using System.Runtime.InteropServices;

namespace External_Game_Hacking_Template.Windows.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardInput
    {
        /// <summary>
        ///     https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-keybdinput
        /// </summary>
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }
}