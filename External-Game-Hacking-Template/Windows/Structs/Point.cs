using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace External_Game_Hacking_Template.Windows.Structs
{
    /// <summary>
    ///     https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-point
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int X, Y;

        public override string ToString()
        {
            return $"{{X = {X},Y = {Y}}}";
        }
    }
}
