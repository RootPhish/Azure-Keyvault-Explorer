using Azure;
using Azure.ResourceManager;
using AzureKeyvaultExplorer.Classes;
using Library;
using System.Runtime.InteropServices;
using Timer = System.Windows.Forms.Timer;

namespace AzureKeyvaultExplorer
{
    public partial class MainForm : Form
    {
        public const uint WDA_EXCLUDEFROMCAPTURE = 17;

        private bool _revealed;
        private MsalTokenCredential? _credential;
        private ArmClient? _arm;
        private AzureSecretService? _secretService;

        // Vaults for all subscriptions, loaded with one Resource Graph query after connecting.
        // Kept as a task so selecting a subscription while it is still loading simply awaits it.
        private Task<Dictionary<string, List<VaultItem>>>? _vaultsTask;
        private List<VaultItem> _allVaults = new();

        // Secrets per vault, kept for the session (cleared by Refresh / Connect).
        private readonly Dictionary<Uri, List<SecretListItem>> _secretCache = new();
        private List<SecretListItem> _allSecrets = new();
        private CancellationTokenSource? _secretsCts;

        // The secret filter box is only shown for vaults with at least this many secrets.
        private const int SecretFilterThreshold = 20;

        // Vault to reselect once the vault list has been reloaded (after Refresh).
        private string? _restoreVaultName;

        private readonly Timer _timer = new Timer();
        private string _secret = "";

        // Incremented whenever the displayed secret should change, so slower, outdated requests are ignored.
        private int _secretRequest;

        AppSettings _settings = SettingsManager.Load();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowDisplayAffinity(IntPtr hWnd, uint dwAffinity);

        public MainForm()
        {
            InitializeComponent();

            Text = $"Azure Keyvault Explorer v{GetAppVersion()}";

            // The TOTP countdown only runs while a TOTP secret is shown.
            _timer.Interval = 1000;
            _timer.Tick += (_, __) => RefreshTotp();
            pnlTotp.VisibleChanged += (_, __) =>
            {
                _timer.Enabled = pnlTotp.Visible;
                if (pnlTotp.Visible)
                    RefreshTotp();
            };

            lbVaults.DisplayMember = "Name";

            lbSecrets.DrawMode = DrawMode.OwnerDrawFixed;
            lbSecrets.DrawItem += lbSecrets_DrawItem;

            txtValue.UseSystemPasswordChar = true;

            btnEye.Click += (_, __) => ToggleReveal();
            btnCopy.Click += async (_, __) => await CopyPasswordAsync();
            btnCopyTotp.Click += async (_, __) => await CopyTotpAsync();
            connectToolStripMenuItem.Click += async (_, __) => await LoadSubsAsync();
            refreshToolStripMenuItem.Click += async (_, __) => await RefreshVaultsAsync();
            lbSubs.SelectedIndexChanged += async (_, __) => await LoadVaultsAsync();
            lbVaults.SelectedIndexChanged += async (_, __) => await LoadSecretsAsync();
            lbSecrets.SelectedIndexChanged += async (_, __) => await GetSecretValueAsync();
            txtSecretFilter.TextChanged += (_, __) => ApplySecretFilter();
            txtSecretFilter.KeyDown += txtSecretFilter_KeyDown;
            UpdateSecretFilterVisibility(force: true);

            btnEye.ImageList = new();
            btnEye.ImageList.Images.Add(Properties.Resources.eye_open);
            btnEye.ImageList.Images.Add(Properties.Resources.eye_closed);

            btnCopy.ImageList = new();
            btnCopy.ImageList.Images.Add(Properties.Resources.copy);
            btnCopy.ImageList.Images.Add(Properties.Resources.check);

            btnCopyTotp.ImageList = new();
            btnCopyTotp.ImageList.Images.Add(Properties.Resources.copy);
            btnCopyTotp.ImageList.Images.Add(Properties.Resources.check);
        }

        /// <summary>
        /// The ClickOnce version when installed via ClickOnce, otherwise the assembly's product version.
        /// </summary>
        private static string GetAppVersion()
        {
            string? clickOnceVersion = Environment.GetEnvironmentVariable("ClickOnce_CurrentVersion");
            if (!string.IsNullOrEmpty(clickOnceVersion))
                return clickOnceVersion;

            // Strip the "+<commit hash>" suffix the SDK appends to the informational version.
            string version = Application.ProductVersion;
            int plus = version.IndexOf('+');
            return plus >= 0 ? version[..plus] : version;
        }

