using System;
using System.Windows.Forms;
using External_Game_Hacking_Template.Windows;

namespace External_Game_Hacking_Template.Extensions
{
    internal class KeyboardExtensions
    {
        public static bool KeyDown(Keys key)
        {
            return Convert.ToBoolean(User32.GetAsyncKeyState(key) & 0x8000);
        }

        public static bool KeyDown(int key)
        {
            return Convert.ToBoolean(User32.GetAsyncKeyState(key) & 0x8000);
        }

        public static bool KeyPressed(Keys key)
        {
            return Convert.ToBoolean(User32.GetAsyncKeyState(key) & 1);
        }

        public static bool KeyPressed(int key)
        {
            return Convert.ToBoolean(User32.GetAsyncKeyState(key) & 1);
        }
    }
}
