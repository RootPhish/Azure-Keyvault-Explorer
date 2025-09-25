using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.KeyVault;
using Azure.Security.KeyVault.Secrets;
using AzureKeyvaultExplorer.Classes;
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
            connectToolStripMenuItem.Click += async (_, __) => await LoadSubsAsync();
            lbSubs.SelectedIndexChanged += async (_, __) => await LoadVaultsAsync();
            lbVaults.SelectedIndexChanged += async (_, __) => await LoadSecretsAsync();
            lbSecrets.SelectedIndexChanged += async (_, __) => await GetSecretValueAsync();

            btnEye.ImageList = new();
            btnEye.ImageList.Images.Add(Properties.Resources.eye_open);
            btnEye.ImageList.Images.Add(Properties.Resources.eye_closed);

            btnCopy.ImageList = new();
            btnCopy.ImageList.Images.Add(Properties.Resources.copy);
            btnCopy.ImageList.Images.Add(Properties.Resources.check);

        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetWindowDisplayAffinity(this.Handle, WDA_EXCLUDEFROMCAPTURE);
        }

        private TokenCredential? _credential;

        private class SubItem
        {
            public string Name { get; set; } = "";
            public Azure.Core.ResourceIdentifier? ResourceId { get; set; }

            public override string ToString() => Name;
        }

        private class VaultItem
        {
            public string Name { get; set; } = "";
            public string VaultUri { get; set; } = "";
            public Azure.Core.ResourceIdentifier? ResourceId { get; set; }

            public override string ToString() => Name;
        }

        private async Task LoadSubsAsync()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.TenantID) || string.IsNullOrEmpty(Properties.Settings.Default.ClientID))
            {
                MessageBox.Show("No TenantID and/or ClientID defined. Please set in the Preferences");
                return;
            }
            _credential = new MsalTokenCredential(Properties.Settings.Default.ClientID, Properties.Settings.Default.TenantID);

            try
            {
                lbSubs.Items.Clear();
                lbVaults.Items.Clear();
                lbSecrets.Items.Clear();
                btnCopy.Enabled = false;
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

        List<VaultItem> vaultItems = new List<VaultItem>(); // save the loaded vaults for filtering

        private async Task LoadVaultsAsync()
        {
            try
            {
                lbSubs.Enabled = false;
                lbVaults.Items.Clear();
                lbSecrets.Items.Clear();
                btnCopy.Enabled = false;
                txtValue.Clear();

                if (lbSubs.SelectedItem is not SubItem subItem)
                {
                    MessageBox.Show("Please select a Subscription first.", "Info");
                    return;
                }
                var arm = new ArmClient(_credential);
                var sub = arm.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{subItem.ResourceId?.Name}"));
                var kvCollection = sub.GetKeyVaultsAsync();

                progressBar.Style = ProgressBarStyle.Marquee;
                statusLabel.Text = "Loading vaults...";

                await foreach (var kv in kvCollection)
                {
                    VaultItem item = new VaultItem
                    {
                        Name = kv.Data.Name,
                        ResourceId = kv.Id,
                        VaultUri = kv.Data.Properties.VaultUri.ToString()
                    };
                    if (kv.Data.Name.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        lbVaults.Items.Add(item);
                    }
                    vaultItems.Add(item);
                }

                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 100;
                statusLabel.Text = $"Loaded {lbVaults.Items.Count} vaults.";
            }
            catch
            {
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 0;
                statusLabel.Text = "";
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
                btnCopy.Enabled = false;
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
                btnCopy.Enabled = false;
                btnCopy.Enabled = false;
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
                btnCopy.Enabled = true;
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
                Clipboard.SetText(txtValue.Text ?? "");

                btnCopy.ImageIndex = 1;
                await Task.Delay(900);
                btnCopy.ImageIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy to clipboard:\n{ex.Message}", "Error");
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
            lbSecrets.Items.Clear();
            btnCopy.Enabled = false;
            txtValue.Clear();
            lbVaults.Items.Clear();
            foreach (string str in vaultItems.Select(v => v.Name).Where(n => n.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase)))
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

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
