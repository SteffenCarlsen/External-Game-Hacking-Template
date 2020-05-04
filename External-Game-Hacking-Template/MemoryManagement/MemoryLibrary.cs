using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using MemoryExtensions = External_Game_Hacking_Template.Extensions.MemoryExtensions;

namespace External_Game_Hacking_Template.MemoryManagement
{
    internal class MemoryLibrary
    {
        private static IntPtr _processHandle;

        /// <summary>
        /// Initialize the memory library to a specific process
        /// </summary>
        /// <param name="processId"></param>
        public static void Initialize(int processId)
        {
            _processHandle = Windows.Kernel32.OpenProcess(
                (int) (Windows.Enums.ProcessAccessType.PROCESS_VM_OPERATION |
                                   Windows.Enums.ProcessAccessType.PROCESS_VM_READ |
                                   Windows.Enums.ProcessAccessType.PROCESS_VM_WRITE), false, processId);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadMemory<T>(IntPtr address) where T : struct
        {
            var ByteSize = Marshal.SizeOf(typeof(T));

            var buffer = new byte[ByteSize];

             Windows.Kernel32.ReadProcessMemory(_processHandle, address, buffer, buffer.Length, out var readBytes);

            return MemoryExtensions.ByteArrayToStructure<T>(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ReadMemory(IntPtr address, int size)
        {
            var buffer = new byte[size];

            Windows.Kernel32.ReadProcessMemory(_processHandle, address, buffer, size, out var readBytes);

            return buffer;
        }

        public static bool ReadMemoryWithSuccess(IntPtr address, int size, out byte[] bytes)
        {
            bytes = new byte[size];

            var status = Windows.Kernel32.ReadProcessMemory(_processHandle, address, bytes, size, out var readBytes);

            return status;
        }

        public static float[] ReadMatrix<T>(IntPtr address, int matrixSize) where T : struct
        {
            var ByteSize = Marshal.SizeOf(typeof(T));

            var buffer = new byte[ByteSize * matrixSize];

            Windows.Kernel32.ReadProcessMemory(_processHandle, address, buffer, buffer.Length, out var readBytes);

            return MemoryExtensions.ConvertToFloatArray(buffer);
        }

        public static string ReadString(IntPtr address, Encoding encoding, int maximumLength = 512)
        {
            var buffer = ReadMemory(address, maximumLength);
            var ret = encoding.GetString(buffer);

            if (ret.IndexOf('\0') != -1)
            {
                ret = ret.Remove(ret.IndexOf('\0'));
            }

            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteMemory<T>(IntPtr address, object value) where T : struct
        {
            var buffer = MemoryExtensions.StructureToByteArray(value);

            return Windows.Kernel32.WriteProcessMemory(_processHandle, address, buffer, buffer.Length, out var writtenBytes) && writtenBytes != 0;
        }
    }
}