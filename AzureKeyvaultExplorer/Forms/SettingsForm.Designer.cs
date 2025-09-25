namespace AzureKeyvaultExplorer
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            button1 = new Button();
            btnOK = new Button();
            pgSettings = new PropertyGrid();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(button1, 1, 1);
            tableLayoutPanel1.Controls.Add(btnOK, 0, 1);
            tableLayoutPanel1.Controls.Add(pgSettings, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.Size = new Size(514, 391);
            tableLayoutPanel1.TabIndex = 7;
            // 
            // button1
            // 
            button1.DialogResult = DialogResult.Cancel;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(260, 352);
            button1.Margin = new Padding(3, 6, 3, 3);
            button1.Name = "button1";
            button1.Size = new Size(75, 24);
            button1.TabIndex = 8;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Location = new Point(179, 352);
            btnOK.Margin = new Padding(3, 6, 3, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 24);
            btnOK.TabIndex = 7;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // pgSettings
            // 
            tableLayoutPanel1.SetColumnSpan(pgSettings, 2);
            pgSettings.Dock = DockStyle.Fill;
            pgSettings.Location = new Point(10, 10);
            pgSettings.Margin = new Padding(10);
            pgSettings.Name = "pgSettings";
            pgSettings.Size = new Size(494, 326);
            pgSettings.TabIndex = 6;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(514, 391);
            Controls.Add(tableLayoutPanel1);
            Name = "SettingsForm";
            Text = "Settings";
            Load += SettingsForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button button1;
        private Button btnOK;
        private PropertyGrid pgSettings;
    }
}