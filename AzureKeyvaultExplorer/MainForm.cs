using Azure;
using Azure.Core;
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

        private UserSettings settings;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowDisplayAffinity(IntPtr hWnd, uint dwAffinity);

        public MainForm()
        {
            InitializeComponent();

            txtValue.UseSystemPasswordChar = true;

            btnEye.Click += (_, __) => ToggleReveal();
            btnCopy.Click += async (_, __) => await CopyPasswordAsync();
            loadSubscriptionsToolStripMenuItem.Click += async (_, __) => await LoadSubsAsync();
            lbSubs.SelectedIndexChanged += async (_, __) => await LoadVaultsAsync();
            lbVaults.SelectedIndexChanged += async (_, __) => await LoadSecretsAsync();
            lbSecrets.SelectedIndexChanged += async (_, __) => await GetSecretValueAsync();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetWindowDisplayAffinity(this.Handle, WDA_EXCLUDEFROMCAPTURE);
        }

        private TokenCredential _credential;

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

            public override string ToString() => Name;
        }

        private async Task LoadSubsAsync()
        {
            if (string.IsNullOrEmpty(settings.TenantID) || string.IsNullOrEmpty(settings.ClientID))
            {
                MessageBox.Show("No TenantID and/or ClientID defined. Please set in the Preferences");
                return;
            }
            _credential = new MsalTokenCredential(settings.ClientID, settings.TenantID);

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

        List<VaultItem> vaultItems = new List<VaultItem>();

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
                    VaultItem item = new VaultItem
                    {
                        Name = kv.Data.Name,
                        ResourceId = kv.Id,
                        VaultUri = kv.Data.Properties.VaultUri.ToString()
                    };
                    if (kv.Data.Name.Contains(textBox1.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        lbVaults.Items.Add(item);
                    }
                    vaultItems.Add(item);
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
                if (lbSecrets.Items.Count != 0)
                {
                    lbSecrets.SelectedIndex = 0;
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            lbVaults.Items.Clear();
            foreach (string str in vaultItems.Select(v => v.Name).Where(n => n.Contains(textBox1.Text, StringComparison.OrdinalIgnoreCase)))
            {
                lbVaults.Items.Add(vaultItems.First(v => v.Name == str));
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.C))
            {
                _ = CopyPasswordAsync();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.C)
            {
                _ = CopyPasswordAsync();
            }
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
            settings = UserSettings.Load();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            settings = UserSettings.Load();
        }

        private void loadSubscriptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