        private void RefreshTotp()
        {
            if (_secret.Length == 0)
                return;
            txtTotp.Text = TotpGenerator.GenerateTotp(_secret);
            lblCountdown.Text = $"{SecondsRemaining()}s";
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetWindowDisplayAffinity(this.Handle, WDA_EXCLUDEFROMCAPTURE);
        }

        private bool HasValidSettings() =>
            !string.IsNullOrWhiteSpace(_settings.ClientID) &&
            !string.IsNullOrWhiteSpace(_settings.TenantID) &&
            _settings.ClientID != "00000000-0000-0000-0000-000000000000" &&
            _settings.TenantID != "00000000-0000-0000-0000-000000000000";

        private void SetCredential()
        {
            _credential = new MsalTokenCredential(_settings.ClientID, _settings.TenantID);
            _arm = new ArmClient(_credential);
            _secretService = new AzureSecretService(_credential);
            _vaultsTask = null;
            _secretCache.Clear();
        }

        private bool EnsureConnected()
        {
            if (_arm != null && _secretService != null)
                return true;

            MessageBox.Show("Invalid authentication configuration. Please check your Preferences.");
            return false;
        }

        /// <summary>Remembers the last used subscription. Failing to save is not worth bothering the user.</summary>
        private void RememberSelection(Action<AppSettings> update)
        {
            update(_settings);
            try
            {
                SettingsManager.Save(_settings);
            }
            catch
            {
                // Ignore: this is only a convenience.
            }
        }

        private void ClearSecretValue()
        {
            _secretRequest++;
            _lastSecretIndex = -2;
            btnCopy.Enabled = false;
            txtValue.Clear();
            txtValue.PlaceholderText = "";
            pnlTotp.Visible = false;
            _secret = "";
        }

        private void ClearSecretView()
        {
            // Stop any secret list that is still loading for the previously selected vault.
            _secretsCts?.Cancel();
            _secretsCts = null;

            ClearSecretValue();
            _allSecrets = new();
            lbSecrets.Items.Clear();
            UpdateSecretFilterVisibility();
            UpdateSecretCountLabel();
        }

        private void ClearVaultView()
        {
            _lastVaultIndex = -2;
            _allVaults = new();
            lbVaults.Items.Clear();
            ClearSecretView();
        }

        private async Task LoadSubsAsync()
        {
            if (!EnsureConnected())
                return;

            try
            {
                _lastSubIndex = -2;
                _vaultsTask = null;
                _secretCache.Clear();
                lbSubs.Items.Clear();
                ClearVaultView();

                progressBar.Style = ProgressBarStyle.Marquee;
                statusLabel.Text = "Connecting...";

                var service = new AzureSubscriptionService(_arm!);
                var subs = new List<SubscriptionItem>();
                await foreach (var sub in service.GetSubscriptionsAsync())
                {
                    subs.Add(sub);
                }

                lbSubs.BeginUpdate();
                lbSubs.Items.AddRange(subs.ToArray());
                lbSubs.EndUpdate();

                progressBar.Style = ProgressBarStyle.Blocks;
                statusLabel.Text = $"Loaded {subs.Count} subscriptions.";

                if (subs.Count == 0)
                {
                    MessageBox.Show("No Subscriptions found for your account.", "Info");
                    return;
                }

                // Start loading the vaults of all subscriptions in the background right away.
                // (Must happen before reselecting a subscription, which would otherwise start its own load.)
                _ = StartVaultLoad();

                // Reselect the subscription used last time.
                var lastSub = subs.FirstOrDefault(s =>
                    string.Equals(s.ResourceId?.Name, _settings.LastSubscriptionId, StringComparison.OrdinalIgnoreCase));
                if (lastSub != null)
                    lbSubs.SelectedItem = lastSub;
            }
            catch (Exception ex)
            {
                progressBar.Style = ProgressBarStyle.Blocks;
                statusLabel.Text = "";
                MessageBox.Show($"Failed to load subscriptions:\n{ex.Message}", "Error");
            }
        }

        private Task<Dictionary<string, List<VaultItem>>> StartVaultLoad()
        {
            var subIds = lbSubs.Items
                .OfType<SubscriptionItem>()
                .Select(s => s.ResourceId?.Name)
                .OfType<string>()
                .ToList();

            _vaultsTask = new AzureKeyvaultService(_arm!).GetKeyvaultsBySubscriptionAsync(subIds);
            return _vaultsTask;
        }

