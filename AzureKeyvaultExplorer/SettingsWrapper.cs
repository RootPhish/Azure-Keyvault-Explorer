using System.ComponentModel;

namespace AzureKeyvaultExplorer
{
    internal class SettingsWrapper
    {
        [Category("Azure Settings"), Description("The ID of the Tenant where the Enterprise App lives")]
        public string TenantID
        {
            get => Properties.Settings.Default.TenantID;
            set => Properties.Settings.Default.TenantID = value;
        }

        [Category("Azure Settings"), Description("The Client ID of the Enterprise App")]
        public string ClientID
        {
            get => Properties.Settings.Default.ClientID;
            set => Properties.Settings.Default.ClientID = value;
        }
    }
}
