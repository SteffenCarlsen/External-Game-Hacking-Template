using System;
using System.Collections.Generic;
using System.Text;

namespace External_Game_Hacking_Template.Windows.Delegates
{
    /// <summary>
    ///     https://docs.microsoft.com/en-us/windows/win32/api/winuser/nc-winuser-hookproc
    /// </summary>
    public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
}
