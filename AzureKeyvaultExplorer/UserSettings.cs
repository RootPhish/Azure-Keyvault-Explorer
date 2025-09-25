using System.ComponentModel;
using System.Configuration;

namespace AzureKeyvaultExplorer
{
    internal class UserSettings
    {
        [Category("Azure Settings"), Description("The ID of the Tenant where the Enterprise App")]
        public string TenantID { get; set; } = "";

        [Category("Azure Settings"), Description("The Client ID of the Enterprise App")]
        public string ClientID { get; set; } = "";

        public static UserSettings Load()
        {
            var settings = new UserSettings();
            settings.TenantID = ConfigurationManager.AppSettings["TenantID"] ?? "";
            settings.ClientID = ConfigurationManager.AppSettings["ClientID"] ?? "";
            return settings;
        }

        public void Save()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("TenantID");
            config.AppSettings.Settings.Add("TenantID", TenantID);

            config.AppSettings.Settings.Remove("ClientID");
            config.AppSettings.Settings.Add("ClientID", ClientID);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }

}
