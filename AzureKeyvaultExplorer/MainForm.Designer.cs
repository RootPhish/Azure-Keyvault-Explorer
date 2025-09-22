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
            btnLoadSubs = new Button();
            lbVaults = new ListBox();
            lbSecrets = new ListBox();
            txtValue = new TextBox();
            lbSubs = new ListBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            btnEye = new Button();
            imageList1 = new ImageList(components);
            btnCopy = new Button();
            imageList2 = new ImageList(components);
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // btnLoadSubs
            // 
            btnLoadSubs.Location = new Point(12, 344);
            btnLoadSubs.Name = "btnLoadSubs";
            btnLoadSubs.Size = new Size(121, 23);
            btnLoadSubs.TabIndex = 3;
            btnLoadSubs.Text = "Load Subscriptions";
            btnLoadSubs.UseVisualStyleBackColor = true;
            // 
            // lbVaults
            // 
            lbVaults.FormattingEnabled = true;
            lbVaults.ItemHeight = 15;
            lbVaults.Location = new Point(208, 93);
            lbVaults.Name = "lbVaults";
            lbVaults.Size = new Size(190, 244);
            lbVaults.TabIndex = 4;
            // 
            // lbSecrets
            // 
            lbSecrets.FormattingEnabled = true;
            lbSecrets.ItemHeight = 15;
            lbSecrets.Location = new Point(404, 93);
            lbSecrets.Name = "lbSecrets";
            lbSecrets.Size = new Size(190, 244);
            lbSecrets.TabIndex = 5;
            // 
            // txtValue
            // 
            txtValue.Location = new Point(600, 93);
            txtValue.Name = "txtValue";
            txtValue.Size = new Size(219, 23);
            txtValue.TabIndex = 8;
            txtValue.UseSystemPasswordChar = true;
            txtValue.ReadOnly = true;
            // 
            // lbSubs
            // 
            lbSubs.FormattingEnabled = true;
            lbSubs.ItemHeight = 15;
            lbSubs.Location = new Point(12, 94);
            lbSubs.Name = "lbSubs";
            lbSubs.Size = new Size(190, 244);
            lbSubs.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 76);
            label1.Name = "label1";
            label1.Size = new Size(78, 15);
            label1.TabIndex = 11;
            label1.Text = "Subscriptions";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(208, 50);
            label2.Name = "label2";
            label2.Size = new Size(60, 15);
            label2.TabIndex = 12;
            label2.Text = "Key Vaults";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(404, 75);
            label3.Name = "label3";
            label3.Size = new Size(44, 15);
            label3.TabIndex = 13;
            label3.Text = "Secrets";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(600, 75);
            label4.Name = "label4";
            label4.Size = new Size(35, 15);
            label4.TabIndex = 14;
            label4.Text = "Value";
            // 
            // btnEye
            // 
            btnEye.BackgroundImageLayout = ImageLayout.Zoom;
            btnEye.FlatAppearance.BorderSize = 0;
            btnEye.FlatStyle = FlatStyle.Flat;
            btnEye.ImageIndex = 1;
            btnEye.ImageList = imageList1;
            btnEye.Location = new Point(820, 93);
            btnEye.Name = "btnEye";
            btnEye.Size = new Size(20, 20);
            btnEye.TabIndex = 15;
            btnEye.TabStop = false;
            btnEye.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "Ic_visibility_48px.svg.png");
            imageList1.Images.SetKeyName(1, "Ic_visibility_off_48px.svg.png");
            // 
            // btnCopy
            // 
            btnCopy.BackgroundImageLayout = ImageLayout.Zoom;
            btnCopy.FlatAppearance.BorderSize = 0;
            btnCopy.FlatStyle = FlatStyle.Flat;
            btnCopy.ImageIndex = 0;
            btnCopy.ImageList = imageList2;
            btnCopy.Location = new Point(845, 93);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(20, 20);
            btnCopy.TabIndex = 16;
            btnCopy.TabStop = false;
            btnCopy.UseVisualStyleBackColor = true;
            // 
            // imageList2
            // 
            imageList2.ColorDepth = ColorDepth.Depth32Bit;
            imageList2.ImageStream = (ImageListStreamer)resources.GetObject("imageList2.ImageStream");
            imageList2.TransparentColor = Color.Transparent;
            imageList2.Images.SetKeyName(0, "Ic_content_copy_48px.svg.png");
            imageList2.Images.SetKeyName(1, "Ic_done_48px.svg.png");
            // 
            // textBox1
            // 
            textBox1.Location = new Point(208, 68);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(190, 23);
            textBox1.TabIndex = 17;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(961, 450);
            Controls.Add(textBox1);
            Controls.Add(btnCopy);
            Controls.Add(btnEye);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lbSubs);
            Controls.Add(txtValue);
            Controls.Add(lbSecrets);
            Controls.Add(lbVaults);
            Controls.Add(btnLoadSubs);
            Name = "MainForm";
            Text = "Azure Keyvault Explorer";
            KeyDown += MainForm_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnLoadSubs;
        private ListBox lbVaults;
        private ListBox lbSecrets;
        private TextBox txtValue;
        private ListBox lbSubs;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button btnEye;
        private Button btnCopy;
        private ImageList imageList1;
        private ImageList imageList2;
        private TextBox textBox1;
    }
}
