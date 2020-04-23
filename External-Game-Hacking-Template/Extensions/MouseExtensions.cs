using System.Runtime.InteropServices;

namespace External_Game_Hacking_Template.Extensions
{
    internal class MouseExtensions
    {
        public static void MouseLeftDown()
        {
            var mouseMoveInput = new Windows.Structs.Input { type = Windows.Enums.SendInputEventType.InputMouse, mi = { dwFlags = Windows.Enums.MouseEventFlags.MOUSEEVENTF_LEFTDOWN } };
            Windows.User32.SendInput(1, ref mouseMoveInput, Marshal.SizeOf<Windows.Structs.Input>());
        }
        public static void MouseMove(int deltaX, int deltaY)
        {
            var mouseMoveInput = new Windows.Structs.Input { type = Windows.Enums.SendInputEventType.InputMouse, mi = { dwFlags = Windows.Enums.MouseEventFlags.MOUSEEVENTF_MOVE, dx = deltaX, dy = deltaY } };
            Windows.User32.SendInput(1, ref mouseMoveInput, Marshal.SizeOf<Windows.Structs.Input>());
        }

        public static void MoveMouseRelative(int deltaX, int deltaY)
        {
            var mouseMoveInput = new Windows.Structs.Input { type = Windows.Enums.SendInputEventType.InputMouse, mi = { dwFlags = Windows.Enums.MouseEventFlags.MOUSEEVENTF_MOVE, dx = deltaX, dy = deltaY } };
            Windows.User32.SendInput(1, ref mouseMoveInput, Marshal.SizeOf<Windows.Structs.Input>());
        }
    }
}
