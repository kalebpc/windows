namespace ScreenSaverGameofLife
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
            this.label1 = new System.Windows.Forms.Label();
            this.OutlineCheckBox = new System.Windows.Forms.CheckBox();
            this.ShapeComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.BorderNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ShapeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ShapeColorBox = new System.Windows.Forms.PictureBox();
            this.BackgroundColorBox = new System.Windows.Forms.PictureBox();
            this.ShapeColorComboBox = new System.Windows.Forms.ComboBox();
            this.BackgroundColorComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AngleGroupBox = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.EndAngleTrackBar = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.StartAngleTrackBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.BorderNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShapeNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShapeColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundColorBox)).BeginInit();
            this.AngleGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EndAngleTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartAngleTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // screenSaverTitle
            // 
            this.screenSaverTitle.AutoSize = true;
            this.screenSaverTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenSaverTitle.Location = new System.Drawing.Point(13, 9);
            this.screenSaverTitle.Name = "screenSaverTitle";
            this.screenSaverTitle.Size = new System.Drawing.Size(123, 13);
            this.screenSaverTitle.TabIndex = 1;
            this.screenSaverTitle.Text = "ScreenSaverGameofLife";
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
            this.okButton.Location = new System.Drawing.Point(236, 45);
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
            this.cancelButton.Location = new System.Drawing.Point(236, 74);
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
            this.label1.Location = new System.Drawing.Point(6, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Shape Color";
            // 
            // OutlineCheckBox
            // 
            this.OutlineCheckBox.AutoSize = true;
            this.OutlineCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OutlineCheckBox.Location = new System.Drawing.Point(16, 249);
            this.OutlineCheckBox.Name = "OutlineCheckBox";
            this.OutlineCheckBox.Size = new System.Drawing.Size(128, 17);
            this.OutlineCheckBox.TabIndex = 13;
            this.OutlineCheckBox.Text = "Borders Only (Outline)";
            this.OutlineCheckBox.UseVisualStyleBackColor = true;
            // 
            // ShapeComboBox
            // 
            this.ShapeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShapeComboBox.FormattingEnabled = true;
            this.ShapeComboBox.Items.AddRange(new object[] {
            "Square",
            "Circle",
            "Fish Scales",
            "Arc",
            "Stingray"});
            this.ShapeComboBox.Location = new System.Drawing.Point(16, 285);
            this.ShapeComboBox.Name = "ShapeComboBox";
            this.ShapeComboBox.Size = new System.Drawing.Size(108, 21);
            this.ShapeComboBox.TabIndex = 14;
            this.ShapeComboBox.SelectedIndexChanged += new System.EventHandler(this.ShapeComboBox_SelectedIndexChanged);
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Shape Size (pixels) :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Border Size (pixels) :";
            // 
            // BorderNumericUpDown
            // 
            this.BorderNumericUpDown.Location = new System.Drawing.Point(113, 48);
            this.BorderNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.BorderNumericUpDown.Name = "BorderNumericUpDown";
            this.BorderNumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.BorderNumericUpDown.TabIndex = 25;
            this.BorderNumericUpDown.ValueChanged += new System.EventHandler(this.BorderNumericUpDown_ValueChanged);
            // 
            // ShapeNumericUpDown
            // 
            this.ShapeNumericUpDown.AutoSize = true;
            this.ShapeNumericUpDown.Location = new System.Drawing.Point(114, 16);
            this.ShapeNumericUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.ShapeNumericUpDown.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.ShapeNumericUpDown.Name = "ShapeNumericUpDown";
            this.ShapeNumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.ShapeNumericUpDown.TabIndex = 26;
            this.ShapeNumericUpDown.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.ShapeNumericUpDown.ValueChanged += new System.EventHandler(this.ShapeNumericUpDown_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.BorderNumericUpDown);
            this.groupBox1.Controls.Add(this.ShapeNumericUpDown);
            this.groupBox1.Location = new System.Drawing.Point(16, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 78);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Size";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ShapeColorBox);
            this.groupBox2.Controls.Add(this.BackgroundColorBox);
            this.groupBox2.Controls.Add(this.ShapeColorComboBox);
            this.groupBox2.Controls.Add(this.BackgroundColorComboBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(16, 129);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(191, 114);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Color";
            // 
            // ShapeColorBox
            // 
            this.ShapeColorBox.Location = new System.Drawing.Point(138, 60);
            this.ShapeColorBox.Name = "ShapeColorBox";
            this.ShapeColorBox.Size = new System.Drawing.Size(39, 37);
            this.ShapeColorBox.TabIndex = 23;
            this.ShapeColorBox.TabStop = false;
            // 
            // BackgroundColorBox
            // 
            this.BackgroundColorBox.Location = new System.Drawing.Point(138, 20);
            this.BackgroundColorBox.Name = "BackgroundColorBox";
            this.BackgroundColorBox.Size = new System.Drawing.Size(39, 37);
            this.BackgroundColorBox.TabIndex = 22;
            this.BackgroundColorBox.TabStop = false;
            // 
            // ShapeColorComboBox
            // 
            this.ShapeColorComboBox.FormattingEnabled = true;
            this.ShapeColorComboBox.Location = new System.Drawing.Point(9, 76);
            this.ShapeColorComboBox.Name = "ShapeColorComboBox";
            this.ShapeColorComboBox.Size = new System.Drawing.Size(121, 21);
            this.ShapeColorComboBox.TabIndex = 21;
            this.ShapeColorComboBox.SelectedIndexChanged += new System.EventHandler(this.ShapeColorComboBox_SelectedIndexChanged);
            // 
            // BackgroundColorComboBox
            // 
            this.BackgroundColorComboBox.FormattingEnabled = true;
            this.BackgroundColorComboBox.Location = new System.Drawing.Point(9, 36);
            this.BackgroundColorComboBox.Name = "BackgroundColorComboBox";
            this.BackgroundColorComboBox.Size = new System.Drawing.Size(121, 21);
            this.BackgroundColorComboBox.TabIndex = 20;
            this.BackgroundColorComboBox.SelectedIndexChanged += new System.EventHandler(this.BackgroundColorComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 269);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Shape :";
            // 
            // AngleGroupBox
            // 
            this.AngleGroupBox.Controls.Add(this.label17);
            this.AngleGroupBox.Controls.Add(this.EndAngleTrackBar);
            this.AngleGroupBox.Controls.Add(this.label11);
            this.AngleGroupBox.Controls.Add(this.label10);
            this.AngleGroupBox.Controls.Add(this.label9);
            this.AngleGroupBox.Controls.Add(this.label8);
            this.AngleGroupBox.Controls.Add(this.label7);
            this.AngleGroupBox.Controls.Add(this.label6);
            this.AngleGroupBox.Controls.Add(this.StartAngleTrackBar);
            this.AngleGroupBox.Location = new System.Drawing.Point(16, 312);
            this.AngleGroupBox.Name = "AngleGroupBox";
            this.AngleGroupBox.Size = new System.Drawing.Size(295, 137);
            this.AngleGroupBox.TabIndex = 30;
            this.AngleGroupBox.TabStop = false;
            this.AngleGroupBox.Text = "Angle";
            this.AngleGroupBox.Visible = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 115);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(26, 13);
            this.label17.TabIndex = 15;
            this.label17.Text = "End";
            // 
            // EndAngleTrackBar
            // 
            this.EndAngleTrackBar.LargeChange = 1;
            this.EndAngleTrackBar.Location = new System.Drawing.Point(6, 83);
            this.EndAngleTrackBar.Maximum = 8;
            this.EndAngleTrackBar.Name = "EndAngleTrackBar";
            this.EndAngleTrackBar.Size = new System.Drawing.Size(281, 45);
            this.EndAngleTrackBar.TabIndex = 16;
            this.EndAngleTrackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(198, 65);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(25, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "270";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(134, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "180";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(74, 65);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "90";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(262, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "360";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Start";
            // 
            // StartAngleTrackBar
            // 
            this.StartAngleTrackBar.LargeChange = 1;
            this.StartAngleTrackBar.Location = new System.Drawing.Point(6, 32);
            this.StartAngleTrackBar.Maximum = 8;
            this.StartAngleTrackBar.Name = "StartAngleTrackBar";
            this.StartAngleTrackBar.Size = new System.Drawing.Size(281, 45);
            this.StartAngleTrackBar.TabIndex = 0;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(327, 464);
            this.Controls.Add(this.AngleGroupBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ShapeComboBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.OutlineCheckBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.screenSaverAuthor);
            this.Controls.Add(this.screenSaverTitle);
            this.Name = "SettingsForm";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 15, 15);
            this.ShowIcon = false;
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.BorderNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShapeNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShapeColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundColorBox)).EndInit();
            this.AngleGroupBox.ResumeLayout(false);
            this.AngleGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EndAngleTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartAngleTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label screenSaverTitle;
        private System.Windows.Forms.Label screenSaverAuthor;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox OutlineCheckBox;
        private System.Windows.Forms.ComboBox ShapeComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown BorderNumericUpDown;
        private System.Windows.Forms.NumericUpDown ShapeNumericUpDown;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ShapeColorComboBox;
        private System.Windows.Forms.ComboBox BackgroundColorComboBox;
        private System.Windows.Forms.PictureBox ShapeColorBox;
        private System.Windows.Forms.PictureBox BackgroundColorBox;
        private System.Windows.Forms.GroupBox AngleGroupBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TrackBar StartAngleTrackBar;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TrackBar EndAngleTrackBar;
    }
}