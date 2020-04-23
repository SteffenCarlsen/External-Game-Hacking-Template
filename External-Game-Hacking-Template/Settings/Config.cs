using System.IO;
using External_Game_Hacking_Template.Extensions;

namespace External_Game_Hacking_Template.Settings
{
    public class Config
    {
        private readonly string CONFIG_PATH = SteamExtensions.GetSteamPath();

        public string Name { get; private set; }
        public AimbotConfig AimbotConfig;

        public Config(string name = "default")
        {
            Name = name;
            AimbotConfig = new AimbotConfig();
        }

        public Config() { }

        public bool SaveConfig()
        {
           return SettingsExtensions.WriteToJsonFile<Config>(Path.Combine(CONFIG_PATH, Name + ".json"),this);
        }

        public void LoadConfig()
        {
            var configFile = SettingsExtensions.ReadFromJsonFile<Config>(Path.Combine(CONFIG_PATH, Name + ".json"));
            this.Name = configFile.Name;
            this.AimbotConfig = configFile.AimbotConfig;
        }

        public void CreateDefaultCfg(string name = "default")
        {
            AimbotConfig.Enabled = true;
        }

    }
}
