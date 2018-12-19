using System.Configuration;
using System;

namespace Sokoban
{
    class SettingsManager
    {
        public int Height { get; private set; }
        public int Width { get; private set; }
        public int HeightFooter { get; private set; }
        public int CurrentLevel { get; private set; }
        public int CountLevel { get; private set; }
        public int SizeSprite { get; private set; }

        public SettingsManager()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                Height = int.Parse(appSettings["height"]);
                Width = int.Parse(appSettings["width"]);
                HeightFooter = int.Parse(appSettings["heightFooter"]);
                CurrentLevel = int.Parse(appSettings["currentLevel"]);
                CountLevel = int.Parse(appSettings["countLevel"]);
                SizeSprite = int.Parse(appSettings["sizeSprite"]);
            }
            catch (ConfigurationErrorsException)
            {
                
            }
        }

        public bool UpdateCurrentLevel(int newLevel)
        {
            if (newLevel > CountLevel || newLevel < 1)
            {
                throw new ArgumentException(nameof(newLevel));
            }

            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings["currentLevel"] == null)
                {
                    settings.Add("currentLevel", newLevel.ToString());
                }
                else
                {
                    settings["currentLevel"].Value = newLevel.ToString();
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                return true;
            }
            catch (ConfigurationErrorsException)
            {
                return false;
            }
        }
    }
}
