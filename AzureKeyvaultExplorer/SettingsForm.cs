namespace AzureKeyvaultExplorer
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            pgSettings.SelectedObject = new SettingsWrapper();
        }
    }
}
