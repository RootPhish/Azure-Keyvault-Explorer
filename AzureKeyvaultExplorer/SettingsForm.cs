namespace AzureKeyvaultExplorer
{
    public partial class SettingsForm : Form
    {
        private UserSettings settings;

        public SettingsForm()
        {
            InitializeComponent();


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            settings.Save();
            this.Close();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            settings = UserSettings.Load();
            pgSettings.SelectedObject = settings;
        }
    }
}
