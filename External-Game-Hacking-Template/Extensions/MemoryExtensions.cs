using System;
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
    }
}