        private async Task RefreshVaultsAsync()
        {
            if (!EnsureConnected())
                return;

            if (lbSubs.Items.Count == 0)
            {
                await LoadSubsAsync();
                return;
            }

            // Come back to the same vault after the refresh.
            _restoreVaultName = (lbVaults.SelectedItem as VaultItem)?.Name;

            _secretCache.Clear();
            _ = StartVaultLoad();
            _lastSubIndex = -2;
            await LoadVaultsAsync();
        }

        private int _lastSubIndex = -2;
        private async Task LoadVaultsAsync()
        {
            if (!EnsureConnected())
                return;

            if ((lbSubs.SelectedIndex == -1) || (lbSubs.SelectedIndex == _lastSubIndex))
                return;
            _lastSubIndex = lbSubs.SelectedIndex;

            if (lbSubs.SelectedItem is not SubscriptionItem subItem)
                return;

            string subId = subItem.ResourceId?.Name ?? "";
            if (!string.Equals(subId, _settings.LastSubscriptionId, StringComparison.OrdinalIgnoreCase))
                RememberSelection(s => s.LastSubscriptionId = subId);

            ClearVaultView();

            try
            {
                var task = _vaultsTask ?? StartVaultLoad();
                if (!task.IsCompleted)
                {
                    lbSubs.Enabled = false;
                    progressBar.Style = ProgressBarStyle.Marquee;
                    statusLabel.Text = "Loading vaults...";
                }

                var vaultsBySub = await task;

                // The subscription list may have been reloaded (Connect) while we were waiting.
                if (!ReferenceEquals(lbSubs.SelectedItem, subItem))
                {
                    progressBar.Style = ProgressBarStyle.Blocks;
                    statusLabel.Text = "";
                    return;
                }

                _allVaults = vaultsBySub.TryGetValue(subId, out var vaults) ? vaults : new();
                ApplyVaultFilter();

                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 100;
                statusLabel.Text = $"Loaded {_allVaults.Count} vaults.";

                // After Refresh: reselect the vault that was selected, if it is still there.
                string? restore = _restoreVaultName;
                _restoreVaultName = null;
                if (restore != null)
                {
                    var vault = lbVaults.Items.OfType<VaultItem>()
                        .FirstOrDefault(v => string.Equals(v.Name, restore, StringComparison.OrdinalIgnoreCase));
                    if (vault != null)
                        lbVaults.SelectedItem = vault;
                }
            }
            catch (Exception ex)
            {
                // Allow a retry (re-select the subscription or press F5 / Refresh).
                _vaultsTask = null;
                _lastSubIndex = -2;
                _restoreVaultName = null;
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 0;
                statusLabel.Text = "";
                MessageBox.Show($"Failed to load vaults:\n{ex.Message}\n\nDo you have the correct authorizations?", "Error");
            }
            finally
            {
                lbSubs.Enabled = true;
            }
        }

