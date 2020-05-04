using External_Game_Hacking_Template.Features;
using External_Game_Hacking_Template.Windows;
using External_Game_Hacking_Template.Windows.Delegates;
using External_Game_Hacking_Template.Windows.Enums;
using External_Game_Hacking_Template.Windows.Structs;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace External_Game_Hacking_Template.Managers
{
    public sealed class InputManager
    {
        private static readonly object padlock = new object();
        private static InputManager instance = null;
        private static ConcurrentDictionary<MouseMessage, Action<Point, MouseMessage>> MouseMessages;
        private static ConcurrentDictionary<Keys, Action<Keys, KeyboardMessage>> KeyboardMessages;
        public GlobalHook MouseHook { get; private set; }

        /// <summary>
        /// If specified to true, will not call CallNextHookEx
        /// </summary>
        public bool StealthyMouseHook { get; private set; } = false;

        /// <summary>
        /// If specified to true, will not call CallNextHookEx
        /// </summary>
        public bool StealthyKeyboardHook { get; private set; } = false;

        public GlobalHook KeyboardHook { get; private set; }

        /// <summary>
        /// Singleton
        /// </summary>
        private InputManager()
        {
            MouseMessages = new ConcurrentDictionary<MouseMessage, Action<Point, MouseMessage>>(4, 5);
            KeyboardMessages = new ConcurrentDictionary<Keys, Action<Keys, KeyboardMessage>>(4, 20);
        }

        public static InputManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new InputManager();
                    }

                    return instance;
                }
            }
        }


        /// <summary>
        /// Adds a mouse event based on a MouseMessage
        /// </summary>
        /// <param name="eventParam"></param>
        /// <param name="targetEvent"></param>
        /// <returns>Whether the event could be added</returns>
        internal bool AddMouseMessage(MouseMessage eventParam, Action<Point, MouseMessage> targetEvent)
        {
            return MouseMessages.TryAdd(eventParam, targetEvent);
        }
        /// <summary>
        /// Adds a keyboard event based on a key
        /// </summary>
        /// <param name="eventParam"></param>
        /// <param name="targetEvent"></param>
        /// <returns>Whether the event could be added</returns>
        internal bool AddKeyboardMessage(Keys eventParam, Action<Keys, KeyboardMessage> targetEvent)
        {
            return KeyboardMessages.TryAdd(eventParam, targetEvent);
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
                var mouseMessage = (MouseMessage)wParam;
                var exists = MouseMessages.TryGetValue(mouseMessage, out var eventMethod);
                if (exists)
                {
                    eventMethod(mouseStructure.pt, mouseMessage);
                }
            }
            if (!StealthyMouseHook)
            {
                return User32.CallNextHookEx(MouseHook.HookHandle, nCode, wParam, lParam);
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
                var keyboardMessage = (KeyboardMessage)wParam;
                var exists = KeyboardMessages.TryGetValue((Keys)keyboardStructure.vkCode, out var eventMethod);
                if (exists)
                {
                    Console.WriteLine(keyboardStructure.vkCode);
                    eventMethod((Keys)keyboardStructure.vkCode, keyboardMessage);
                }
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
                GC.KeepAlive(HookProc);
                GC.KeepAlive(HookHandle);
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
            /// 

            public static IntPtr Hook(HookType hookType, HookProc hookProc)
            {
                using var currentProcess = Process.GetCurrentProcess();
                using var curModule = currentProcess.MainModule;
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
    }
}
