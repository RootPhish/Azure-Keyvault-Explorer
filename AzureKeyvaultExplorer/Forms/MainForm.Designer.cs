namespace AzureKeyvaultExplorer
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            mainMenu = new MenuStrip();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            preferencesToolStripMenuItem = new ToolStripMenuItem();
            menuSeparator = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            connectToolStripMenuItem = new ToolStripMenuItem();
            mainTableLayoutPanel = new TableLayoutPanel();
            lbVaults = new ListBox();
            txtFilter = new TextBox();
            lblValue = new Label();
            lblSecrets = new Label();
            lblKeyvaults = new Label();
            lbSubs = new ListBox();
            lbSecrets = new ListBox();
            panelValue = new Panel();
            btnCopy = new Button();
            btnEye = new Button();
            txtValue = new TextBox();
            lblSubscriptions = new Label();
            statusBar = new StatusStrip();
            progressBar = new ToolStripProgressBar();
            statusLabel = new ToolStripStatusLabel();
            mainMenu.SuspendLayout();
            mainTableLayoutPanel.SuspendLayout();
            panelValue.SuspendLayout();
            statusBar.SuspendLayout();
            SuspendLayout();
            // 
            // mainMenu
            // 
            mainMenu.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem, connectToolStripMenuItem });
            mainMenu.Location = new Point(0, 0);
            mainMenu.Name = "mainMenu";
            mainMenu.Size = new Size(961, 24);
            mainMenu.TabIndex = 18;
            mainMenu.Text = "mainMenu";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { preferencesToolStripMenuItem, menuSeparator, exitToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(37, 20);
            settingsToolStripMenuItem.Text = "File";
            // 
            // preferencesToolStripMenuItem
            // 
            preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            preferencesToolStripMenuItem.Size = new Size(144, 22);
            preferencesToolStripMenuItem.Text = "Preferences...";
            preferencesToolStripMenuItem.Click += preferencesToolStripMenuItem_Click;
            // 
            // menuSeparator
            // 
            menuSeparator.Name = "menuSeparator";
            menuSeparator.Size = new Size(141, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(144, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // connectToolStripMenuItem
            // 
            connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            connectToolStripMenuItem.Size = new Size(64, 20);
            connectToolStripMenuItem.Text = "Connect";
            // 
            // mainTableLayoutPanel
            // 
            mainTableLayoutPanel.ColumnCount = 4;
            mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260F));
            mainTableLayoutPanel.Controls.Add(lbVaults, 1, 2);
            mainTableLayoutPanel.Controls.Add(txtFilter, 1, 1);
            mainTableLayoutPanel.Controls.Add(lblValue, 3, 0);
            mainTableLayoutPanel.Controls.Add(lblSecrets, 2, 0);
            mainTableLayoutPanel.Controls.Add(lblKeyvaults, 1, 0);
            mainTableLayoutPanel.Controls.Add(lbSubs, 0, 1);
            mainTableLayoutPanel.Controls.Add(lbSecrets, 2, 1);
            mainTableLayoutPanel.Controls.Add(panelValue, 3, 1);
            mainTableLayoutPanel.Controls.Add(lblSubscriptions, 0, 0);
            mainTableLayoutPanel.Controls.Add(statusBar, 0, 3);
            mainTableLayoutPanel.Dock = DockStyle.Fill;
            mainTableLayoutPanel.Location = new Point(0, 24);
            mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            mainTableLayoutPanel.Padding = new Padding(3);
            mainTableLayoutPanel.RowCount = 4;
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            mainTableLayoutPanel.Size = new Size(961, 426);
            mainTableLayoutPanel.TabIndex = 19;
            // 
            // lbVaults
            // 
            lbVaults.BorderStyle = BorderStyle.FixedSingle;
            lbVaults.Dock = DockStyle.Fill;
            lbVaults.FormattingEnabled = true;
            lbVaults.IntegralHeight = false;
            lbVaults.ItemHeight = 15;
            lbVaults.Location = new Point(237, 66);
            lbVaults.Name = "lbVaults";
            lbVaults.Size = new Size(225, 334);
            lbVaults.TabIndex = 36;
            // 
            // txtFilter
            // 
            txtFilter.BorderStyle = BorderStyle.FixedSingle;
            txtFilter.Dock = DockStyle.Fill;
            txtFilter.Location = new Point(237, 36);
            txtFilter.Name = "txtFilter";
            txtFilter.Size = new Size(225, 23);
            txtFilter.TabIndex = 33;
            txtFilter.TextChanged += txtFilter_TextChanged;
            txtFilter.KeyDown += txtFilter_KeyDown;
            // 
            // lblValue
            // 
            lblValue.AutoSize = true;
            lblValue.Dock = DockStyle.Fill;
            lblValue.Location = new Point(699, 3);
            lblValue.Name = "lblValue";
            lblValue.Size = new Size(256, 30);
            lblValue.TabIndex = 30;
            lblValue.Text = "Value";
            lblValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSecrets
            // 
            lblSecrets.AutoSize = true;
            lblSecrets.Dock = DockStyle.Fill;
            lblSecrets.Location = new Point(468, 3);
            lblSecrets.Name = "lblSecrets";
            lblSecrets.Size = new Size(225, 30);
            lblSecrets.TabIndex = 29;
            lblSecrets.Text = "Secrets";
            lblSecrets.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblKeyvaults
            // 
            lblKeyvaults.AutoSize = true;
            lblKeyvaults.Dock = DockStyle.Fill;
            lblKeyvaults.Location = new Point(237, 3);
            lblKeyvaults.Name = "lblKeyvaults";
            lblKeyvaults.Size = new Size(225, 30);
            lblKeyvaults.TabIndex = 28;
            lblKeyvaults.Text = "Key Vaults";
            lblKeyvaults.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbSubs
            // 
            lbSubs.BorderStyle = BorderStyle.FixedSingle;
            lbSubs.Dock = DockStyle.Fill;
            lbSubs.FormattingEnabled = true;
            lbSubs.IntegralHeight = false;
            lbSubs.ItemHeight = 15;
            lbSubs.Location = new Point(6, 36);
            lbSubs.Name = "lbSubs";
            mainTableLayoutPanel.SetRowSpan(lbSubs, 2);
            lbSubs.Size = new Size(225, 364);
            lbSubs.TabIndex = 24;
            // 
            // lbSecrets
            // 
            lbSecrets.BorderStyle = BorderStyle.FixedSingle;
            lbSecrets.Dock = DockStyle.Fill;
            lbSecrets.FormattingEnabled = true;
            lbSecrets.IntegralHeight = false;
            lbSecrets.ItemHeight = 15;
            lbSecrets.Location = new Point(468, 36);
            lbSecrets.Name = "lbSecrets";
            mainTableLayoutPanel.SetRowSpan(lbSecrets, 2);
            lbSecrets.Size = new Size(225, 364);
            lbSecrets.TabIndex = 13;
            // 
            // panelValue
            // 
            panelValue.Controls.Add(btnCopy);
            panelValue.Controls.Add(btnEye);
            panelValue.Controls.Add(txtValue);
            panelValue.Dock = DockStyle.Fill;
            panelValue.Location = new Point(699, 36);
            panelValue.Name = "panelValue";
            mainTableLayoutPanel.SetRowSpan(panelValue, 2);
            panelValue.Size = new Size(256, 364);
            panelValue.TabIndex = 23;
            // 
            // btnCopy
            // 
            btnCopy.BackgroundImageLayout = ImageLayout.Zoom;
            btnCopy.Enabled = false;
            btnCopy.FlatAppearance.BorderSize = 0;
            btnCopy.FlatStyle = FlatStyle.Flat;
            btnCopy.ImageIndex = 0;
            btnCopy.Location = new Point(227, 0);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(20, 20);
            btnCopy.TabIndex = 23;
            btnCopy.TabStop = false;
            btnCopy.UseVisualStyleBackColor = true;
            // 
            // btnEye
            // 
            btnEye.BackgroundImageLayout = ImageLayout.Zoom;
            btnEye.FlatAppearance.BorderSize = 0;
            btnEye.FlatStyle = FlatStyle.Flat;
            btnEye.ImageIndex = 1;
            btnEye.Location = new Point(202, 0);
            btnEye.Name = "btnEye";
            btnEye.Size = new Size(20, 20);
            btnEye.TabIndex = 22;
            btnEye.TabStop = false;
            btnEye.UseVisualStyleBackColor = true;
            // 
            // txtValue
            // 
            txtValue.BackColor = SystemColors.Window;
            txtValue.BorderStyle = BorderStyle.FixedSingle;
            txtValue.Location = new Point(0, 0);
            txtValue.Name = "txtValue";
            txtValue.ReadOnly = true;
            txtValue.Size = new Size(196, 23);
            txtValue.TabIndex = 21;
            txtValue.UseSystemPasswordChar = true;
            // 
            // lblSubscriptions
            // 
            lblSubscriptions.AutoSize = true;
            lblSubscriptions.Dock = DockStyle.Fill;
            lblSubscriptions.Location = new Point(6, 3);
            lblSubscriptions.Name = "lblSubscriptions";
            lblSubscriptions.Size = new Size(225, 30);
            lblSubscriptions.TabIndex = 27;
            lblSubscriptions.Text = "Subscriptions";
            lblSubscriptions.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // statusBar
            // 
            mainTableLayoutPanel.SetColumnSpan(statusBar, 4);
            statusBar.Items.AddRange(new ToolStripItem[] { progressBar, statusLabel });
            statusBar.Location = new Point(3, 403);
            statusBar.Name = "statusBar";
            statusBar.Size = new Size(955, 20);
            statusBar.TabIndex = 32;
            statusBar.Text = "statusStrip1";
            // 
            // progressBar
            // 
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(100, 14);
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(0, 15);
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(961, 450);
            Controls.Add(mainTableLayoutPanel);
            Controls.Add(mainMenu);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = mainMenu;
            Name = "MainForm";
            Text = "Azure Keyvault Explorer";
            Shown += MainForm_Shown;
            mainMenu.ResumeLayout(false);
            mainMenu.PerformLayout();
            mainTableLayoutPanel.ResumeLayout(false);
            mainTableLayoutPanel.PerformLayout();
            panelValue.ResumeLayout(false);
            panelValue.PerformLayout();
            statusBar.ResumeLayout(false);
            statusBar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip mainMenu;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem preferencesToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripSeparator menuSeparator;
        private TableLayoutPanel mainTableLayoutPanel;
        private ListBox lbSecrets;
        private Panel panelValue;
        private Button btnCopy;
        private Button btnEye;
        private TextBox txtValue;
        private ListBox lbSubs;
        private Label lblValue;
        private Label lblSecrets;
        private Label lblKeyvaults;
        private Label lblSubscriptions;
        private ToolStripMenuItem connectToolStripMenuItem;
        private StatusStrip statusBar;
        private ToolStripProgressBar progressBar;
        private ToolStripStatusLabel statusLabel;
        private ListBox lbVaults;
        private TextBox txtFilter;
    }
}
