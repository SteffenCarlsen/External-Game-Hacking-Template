using System;
using System.IO;
using Microsoft.Win32;

namespace External_Game_Hacking_Template.Extensions
{
    internal class SteamExtensions
    {
        private const string REG_KEY_STEAM = "Software\\Valve\\Steam";
        private const string STEAM_EXE = "SteamPath";

        public static string GetSteamPath()
        {
            var path = string.Empty;
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(REG_KEY_STEAM);
                var o = key?.GetValue(STEAM_EXE);
                path = (string)o ?? throw new CSGONotFoundException();
                Console.WriteLine("Found Steam path - Set it in the config file");
                return path;
            }
            catch (CSGONotFoundException)
            {
                Console.WriteLine("Unable to find Steam, using default config location");
                return Directory.GetCurrentDirectory();
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to find steam folder, please open the config file and input information manually");
                return Directory.GetCurrentDirectory();
            }
        }
    }

    [Serializable]
    public class CSGONotFoundException : Exception
    {
    }
}