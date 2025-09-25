using System.ComponentModel;

namespace AzureKeyvaultExplorer.Classes
{
    internal class SettingsWrapper
    {
        [Category("Azure Settings"), Description("The Tenant ID for the App registration in Azure")]
        public string TenantID
        {
            get => Properties.Settings.Default.TenantID;
            set => Properties.Settings.Default.TenantID = value;
        }

        [Category("Azure Settings"), Description("The Client ID of the App registration in Azure")]
        public string ClientID
        {
            get => Properties.Settings.Default.ClientID;
            set => Properties.Settings.Default.ClientID = value;
        }
    }
}
