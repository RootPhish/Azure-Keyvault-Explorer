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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            imageList1 = new ImageList(components);
            imageList2 = new ImageList(components);
            menuStrip1 = new MenuStrip();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            loadSubscriptionsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            preferencesToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblValue = new Label();
            lblSecrets = new Label();
            lblKeyvaults = new Label();
            textBox1 = new TextBox();
            lbVaults = new ListBox();
            lbSubs = new ListBox();
            lbSecrets = new ListBox();
            panel2 = new Panel();
            btnCopy = new Button();
            btnEye = new Button();
            txtValue = new TextBox();
            lblSubscriptions = new Label();
            menuStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "Ic_visibility_48px.svg.png");
            imageList1.Images.SetKeyName(1, "Ic_visibility_off_48px.svg.png");
            // 
            // imageList2
            // 
            imageList2.ColorDepth = ColorDepth.Depth32Bit;
            imageList2.ImageStream = (ImageListStreamer)resources.GetObject("imageList2.ImageStream");
            imageList2.TransparentColor = Color.Transparent;
            imageList2.Images.SetKeyName(0, "Ic_content_copy_48px.svg.png");
            imageList2.Images.SetKeyName(1, "Ic_done_48px.svg.png");
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(961, 24);
            menuStrip1.TabIndex = 18;
            menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadSubscriptionsToolStripMenuItem, toolStripSeparator1, preferencesToolStripMenuItem, toolStripSeparator2, exitToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(37, 20);
            settingsToolStripMenuItem.Text = "File";
            // 
            // loadSubscriptionsToolStripMenuItem
            // 
            loadSubscriptionsToolStripMenuItem.Name = "loadSubscriptionsToolStripMenuItem";
            loadSubscriptionsToolStripMenuItem.Size = new Size(180, 22);
            loadSubscriptionsToolStripMenuItem.Text = "Load Subscriptions";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // preferencesToolStripMenuItem
            // 
            preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            preferencesToolStripMenuItem.Size = new Size(180, 22);
            preferencesToolStripMenuItem.Text = "Preferences...";
            preferencesToolStripMenuItem.Click += preferencesToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(180, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            tableLayoutPanel1.Controls.Add(lblValue, 3, 0);
            tableLayoutPanel1.Controls.Add(lblSecrets, 2, 0);
            tableLayoutPanel1.Controls.Add(lblKeyvaults, 1, 0);
            tableLayoutPanel1.Controls.Add(textBox1, 1, 1);
            tableLayoutPanel1.Controls.Add(lbVaults, 1, 2);
            tableLayoutPanel1.Controls.Add(lbSubs, 0, 1);
            tableLayoutPanel1.Controls.Add(lbSecrets, 2, 1);
            tableLayoutPanel1.Controls.Add(panel2, 3, 1);
            tableLayoutPanel1.Controls.Add(lblSubscriptions, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 24);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(961, 426);
            tableLayoutPanel1.TabIndex = 19;
            // 
            // lblValue
            // 
            lblValue.AutoSize = true;
            lblValue.Dock = DockStyle.Fill;
            lblValue.Location = new Point(663, 0);
            lblValue.Name = "lblValue";
            lblValue.Size = new Size(295, 30);
            lblValue.TabIndex = 30;
            lblValue.Text = "Value";
            lblValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSecrets
            // 
            lblSecrets.AutoSize = true;
            lblSecrets.Dock = DockStyle.Fill;
            lblSecrets.Location = new Point(443, 0);
            lblSecrets.Name = "lblSecrets";
            lblSecrets.Size = new Size(214, 30);
            lblSecrets.TabIndex = 29;
            lblSecrets.Text = "Secrets";
            lblSecrets.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblKeyvaults
            // 
            lblKeyvaults.AutoSize = true;
            lblKeyvaults.Dock = DockStyle.Fill;
            lblKeyvaults.Location = new Point(223, 0);
            lblKeyvaults.Name = "lblKeyvaults";
            lblKeyvaults.Size = new Size(214, 30);
            lblKeyvaults.TabIndex = 28;
            lblKeyvaults.Text = "Key Vaults";
            lblKeyvaults.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Top;
            textBox1.Location = new Point(223, 33);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(214, 23);
            textBox1.TabIndex = 26;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // lbVaults
            // 
            lbVaults.Dock = DockStyle.Fill;
            lbVaults.FormattingEnabled = true;
            lbVaults.ItemHeight = 15;
            lbVaults.Location = new Point(223, 63);
            lbVaults.Name = "lbVaults";
            lbVaults.Size = new Size(214, 360);
            lbVaults.TabIndex = 25;
            // 
            // lbSubs
            // 
            lbSubs.Dock = DockStyle.Fill;
            lbSubs.FormattingEnabled = true;
            lbSubs.ItemHeight = 15;
            lbSubs.Location = new Point(3, 33);
            lbSubs.Name = "lbSubs";
            tableLayoutPanel1.SetRowSpan(lbSubs, 2);
            lbSubs.Size = new Size(214, 390);
            lbSubs.TabIndex = 24;
            // 
            // lbSecrets
            // 
            lbSecrets.Dock = DockStyle.Fill;
            lbSecrets.FormattingEnabled = true;
            lbSecrets.ItemHeight = 15;
            lbSecrets.Location = new Point(443, 33);
            lbSecrets.Name = "lbSecrets";
            tableLayoutPanel1.SetRowSpan(lbSecrets, 2);
            lbSecrets.Size = new Size(214, 390);
            lbSecrets.TabIndex = 13;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnCopy);
            panel2.Controls.Add(btnEye);
            panel2.Controls.Add(txtValue);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(663, 33);
            panel2.Name = "panel2";
            tableLayoutPanel1.SetRowSpan(panel2, 2);
            panel2.Size = new Size(295, 390);
            panel2.TabIndex = 23;
            // 
            // btnCopy
            // 
            btnCopy.BackgroundImageLayout = ImageLayout.Zoom;
            btnCopy.FlatAppearance.BorderSize = 0;
            btnCopy.FlatStyle = FlatStyle.Flat;
            btnCopy.ImageIndex = 0;
            btnCopy.ImageList = imageList2;
            btnCopy.Location = new Point(253, 26);
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
            btnEye.ImageList = imageList1;
            btnEye.Location = new Point(228, 26);
            btnEye.Name = "btnEye";
            btnEye.Size = new Size(20, 20);
            btnEye.TabIndex = 22;
            btnEye.TabStop = false;
            btnEye.UseVisualStyleBackColor = true;
            // 
            // txtValue
            // 
            txtValue.Location = new Point(26, 23);
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
            lblSubscriptions.Location = new Point(3, 0);
            lblSubscriptions.Name = "lblSubscriptions";
            lblSubscriptions.Size = new Size(214, 30);
            lblSubscriptions.TabIndex = 27;
            lblSubscriptions.Text = "Subscriptions";
            lblSubscriptions.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(961, 450);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "Azure Keyvault Explorer";
            Load += MainForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ImageList imageList1;
        private ImageList imageList2;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem preferencesToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem loadSubscriptionsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private TableLayoutPanel tableLayoutPanel1;
        private ListBox lbSecrets;
        private Panel panel2;
        private Button btnCopy;
        private Button btnEye;
        private TextBox txtValue;
        private TextBox textBox1;
        private ListBox lbVaults;
        private ListBox lbSubs;
        private Label lblValue;
        private Label lblSecrets;
        private Label lblKeyvaults;
        private Label lblSubscriptions;
    }
}
