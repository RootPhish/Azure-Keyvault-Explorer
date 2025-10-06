using Azure;
using Library;
using System.Runtime.InteropServices;

namespace AzureKeyvaultExplorer
{
    public partial class MainForm : Form
    {
        public const uint WDA_NONE = 0;
        public const uint WDA_MONITOR = 1;
        public const uint WDA_EXCLUDEFROMCAPTURE = 17;

        private bool _revealed;
        private MsalTokenCredential? _credential;
        private List<string> _allVaults = new();

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

        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetWindowDisplayAffinity(this.Handle, WDA_EXCLUDEFROMCAPTURE);
        }

        private async Task LoadSubsAsync()
        {
            if (_credential == null)
            {
                MessageBox.Show("Invalid authentication configuration. Please check your Preferences.");
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

        private int _lastSubIndex = -2;
        private async Task LoadVaultsAsync()
        {
            if (_credential == null)
            {
                MessageBox.Show("Invalid authentication configuration. Please check your Preferences.");
                return;
            }

            if ((lbSubs.SelectedIndex == -1) || (lbSubs.SelectedIndex == _lastSubIndex))
                return;
            _lastSubIndex = lbSubs.SelectedIndex;
            try
            {
                lbSubs.Enabled = false;
                lbVaults.Items.Clear();
                _allVaults.Clear();
                lbSecrets.Items.Clear();
                btnCopy.Enabled = false;
                txtValue.Clear();

                SubscriptionItem? subItem = lbSubs.SelectedItem as SubscriptionItem;
                if (subItem == null)
                    return;

                progressBar.Style = ProgressBarStyle.Marquee;
                statusLabel.Text = "Loading vaults...";

                var service = new AzureKeyvaultService(_credential);

                await foreach (var kv in service.GetKeyvaultsAsync(subItem))
                {
                    _allVaults.Add(kv);
                    if (kv.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase))
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

        private int _lastVaultIndex = -2;
        private async Task LoadSecretsAsync()
        {
            if (_credential == null)
            {
                MessageBox.Show("Invalid authentication configuration. Please check your Preferences.");
                return;
            }

            if ((lbVaults.SelectedIndex == -1) || (lbVaults.SelectedIndex == _lastVaultIndex))
                return;
            _lastVaultIndex = lbVaults.SelectedIndex;
            try
            {
                string? vault = lbVaults.SelectedItem?.ToString();

                if (vault == null)
                    return;

                lbVaults.Enabled = false;
                lbSecrets.Items.Clear();
                btnCopy.Enabled = false;
                _lastSecretIndex = -2;
                txtValue.Clear();

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

        private int _lastSecretIndex = -2;
        private void GetSecretValue()
        {
            if (_credential == null)
            {
                MessageBox.Show("Invalid authentication configuration. Please check your Preferences.");
                return;
            }
            SetReveal(false);
            if ((lbSecrets.SelectedIndex == -1) || (lbSecrets.SelectedIndex == _lastSecretIndex))
                return;
            _lastSecretIndex = lbSecrets.SelectedIndex;
            try
            {
                lbSecrets.Enabled = false;
                btnCopy.Enabled = false;
                txtValue.Clear();

                string? vault = lbVaults.SelectedItem?.ToString();
                string? secretName = lbSecrets.SelectedItem as string;

                if ((vault == null) || (secretName == null))
                    return;

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
                btnCopy.Enabled = true;
                // Keep focus on the list to allow changing selection with keyboard
                this.BeginInvoke((Action)(() => lbSecrets.Focus()));
            }
        }

        private async Task CopyPasswordAsync()
        {
            try
            {
                Clipboard.SetText(txtValue.Text ?? "");

                _ = ClearClipboardAfterDelayAsync(Properties.Settings.Default.ClearClipboard);

                btnCopy.ImageIndex = 1;
                await Task.Delay(900);
                btnCopy.ImageIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy to clipboard:\n{ex.Message}", "Error");
            }
        }

        private async Task ClearClipboardAfterDelayAsync(int seconds)
        {
            await Task.Delay(seconds * 1000);
            Clipboard.Clear();
        }

        private void ToggleReveal()
        {
            SetReveal(!_revealed);
        }

        private void SetReveal(bool reveal)
        {
            _revealed = reveal;
            txtValue.UseSystemPasswordChar = !_revealed;
            btnEye.ImageIndex = _revealed ? 1 : 0;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            lbSecrets.Items.Clear();
            btnCopy.Enabled = false;
            txtValue.Clear();
            lbVaults.Items.Clear();
            _lastVaultIndex = -2;
            foreach (string str in _allVaults.Where(n => n.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase)))
            {
                lbVaults.Items.Add(_allVaults.First(v => v == str));
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
            var dialogResult = settingsForm.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            try
            {
                _credential = new MsalTokenCredential(
                    Properties.Settings.Default.ClientID,
                    Properties.Settings.Default.TenantID);
            }

            catch
            {
                MessageBox.Show("Invalid authentication configuration. Please check your Preferences.");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (lbVaults.Items.Count > 0)
                {
                    lbVaults.SelectedIndex = 0;
                }
            }
            Console.WriteLine(e.KeyCode);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.ClientID) ||
                string.IsNullOrWhiteSpace(Properties.Settings.Default.TenantID) ||
                Properties.Settings.Default.ClientID == "00000000-0000-0000-0000-000000000000" ||
                Properties.Settings.Default.TenantID == "00000000-0000-0000-0000-000000000000")
            {
                MessageBox.Show("Missing or incorrect authentication configuration.\nPlease check your Preferences.", "Info");
            }
            _credential = new MsalTokenCredential(
                Properties.Settings.Default.ClientID,
                Properties.Settings.Default.TenantID);
        }
    }
}
