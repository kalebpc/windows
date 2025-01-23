namespace ScreenSaverFallingCode
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
            this.screenSaverTitle = new System.Windows.Forms.Label();
            this.screenSaverAuthor = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RainTrailComboBox = new System.Windows.Forms.ComboBox();
            this.RaindropComboBox = new System.Windows.Forms.ComboBox();
            this.BackgroundComboBox = new System.Windows.Forms.ComboBox();
            this.PreviewPictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FontBrowseButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.FontLabel = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPictureBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // screenSaverTitle
            // 
            this.screenSaverTitle.AutoSize = true;
            this.screenSaverTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenSaverTitle.Location = new System.Drawing.Point(13, 9);
            this.screenSaverTitle.Name = "screenSaverTitle";
            this.screenSaverTitle.Size = new System.Drawing.Size(124, 13);
            this.screenSaverTitle.TabIndex = 1;
            this.screenSaverTitle.Text = "ScreenSaverFallingCode";
            // 
            // screenSaverAuthor
            // 
            this.screenSaverAuthor.AutoSize = true;
            this.screenSaverAuthor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenSaverAuthor.Location = new System.Drawing.Point(142, 9);
            this.screenSaverAuthor.Name = "screenSaverAuthor";
            this.screenSaverAuthor.Size = new System.Drawing.Size(147, 13);
            this.screenSaverAuthor.TabIndex = 2;
            this.screenSaverAuthor.Text = "© 2025 (github.com/kalebpc)";
            // 
            // okButton
            // 
            this.okButton.AutoSize = true;
            this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.Location = new System.Drawing.Point(153, 246);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.AutoSize = true;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(234, 246);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Raindrop Color";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Background Color";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Rain Trail Color";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.RaindropComboBox);
            this.groupBox2.Controls.Add(this.RainTrailComboBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.BackgroundComboBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(14, 25);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(162, 151);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Color";
            // 
            // RainTrailComboBox
            // 
            this.RainTrailComboBox.FormattingEnabled = true;
            this.RainTrailComboBox.Location = new System.Drawing.Point(8, 75);
            this.RainTrailComboBox.Name = "RainTrailComboBox";
            this.RainTrailComboBox.Size = new System.Drawing.Size(145, 21);
            this.RainTrailComboBox.TabIndex = 31;
            this.RainTrailComboBox.SelectedIndexChanged += new System.EventHandler(this.RainTrailComboBox_SelectedIndexChanged);
            // 
            // RaindropComboBox
            // 
            this.RaindropComboBox.FormattingEnabled = true;
            this.RaindropComboBox.Location = new System.Drawing.Point(8, 115);
            this.RaindropComboBox.Name = "RaindropComboBox";
            this.RaindropComboBox.Size = new System.Drawing.Size(145, 21);
            this.RaindropComboBox.TabIndex = 30;
            this.RaindropComboBox.SelectedIndexChanged += new System.EventHandler(this.RaindropComboBox_SelectedIndexChanged);
            // 
            // BackgroundComboBox
            // 
            this.BackgroundComboBox.FormattingEnabled = true;
            this.BackgroundComboBox.Location = new System.Drawing.Point(8, 35);
            this.BackgroundComboBox.Name = "BackgroundComboBox";
            this.BackgroundComboBox.Size = new System.Drawing.Size(145, 21);
            this.BackgroundComboBox.TabIndex = 29;
            this.BackgroundComboBox.SelectedIndexChanged += new System.EventHandler(this.BackgroundComboBox_SelectedIndexChanged);
            // 
            // PreviewPictureBox
            // 
            this.PreviewPictureBox.Location = new System.Drawing.Point(6, 15);
            this.PreviewPictureBox.Name = "PreviewPictureBox";
            this.PreviewPictureBox.Size = new System.Drawing.Size(115, 121);
            this.PreviewPictureBox.TabIndex = 26;
            this.PreviewPictureBox.TabStop = false;
            this.PreviewPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPreviewPaint);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.FontBrowseButton);
            this.groupBox1.Location = new System.Drawing.Point(14, 182);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(295, 58);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Font";
            // 
            // FontBrowseButton
            // 
            this.FontBrowseButton.Location = new System.Drawing.Point(214, 19);
            this.FontBrowseButton.Name = "FontBrowseButton";
            this.FontBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.FontBrowseButton.TabIndex = 0;
            this.FontBrowseButton.Text = "Browse";
            this.FontBrowseButton.UseVisualStyleBackColor = true;
            this.FontBrowseButton.Click += new System.EventHandler(this.FontBrowseButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.PreviewPictureBox);
            this.groupBox3.Location = new System.Drawing.Point(182, 25);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(127, 151);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sample";
            // 
            // FontLabel
            // 
            this.FontLabel.AutoSize = true;
            this.FontLabel.BackColor = System.Drawing.SystemColors.Control;
            this.FontLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FontLabel.Location = new System.Drawing.Point(6, 12);
            this.FontLabel.Name = "FontLabel";
            this.FontLabel.Size = new System.Drawing.Size(29, 13);
            this.FontLabel.TabIndex = 1;
            this.FontLabel.Text = "label";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.FontLabel);
            this.groupBox4.Location = new System.Drawing.Point(8, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 31);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(327, 283);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.screenSaverAuthor);
            this.Controls.Add(this.screenSaverTitle);
            this.Name = "SettingsForm";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 15, 15);
            this.Text = "Settings";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPictureBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label screenSaverTitle;
        private System.Windows.Forms.Label screenSaverAuthor;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox PreviewPictureBox;
        private System.Windows.Forms.ComboBox RainTrailComboBox;
        private System.Windows.Forms.ComboBox RaindropComboBox;
        private System.Windows.Forms.ComboBox BackgroundComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button FontBrowseButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label FontLabel;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}