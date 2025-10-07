using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureKeyvaultExplorer.Classes
{
    internal class SettingsManager
    {
        private static readonly string SettingsFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AzureKeyvaultExplorer");

        private static readonly string SettingsFile = Path.Combine(SettingsFolder, "settings.json");

        public static AppSettings Load()
        {
            if (!Directory.Exists(SettingsFolder))
                Directory.CreateDirectory(SettingsFolder);

            if (!File.Exists(SettingsFile))
            {
                // File doesn't exist → return defaults
                var defaultSettings = new AppSettings();
                Save(defaultSettings);
                return defaultSettings;
            }

            string json = File.ReadAllText(SettingsFile);
            try
            {
                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            catch
            {
                // Corrupt file → fallback to defaults
                return new AppSettings();
            }
        }

        public static void Save(AppSettings settings)
        {
            if (!Directory.Exists(SettingsFolder))
                Directory.CreateDirectory(SettingsFolder);

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFile, json);
        }
    }
}
