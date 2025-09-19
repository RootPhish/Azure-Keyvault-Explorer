using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.KeyVault;
using Azure.Security.KeyVault.Secrets;
using System.Runtime.InteropServices;

namespace AzureKeyvaultExplorer
{
    public partial class MainForm : Form
    {
        public const uint WDA_NONE = 0;
        public const uint WDA_MONITOR = 1;
        public const uint WDA_EXCLUDEFROMCAPTURE = 17;

        private bool _revealed;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowDisplayAffinity(IntPtr hWnd, uint dwAffinity);

        public MainForm()
        {
            InitializeComponent();

            txtValue.UseSystemPasswordChar = true;

            btnEye.Click += (_, __) => ToggleReveal();
            btnCopy.Click += async (_, __) => await CopyPasswordAsync();
            btnLoadSubs.Click += async (_, __) => await LoadSubsAsync();
            lbSubs.SelectedIndexChanged += async (_, __) => await LoadVaultsAsync();
            lbVaults.SelectedIndexChanged += async (_, __) => await LoadSecretsAsync();
            lbSecrets.SelectedIndexChanged += async (_, __) => await GetSecretValueAsync();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetWindowDisplayAffinity(this.Handle, WDA_EXCLUDEFROMCAPTURE);
        }

        private readonly TokenCredential _credential = new InteractiveBrowserCredential();

        private class SubItem
        {
            public string Name { get; set; } = "";
            public Azure.Core.ResourceIdentifier ResourceId { get; set; }

            public override string ToString() => Name;
        }

        private class VaultItem
        {
            public string Name { get; set; } = "";
            public string VaultUri { get; set; } = "";
            public Azure.Core.ResourceIdentifier ResourceId { get; set; }

            public override string ToString() => Name; // display in ComboBox
        }

        private async Task LoadSubsAsync()
        {
            try
            {
                lbSubs.Items.Clear();
                lbVaults.Items.Clear();
                lbSecrets.Items.Clear();
                txtValue.Clear();

                var arm = new ArmClient(_credential);
                var subs = arm.GetSubscriptions().GetAllAsync();

                await foreach (var sub in subs)
                {
                    lbSubs.Items.Add(new SubItem
                    {
                        Name = sub.Data.DisplayName,
                        ResourceId = sub.Id,
                    });
                }
                if (lbSubs.Items.Count == 0)
                {
                    MessageBox.Show("No Subscriptions found for your account.", "Info");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load subscriptions:\n{ex.Message}", "Error");
            }
        }

        private async Task LoadVaultsAsync()
        {
            try
            {
                lbSubs.Enabled = false;
                lbVaults.Items.Clear();
                lbSecrets.Items.Clear();
                txtValue.Clear();
                if (lbSubs.SelectedItem is not SubItem subItem)
                {
                    MessageBox.Show("Please select a Subscription first.", "Info");
                    return;
                }
                var arm = new ArmClient(_credential);
                var sub = arm.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{subItem.ResourceId.Name}"));
                var kvCollection = sub.GetKeyVaultsAsync();

                await foreach (var kv in kvCollection)
                {
                    lbVaults.Items.Add(new VaultItem
                    {
                        Name = kv.Data.Name,
                        ResourceId = kv.Id,
                        VaultUri = kv.Data.Properties.VaultUri.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load vaults for selected subscription.\nDo you have the correct authorizations?", "Error");
            }
            finally
            {
                lbSubs.Enabled = true;
            }
        }

        private async Task LoadSecretsAsync()
        {
            try
            {
                lbVaults.Enabled = false;
                lbSecrets.Items.Clear();
                txtValue.Clear();
                if (lbVaults.SelectedItem is not VaultItem vault)
                {
                    MessageBox.Show("Please select a Vault first.", "Info");
                    return;
                }
                var secretClient = new SecretClient(new Uri(vault.VaultUri), _credential);
                await foreach (SecretProperties props in secretClient.GetPropertiesOfSecretsAsync())
                {
                    lbSecrets.Items.Add(props.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load secrets:\n{ex.Message}", "Error");
            }
            finally
            {
                lbVaults.Enabled = true;
            }
        }

        private async Task GetSecretValueAsync()
        {
            try
            {
                lbSecrets.Enabled = false;
                txtValue.Clear();
                if (lbVaults.SelectedItem is not VaultItem vault)
                {
                    MessageBox.Show("Please select a Vault first.", "Info");
                    return;
                }
                if (lbSecrets.SelectedItem is not string secretName || string.IsNullOrWhiteSpace(secretName))
                {
                    MessageBox.Show("Please select a secret.", "Info");
                    return;
                }

                var secretClient = new SecretClient(new Uri(vault.VaultUri), _credential);
                KeyVaultSecret secret = await secretClient.GetSecretAsync(secretName);
                txtValue.Text = secret.Value;
            }
            catch (RequestFailedException rfe)
            {
                MessageBox.Show($"Azure request failed:\n{rfe.Message}", "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to get secret value:\n{ex.Message}", "Error");
            }
            finally
            {
                lbSecrets.Enabled = true;
            }
        }

        private async Task CopyPasswordAsync()
        {
            try
            {
                Clipboard.SetText(txtValue.Text ?? string.Empty);

                btnCopy.Enabled = false;
                btnCopy.ImageIndex = 1;
                await Task.Delay(900);
                btnCopy.ImageIndex = 0;
            }
            finally
            {
                btnCopy.Enabled = true;
            }
        }

        private void ToggleReveal()
        {
            _revealed = !_revealed;
            txtValue.UseSystemPasswordChar = !_revealed;
            btnEye.ImageIndex = (btnEye.ImageIndex + 1) % 2;
        }

    }
}