        private void ApplyVaultFilter()
        {
            var filtered = _allVaults
                .Where(v => v.Name.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            lbVaults.BeginUpdate();
            lbVaults.Items.Clear();
            lbVaults.Items.AddRange(filtered);
            lbVaults.EndUpdate();
        }

        private int _lastVaultIndex = -2;
        private async Task LoadSecretsAsync()
        {
            if (!EnsureConnected())
                return;

            if ((lbVaults.SelectedIndex == -1) || (lbVaults.SelectedIndex == _lastVaultIndex))
                return;
            _lastVaultIndex = lbVaults.SelectedIndex;

            if (lbVaults.SelectedItem is not VaultItem vault)
                return;

            ClearSecretView();

            if (_secretCache.TryGetValue(vault.VaultUri, out var cached))
            {
                AddSecrets(cached);
                statusLabel.Text = $"Loaded {cached.Count} secrets.";
                return;
            }

            // The vault list stays usable while loading: picking another vault cancels this load.
            var cts = new CancellationTokenSource();
            _secretsCts = cts;
            var loaded = new List<SecretListItem>();
            try
            {
                progressBar.Style = ProgressBarStyle.Marquee;
                statusLabel.Text = "Loading secrets...";

                await foreach (var page in _secretService!.GetSecretPagesAsync(vault.VaultUri, cts.Token))
                {
                    if (cts.IsCancellationRequested)
                        return;
                    loaded.AddRange(page);
                    AddSecrets(page);
                }

                _secretCache[vault.VaultUri] = loaded;
                statusLabel.Text = $"Loaded {loaded.Count} secrets.";
            }
            catch (OperationCanceledException) when (cts.IsCancellationRequested)
            {
                // Another vault was selected; nothing to do.
            }
            catch (Exception ex)
            {
                if (!cts.IsCancellationRequested)
                {
                    statusLabel.Text = "";
                    MessageBox.Show($"Failed to load secrets:\n{ex.Message}", "Error");
                }
            }
            finally
            {
                if (ReferenceEquals(_secretsCts, cts))
                    _secretsCts = null;

                // Only stop the progress bar if no newer secret load has taken over.
                if (_secretsCts == null)
                {
                    progressBar.Style = ProgressBarStyle.Blocks;
                    progressBar.Value = 100;
                }
                cts.Dispose();
            }
        }

        private bool MatchesSecretFilter(SecretListItem item) =>
            item.Name.Contains(txtSecretFilter.Text, StringComparison.OrdinalIgnoreCase);

        /// <summary>Appends newly loaded secrets, showing those that match the filter.</summary>
        private void AddSecrets(IReadOnlyList<SecretListItem> items)
        {
            _allSecrets.AddRange(items);
            UpdateSecretFilterVisibility();

            var matches = items.Where(MatchesSecretFilter).ToArray();
            if (matches.Length > 0)
            {
                lbSecrets.BeginUpdate();
                lbSecrets.Items.AddRange(matches);
                lbSecrets.EndUpdate();
            }

            // Select the first secret as soon as the first page arrives, so its value loads
            // while the remaining pages are still coming in. Never override the user's choice.
            if (lbSecrets.SelectedIndex == -1 && lbSecrets.Items.Count > 0)
                lbSecrets.SelectedIndex = 0;

            UpdateSecretCountLabel();
        }

        private void ApplySecretFilter()
        {
            var selected = lbSecrets.SelectedItem as SecretListItem;
            var matches = _allSecrets.Where(MatchesSecretFilter).ToArray();

            lbSecrets.BeginUpdate();
            lbSecrets.Items.Clear();
            lbSecrets.Items.AddRange(matches);
            lbSecrets.EndUpdate();

            // Keep the current secret (without re-fetching it) if it still matches.
            int index = selected == null ? -1 : lbSecrets.Items.IndexOf(selected);
            if (index >= 0)
            {
                _lastSecretIndex = index;
                lbSecrets.SelectedIndex = index;
            }
            else
            {
                ClearSecretValue();
            }

            UpdateSecretFilterVisibility();
            UpdateSecretCountLabel();
        }

        private void UpdateSecretFilterVisibility(bool force = false)
        {
            bool show = _allSecrets.Count >= SecretFilterThreshold || txtSecretFilter.TextLength > 0;
            if (!force && txtSecretFilter.Visible == show)
                return;

            mainTableLayoutPanel.SuspendLayout();
            txtSecretFilter.Visible = show;
            mainTableLayoutPanel.SetRow(lbSecrets, show ? 2 : 1);
            mainTableLayoutPanel.SetRowSpan(lbSecrets, show ? 1 : 2);
            mainTableLayoutPanel.ResumeLayout();
        }

        private void UpdateSecretCountLabel()
        {
            int total = _allSecrets.Count;
            int shown = lbSecrets.Items.Count;
            lblSecrets.Text = total == 0 ? "Secrets"
                : shown == total ? $"Secrets ({total})"
                : $"Secrets ({shown} of {total})";
        }

        /// <summary>Draws disabled secrets greyed out.</summary>
        private void lbSecrets_DrawItem(object? sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0 && e.Index < lbSecrets.Items.Count)
            {
                var item = lbSecrets.Items[e.Index];
                bool selected = (e.State & DrawItemState.Selected) != 0;
                bool disabled = item is SecretListItem { Enabled: false };
                Color color = disabled && !selected ? SystemColors.GrayText : e.ForeColor;

                TextRenderer.DrawText(e.Graphics, item.ToString(), e.Font, e.Bounds, color,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
            }
            e.DrawFocusRectangle();
        }

        private void txtSecretFilter_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && lbSecrets.Items.Count > 0)
            {
                lbSecrets.Focus();
                if (lbSecrets.SelectedIndex == -1)
                    lbSecrets.SelectedIndex = 0;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                txtSecretFilter.Clear();
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        public static int SecondsRemaining(int timestepSeconds = 30)
        {
            long unixSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return timestepSeconds - (int)(unixSeconds % timestepSeconds);
        }

        private int _lastSecretIndex = -2;
        private async Task GetSecretValueAsync()
        {
            if (!EnsureConnected())
                return;

            SetReveal(false);
            if ((lbSecrets.SelectedIndex == -1) || (lbSecrets.SelectedIndex == _lastSecretIndex))
                return;
            _lastSecretIndex = lbSecrets.SelectedIndex;

            if (lbVaults.SelectedItem is not VaultItem vault || lbSecrets.SelectedItem is not SecretListItem item)
                return;

            int request = ++_secretRequest;
            btnCopy.Enabled = false;
            txtValue.Clear();
            txtValue.PlaceholderText = "";
            pnlTotp.Visible = false;
            _secret = "";

            // Key Vault refuses to return the value of a disabled secret; don't bother asking.
            if (!item.Enabled)
            {
                txtValue.PlaceholderText = "This secret is disabled";
                statusLabel.Text = $"Secret '{item.Name}' is disabled.";
                return;
            }

            try
            {
                // Fetched once, asynchronously, so the UI stays responsive.
                string secretValue = await _secretService!.GetSecretValueAsync(vault.VaultUri, item.Name);

                // The user selected another secret (or vault) in the meantime.
                if (request != _secretRequest)
                    return;

                bool isTotp = string.Equals(item.Name, _settings.TOTP, StringComparison.OrdinalIgnoreCase);
                if (isTotp)
                {
                    _secret = secretValue;
                    txtTotp.Text = TotpGenerator.GenerateTotp(secretValue);
                }
                pnlTotp.Visible = isTotp;

                txtValue.Text = secretValue;
                btnCopy.Enabled = true;
            }
            catch (RequestFailedException rfe)
            {
                if (request == _secretRequest)
                    MessageBox.Show($"Azure request failed:\n{rfe.Message}", "Error");
            }
            catch (Exception ex)
            {
                if (request == _secretRequest)
                    MessageBox.Show($"Failed to get secret value:\n{ex.Message}", "Error");
            }
        }

        private async Task CopyTotpAsync()
        {
            try
            {
                Clipboard.SetText(txtTotp.Text ?? "");

                _ = ClearClipboardAfterDelayAsync(_settings.ClearClipboardAfterSeconds);

                btnCopyTotp.ImageIndex = 1;
                await Task.Delay(900);
                btnCopyTotp.ImageIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy to clipboard:\n{ex.Message}", "Error");
            }
        }

        private async Task CopyPasswordAsync()
        {
            try
            {
                Clipboard.SetText(txtValue.Text ?? "");

                _ = ClearClipboardAfterDelayAsync(_settings.ClearClipboardAfterSeconds);

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
            var selected = lbVaults.SelectedItem as VaultItem;

            ApplyVaultFilter();

            // Keep the current vault and its secrets if it still matches the filter.
            int index = selected == null ? -1 : lbVaults.Items.IndexOf(selected);
            if (index >= 0)
            {
                _lastVaultIndex = index;
                lbVaults.SelectedIndex = index;
            }
            else
            {
                _lastVaultIndex = -2;
                ClearSecretView();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.C))
            {
                if (pnlTotp.Visible)
                    _ = CopyTotpAsync();
                else
                    _ = CopyPasswordAsync();
                return true;
            }
            if (keyData == Keys.F5)
            {
                _ = RefreshVaultsAsync();
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
                _settings = SettingsManager.Load();
                SetCredential();
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
            if (e.KeyCode == Keys.Down && lbVaults.Items.Count > 0)
            {
                lbVaults.Focus();
                if (lbVaults.SelectedIndex == -1)
                    lbVaults.SelectedIndex = 0;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                txtFilter.Clear();
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            if (!HasValidSettings())
            {
                MessageBox.Show("Missing or incorrect authentication configuration.\nPlease check your Preferences.", "Info");
            }
            try
            {
                SetCredential();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialise authentication:\n{ex.Message}", "Error");
                return;
            }

            if (_settings.AutoConnect && HasValidSettings())
                await LoadSubsAsync();
        }
    }
}
