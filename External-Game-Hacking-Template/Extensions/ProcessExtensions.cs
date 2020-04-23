using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using External_Game_Hacking_Template.Windows;

namespace External_Game_Hacking_Template.Extensions
{
    public static class ProcessExtensions
    {
        public static bool IsWindowInFocus(Process procName)
        {
            var activatedHandle = User32.GetForegroundWindow();

            if (activatedHandle == IntPtr.Zero) return false;

            User32.GetWindowThreadProcessId(activatedHandle, out var activeProcId);

            return activeProcId == procName.Id;
        }

        /// <summary>
        /// Wait for a process to show up
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public static Process WaitForProcess(string procName)
        {
            var process = Process.GetProcessesByName(procName);

            Console.WriteLine($"> Waiting for {procName} to show up");

            while (process.Length < 1)
            {
                process = Process.GetProcessesByName(procName);
                Thread.Sleep(250);
            }

            Console.WriteLine("Process found");
            return process[0];
        }

        /// <summary>
        /// Get version from executing assembly
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            var version = fvi.FileVersion;
            return version;
        }

        /// <summary>
        /// Get last compile-time from executing assembly
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLinkerTimestampUtc()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var location = assembly.Location;
            return GetLinkerTimestampUtc(location);
        }

        private static DateTime GetLinkerTimestampUtc(string filePath)
        {
            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            var bytes = new byte[2048];

            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                file.Read(bytes, 0, bytes.Length);
            }

            var headerPos = BitConverter.ToInt32(bytes, peHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(bytes, headerPos + linkerTimestampOffset);
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddSeconds(secondsSince1970);
        }
    }
}