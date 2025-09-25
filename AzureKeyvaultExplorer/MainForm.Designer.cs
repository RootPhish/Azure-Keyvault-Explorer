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
            preferencesToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            connectToolStripMenuItem = new ToolStripMenuItem();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblValue = new Label();
            lblSecrets = new Label();
            lblKeyvaults = new Label();
            lbSubs = new ListBox();
            lbSecrets = new ListBox();
            panel2 = new Panel();
            btnCopy = new Button();
            btnEye = new Button();
            txtValue = new TextBox();
            lblSubscriptions = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            textBox1 = new TextBox();
            lbVaults = new ListBox();
            statusStrip1 = new StatusStrip();
            toolStripProgressBar1 = new ToolStripProgressBar();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            menuStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            statusStrip1.SuspendLayout();
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
            menuStrip1.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem, connectToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(961, 24);
            menuStrip1.TabIndex = 18;
            menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { preferencesToolStripMenuItem, toolStripSeparator2, exitToolStripMenuItem });
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
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(141, 6);
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
            connectToolStripMenuItem.Click += connectToolStripMenuItem_Click;
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
            tableLayoutPanel1.Controls.Add(lbSubs, 0, 1);
            tableLayoutPanel1.Controls.Add(lbSecrets, 2, 1);
            tableLayoutPanel1.Controls.Add(panel2, 3, 1);
            tableLayoutPanel1.Controls.Add(lblSubscriptions, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 1);
            tableLayoutPanel1.Controls.Add(statusStrip1, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 24);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
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
            // lbSubs
            // 
            lbSubs.Dock = DockStyle.Fill;
            lbSubs.FormattingEnabled = true;
            lbSubs.ItemHeight = 15;
            lbSubs.Location = new Point(3, 33);
            lbSubs.Name = "lbSubs";
            lbSubs.Size = new Size(214, 370);
            lbSubs.TabIndex = 24;
            // 
            // lbSecrets
            // 
            lbSecrets.Dock = DockStyle.Fill;
            lbSecrets.FormattingEnabled = true;
            lbSecrets.ItemHeight = 15;
            lbSecrets.Location = new Point(443, 33);
            lbSecrets.Name = "lbSecrets";
            lbSecrets.Size = new Size(214, 370);
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
            panel2.Size = new Size(295, 370);
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
            txtValue.BackColor = SystemColors.Window;
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
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(textBox1, 0, 0);
            tableLayoutPanel2.Controls.Add(lbVaults, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(223, 33);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(214, 370);
            tableLayoutPanel2.TabIndex = 31;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Left;
            textBox1.Location = new Point(3, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(208, 23);
            textBox1.TabIndex = 28;
            // 
            // lbVaults
            // 
            lbVaults.Dock = DockStyle.Fill;
            lbVaults.FormattingEnabled = true;
            lbVaults.ItemHeight = 15;
            lbVaults.Location = new Point(3, 33);
            lbVaults.Name = "lbVaults";
            lbVaults.Size = new Size(208, 334);
            lbVaults.TabIndex = 35;
            // 
            // statusStrip1
            // 
            tableLayoutPanel1.SetColumnSpan(statusStrip1, 4);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripProgressBar1, toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 406);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(961, 20);
            statusStrip1.TabIndex = 32;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            toolStripProgressBar1.Name = "toolStripProgressBar1";
            toolStripProgressBar1.Size = new Size(100, 14);
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(0, 15);
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
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
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
        private ToolStripSeparator toolStripSeparator2;
        private TableLayoutPanel tableLayoutPanel1;
        private ListBox lbSecrets;
        private Panel panel2;
        private Button btnCopy;
        private Button btnEye;
        private TextBox txtValue;
        private ListBox lbSubs;
        private Label lblValue;
        private Label lblSecrets;
        private Label lblKeyvaults;
        private Label lblSubscriptions;
        private ToolStripMenuItem connectToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel2;
        private ListBox lbVaults;
        private TextBox textBox1;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar toolStripProgressBar1;
        private ToolStripStatusLabel toolStripStatusLabel1;
    }
}
