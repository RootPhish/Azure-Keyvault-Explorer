using AzureKeyvaultExplorer.Classes;

namespace AzureKeyvaultExplorer
{
    public partial class SettingsForm : Form
    {
        private AppSettings _settings = SettingsManager.Load();

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SettingsManager.Save(_settings);
            this.Close();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            pgSettings.SelectedObject = new SettingsWrapper(_settings);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.Close();
        }
    }
}
