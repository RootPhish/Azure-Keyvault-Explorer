using AzureKeyvaultExplorer.Services;
using AzureKeyvaultExplorer.Classes;
using System.Runtime.InteropServices;
using Azure;

namespace AzureKeyvaultExplorer
{
    public partial class MainForm : Form
    {
        public const uint WDA_NONE = 0;
        public const uint WDA_MONITOR = 1;
        public const uint WDA_EXCLUDEFROMCAPTURE = 17;

        private bool _revealed;
        private MsalTokenCredential _credential;
        private List<KeyvaultItem> _allVaults = new();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowDisplayAffinity(IntPtr hWnd, uint dwAffinity);

        public MainForm()
        {
            InitializeComponent();

            lbVaults.DisplayMember = "Name";

            txtValue.UseSystemPasswordChar = true;

            btnEye.Click += (_, __) => ToggleReveal();
            btnCopy.Click += async (_, __) => await CopyPasswordAsync();
            connectToolStripMenuItem.Click += async (_, __) => await LoadSubsAsync();
            lbSubs.SelectedIndexChanged += async (_, __) => await LoadVaultsAsync();
            lbVaults.SelectedIndexChanged += async (_, __) => await LoadSecretsAsync();
            lbSecrets.SelectedIndexChanged += (_, __) => GetSecretValue();

            btnEye.ImageList = new();
            btnEye.ImageList.Images.Add(Properties.Resources.eye_open);
            btnEye.ImageList.Images.Add(Properties.Resources.eye_closed);

            btnCopy.ImageList = new();
            btnCopy.ImageList.Images.Add(Properties.Resources.copy);
            btnCopy.ImageList.Images.Add(Properties.Resources.check);

            _credential = new MsalTokenCredential(
                Properties.Settings.Default.ClientID,
                Properties.Settings.Default.TenantID);

        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetWindowDisplayAffinity(this.Handle, WDA_EXCLUDEFROMCAPTURE);
        }

        private async Task LoadSubsAsync()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.TenantID) ||
                string.IsNullOrEmpty(Properties.Settings.Default.ClientID))
            {
                MessageBox.Show("No TenantID and/or ClientID defined. Please set in the Preferences");
                return;
            }

            try
            {
                lbSubs.Items.Clear();
                lbVaults.Items.Clear();
                _allVaults.Clear();
                lbSecrets.Items.Clear();
                btnCopy.Enabled = false;
                txtValue.Clear();

                var service = new AzureSubscriptionService(_credential);

                await foreach (var sub in service.GetSubscriptionsAsync())
                {
                    lbSubs.Items.Add(sub);
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
                _allVaults.Clear();
                lbSecrets.Items.Clear();
                btnCopy.Enabled = false;
                txtValue.Clear();

                if (lbSubs.SelectedItem is not SubscriptionItem subItem)
                {
                    MessageBox.Show("Please select a Subscription first.", "Info");
                    return;
                }

                progressBar.Style = ProgressBarStyle.Marquee;
                statusLabel.Text = "Loading vaults...";

                var service = new AzureKeyvaultService(_credential);

                await foreach (var kv in service.GetKeyvaultsAsync(subItem))
                {
                    _allVaults.Add(kv);
                    if (kv.Name.Contains(txtFilter.Text))
                    {
                        lbVaults.Items.Add(kv);
                    }
                }
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 100;
                statusLabel.Text = $"Loaded {_allVaults.Count} vaults.";
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

                if (lbVaults.SelectedItem is not KeyvaultItem vault)
                {
                    MessageBox.Show("Please select a Vault first.", "Info");
                    return;
                }

                var service = new AzureSecretService(_credential);

                await foreach (var secret in service.GetSecretsAsync(vault))
                {
                    lbSecrets.Items.Add(secret);
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

        private void GetSecretValue()
        {
            try
            {
                lbSecrets.Enabled = false;
                btnCopy.Enabled = false;
                btnCopy.Enabled = false;
                txtValue.Clear();

                if (lbVaults.SelectedItem is not KeyvaultItem vault)
                {
                    MessageBox.Show("Please select a Vault first.", "Info");
                    return;
                }
                if (lbSecrets.SelectedItem is not string secretName || string.IsNullOrWhiteSpace(secretName))
                {
                    MessageBox.Show("Please select a secret.", "Info");
                    return;
                }
                var service = new AzureSecretService(_credential);

                txtValue.Text = service.GetSecretValue(vault, secretName);
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
                // Keep focus on the list to allow changing selection with keyboard
                this.BeginInvoke((Action)(() => lbSecrets.Focus()));
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

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            lbSecrets.Items.Clear();
            btnCopy.Enabled = false;
            txtValue.Clear();
            lbVaults.Items.Clear();
            foreach (string str in _allVaults.Select(v => v.Name).Where(n => n.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase)))
            {
                lbVaults.Items.Add(_allVaults.First(v => v.Name == str));
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
