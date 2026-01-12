using System.ComponentModel;

namespace AzureKeyvaultExplorer.Classes
{
    internal class SettingsWrapper(AppSettings settings)
    {
        private readonly AppSettings _settings = settings;

        [Category("Azure Settings")]
        [Description("The Tenant ID for the App registration in Azure")]
        public string TenantID
        {
            get => _settings.TenantID;
            set => _settings.TenantID = value;
        }

        [Category("Azure Settings")]
        [Description("The Client ID of the App registration in Azure")]
        public string ClientID
        {
            get => _settings.ClientID;
            set => _settings.ClientID = value;
        }

        [Category("Security")]
        [Description("The amount of seconds after which a copied password will be erased from the clipboard")]
        [DisplayName("Clear Clipboard After (seconds)")]
        public int ClearClipboardAfterSeconds
        {
            get => _settings.ClearClipboardAfterSeconds;
            set => _settings.ClearClipboardAfterSeconds = value;
        }

        [Category("Misc")]
        [Description("Secret name to be interpreted as TOTP secret")]
        [DisplayName("TOTP Secret Name")]
        public string TOTP
        {
            get => _settings.TOTP;
            set => _settings.TOTP = value;
        }
    }
}
