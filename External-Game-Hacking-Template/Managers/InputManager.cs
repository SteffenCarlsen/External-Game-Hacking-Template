using External_Game_Hacking_Template.Events;
using External_Game_Hacking_Template.Features;
using External_Game_Hacking_Template.Windows;
using External_Game_Hacking_Template.Windows.Delegates;
using External_Game_Hacking_Template.Windows.Enums;
using External_Game_Hacking_Template.Windows.Structs;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace External_Game_Hacking_Template.Managers
{
    public class InputManager
    {
        /// <summary>
        /// Event to process mouse messages from the hook
        /// </summary>
        public event EventHandler<MouseMessageEventArgs> MouseMessageEvent;
        /// <summary>
        /// Event to process keyboard messages from the hook
        /// </summary>
        public event EventHandler<KeyboardMessageEventArgs> KeyboardMessageEvent;

        /// <summary>
        /// If specified to true, will not call CallNextHookEx
        /// </summary>
        public bool StealthyMouseHook { get; private set; } = false;

        /// <summary>
        /// If specified to true, will not call CallNextHookEx
        /// </summary>
        public bool StealthyKeyboardHook { get; private set; } = false;

        private GlobalHook KeyboardHook = null;
        private GlobalHook MouseHook = null;

        public InputManager()
        {
            MessagePump pump = new MessagePump();
            pump.Start();
        }

        /// <summary>
        /// Initializes a mousehook
        /// Remember to specify an <MouseMessageEvent cref="MouseMessageEvent"/>
        /// </summary>
        /// <param name="stealthyHook"></param>
        public void InitMouseHook(bool stealthyHook = false)
        {
            StealthyMouseHook = stealthyHook;
            if (MouseHook != null)
            {
                MouseHook.Dispose();
            }

            MouseHook = new GlobalHook(HookType.WH_MOUSE_LL, MouseHookCallback);
        }

        public IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var mouseStructure = Marshal.PtrToStructure<MouseLowLevelHookStruct>(lParam);
                MouseMessageEvent?.Invoke(this, new MouseMessageEventArgs(mouseStructure.pt, (MouseMessage)wParam));
            }
            if (!StealthyMouseHook)
            {
                return User32.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Initializes a keyboard hook
        /// Remember to specify an event <KeyboardMessageEvent cref="KeyboardMessageEvent"/>
        /// </summary>
        /// <param name="stealthyHook"></param>
        public void InitKeyboardHook(bool stealthyHook = false)
        {
            StealthyKeyboardHook = stealthyHook;
            if (KeyboardHook != null)
            {
                KeyboardHook.Dispose();
            }
            KeyboardHook = new GlobalHook(HookType.WH_KEYBOARD_LL, KeyboardHookCallback);
        }

        private IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var keyboardStructure = Marshal.PtrToStructure<KeyboardLowLevelHookStruct>(lParam);
                KeyboardMessageEvent?.Invoke(this, new KeyboardMessageEventArgs(keyboardStructure.vkCode, (KeyboardMessage)wParam));
            }
            if (!StealthyKeyboardHook)
            {
                return User32.CallNextHookEx(KeyboardHook.HookHandle, nCode, wParam, lParam);

            }
            return IntPtr.Zero;
        }


        public void Dispose()
        {
            KeyboardHook?.Dispose();
            MouseHook?.Dispose();
        }

        public class GlobalHook : IDisposable
        {
            #region // storage

            /// <inheritdoc cref="HookType"/>
            public HookType HookType { get; }

            /// <summary>
            /// Hook callback delegate.
            /// </summary>
            public HookProc HookProc { get; set; }

            /// <summary>
            /// Hook handle.
            /// </summary>
            public IntPtr HookHandle { get; private set; }

            #endregion

            #region // ctor

            /// <summary />
            public GlobalHook(HookType hookType, HookProc hookProc)
            {
                HookType = hookType;
                HookProc = hookProc;
                HookHandle = Hook(HookType, HookProc);
            }

            /// <inheritdoc />
            public void Dispose()
            {
                ReleaseUnmanagedResources();

                // prevent destructor (since we already release unmanaged resources)
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// Destructor. This should be called by finalized if Dispose is not called.
            /// </summary>
            ~GlobalHook()
            {
                ReleaseUnmanagedResources();
            }

            /// <summary>
            /// Unhook. Should be called once.
            /// </summary>
            private void ReleaseUnmanagedResources()
            {
                // unhook and reset handle
                UnHook(HookHandle);
                HookHandle = default;

                // release callback reference, will let GC to collect it
                HookProc = default;
            }

            #endregion

            #region // routines

            /// <summary>
            /// Install an application-defined hook procedure into a hook chain.
            /// </summary>
            public static IntPtr Hook(HookType hookType, HookProc hookProc)
            {
                using (var currentProcess = Process.GetCurrentProcess())
                {
                    using (var curModule = currentProcess.MainModule)
                    {
                        if (curModule is null)
                        {
                            throw new ArgumentNullException(nameof(curModule));
                        }

                        var hHook = User32.SetWindowsHookEx((int)hookType, hookProc, Kernel32.GetModuleHandle(curModule.ModuleName), 0);
                        if (hHook == IntPtr.Zero)
                        {
                            throw new ArgumentException("Hook failed.");
                        }

                        return hHook;
                    }
                }
            }

            /// <summary>
            /// Remove a hook procedure installed in a hook chain.
            /// </summary>
            private static void UnHook(IntPtr hHook)
            {
                if (!User32.UnhookWindowsHookEx(hHook))
                {
                    throw new ArgumentException("UnHook failed.");
                }
            }

            #endregion
        }
        public class MessagePump : BaseFeature
        {
            public MessagePump()
            {

            }
            protected override string ThreadName => nameof(MessagePump);
            protected override void FrameAction()
            {
                Console.WriteLine("i");
                MSG msg;
                while (User32.GetMessage(out msg, IntPtr.Zero, 0, 0) != 1)
                {
                    User32.TranslateMessage(ref msg);
                    User32.DispatchMessage(ref msg);
                }
            }
        }
    }
}
