using External_Game_Hacking_Template.MemoryManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace External_Game_Hacking_Template.Extensions
{
    internal class MemoryExtensions
    {
        public static byte[] StructureToByteArray(object obj)
        {
            var length = Marshal.SizeOf(obj);

            var array = new byte[length];

            var pointer = Marshal.AllocHGlobal(length);

            Marshal.StructureToPtr(obj, pointer, true);
            Marshal.Copy(pointer, array, 0, length);
            Marshal.FreeHGlobal(pointer);

            return array;
        }

        public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);

            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }
        public static float[] ConvertToFloatArray(byte[] bytes)
        {
            if (bytes.Length % 4 != 0) throw new ArgumentException();

            var floats = new float[bytes.Length / 4];

            for (var i = 0; i < floats.Length; i++) floats[i] = BitConverter.ToSingle(bytes, i * 4);

            return floats;
        }
        /// <summary>
        /// Based on a byte array, pick out an entity number based on a size
        /// Example: Pass in the entire EntityList from CSGO as byte array
        /// The players are each 0x10 (16) in size; thus you use 0x10 * index to get the base pointer of each player
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int GetInt(byte[] bytes, int offset)
        {
            return bytes[offset] | (bytes[++offset] << 8) | (bytes[++offset] << 16) | (bytes[++offset] << 24);
        }

        /// <summary>
        /// Finds a specific pattern within a processmodule and returns the address
        /// The pattern is in IDA-style aka "A1 ? ? ? ? 33 D2 6A 00 6A 00 33 C9 89 B0"
        /// </summary>
        /// <param name="process">Process to find the pattern within</param>
        /// <param name="patternToFind">The pattern to find</param>
        /// <param name="subtractModuleBase">Subtract module base from final address</param>
        /// <param name="moduleName">Module the signature recides in</param>
        /// <param name="relative">Flag indicating if the pattern is relative</param>
        /// <param name="offset">List of offsets for the relatives</param>
        /// <param name="read"></param>
        /// <param name="startAddress"></param>
        /// <returns></returns>
        public static IntPtr FindPattern(Process process, string patternToFind, bool subtractModuleBase, string moduleName, bool relative, List<int> offset, bool read = false, int startAddress = 0)
        {
            ProcessModule module = null;
            var temp = new List<byte>();
            var mask = "";

            foreach (ProcessModule processModule in process.Modules)
            {
                if (processModule.ModuleName == moduleName)
                {
                    module = processModule;
                }
            }

            if (module == null)
            {
                return IntPtr.Zero;
            }

            var splitString = patternToFind.Split(' ');

            foreach (var character in splitString)
            {
                if (character == "?" || character == "00")
                {
                    temp.Add(0x00);
                    mask += "?";
                }
                else
                {
                    temp.Add((byte)int.Parse(character, NumberStyles.HexNumber));
                    mask += "x";
                }
            }

            var pattern = temp.ToArray();

            var moduleSize = module.ModuleMemorySize;

            var moduleBase = module.BaseAddress;

            if (startAddress != 0)
            {
                moduleSize = (int)moduleBase + moduleSize - startAddress;
                moduleBase = new IntPtr(startAddress);
            }

            if (moduleSize == 0)
                throw new Exception($"Size of module {module} is INVALID.");

            var moduleBytes = new byte[moduleSize];

            if (MemoryLibrary.ReadMemoryWithSuccess(moduleBase, moduleSize, out moduleBytes))
            {
                for (var i = 0; i < moduleSize; i++)
                {
                    var found = true;

                    for (var length = 0; length < mask.Length; length++)
                    {
                        found = mask[length] == '?' || moduleBytes[length + i] == pattern[length];

                        if (!found)
                        {
                            break;
                        }
                    }

                    if (found)
                    {
                        i += (int)moduleBase;
                        if (read && offset != null)
                        {
                            foreach (var off in offset)
                            {
                                i = MemoryLibrary.ReadMemory<int>(new IntPtr(i + off));
                            }
                        }

                        if (subtractModuleBase)
                        {
                            i -= (int)moduleBase;
                        }
                        GC.Collect();
                        return new IntPtr(i);
                    }
                }
            }
            GC.Collect();
            return IntPtr.Zero;
        }
    }
}
