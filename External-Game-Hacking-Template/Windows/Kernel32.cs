using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace External_Game_Hacking_Template.Windows
{
    internal class Kernel32
    {
        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process
        /// into the specified buffer of the current process.
        /// Any process that has a handle with PROCESS_VM_READ access can call the function.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, int size, out IntPtr lpNumberOfBytesRead);

        /// <summary>
        /// Writes data to an area of memory in a specified process.
        /// The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesWritten);

        /// <summary>
        ///     Retrieves a module handle for the specified module. The module must have been loaded by the calling process.
        ///     To avoid the race conditions described in the Remarks section, use the GetModuleHandleEx function.
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
