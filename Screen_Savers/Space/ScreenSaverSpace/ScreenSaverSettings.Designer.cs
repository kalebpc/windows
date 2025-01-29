namespace ScreenSaverSpace
{
    partial class ScreenSaverSettings
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.StarSpeedNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.StarSizeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.StarCountNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.StarColorBox = new System.Windows.Forms.PictureBox();
            this.BackgroundColorBox = new System.Windows.Forms.PictureBox();
            this.StarColorComboBox = new System.Windows.Forms.ComboBox();
            this.BackgroundColorComboBox = new System.Windows.Forms.ComboBox();
            this.MouseMoveCheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.StarSpeedNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StarSizeNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StarCountNumericUpDown)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StarColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundColorBox)).BeginInit();
            this.SuspendLayout();
            // 
            // screenSaverTitle
            // 
            this.screenSaverTitle.AutoSize = true;
            this.screenSaverTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenSaverTitle.Location = new System.Drawing.Point(13, 9);
            this.screenSaverTitle.Name = "screenSaverTitle";
            this.screenSaverTitle.Size = new System.Drawing.Size(100, 13);
            this.screenSaverTitle.TabIndex = 1;
            this.screenSaverTitle.Text = "ScreenSaverSpace";
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
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Star Color";
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
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Star Size :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Star Speed :";
            // 
            // StarSpeedNumericUpDown
            // 
            this.StarSpeedNumericUpDown.DecimalPlaces = 3;
            this.StarSpeedNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.StarSpeedNumericUpDown.Location = new System.Drawing.Point(114, 42);
            this.StarSpeedNumericUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StarSpeedNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.StarSpeedNumericUpDown.Name = "StarSpeedNumericUpDown";
            this.StarSpeedNumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.StarSpeedNumericUpDown.TabIndex = 25;
            this.StarSpeedNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // StarSizeNumericUpDown
            // 
            this.StarSizeNumericUpDown.AutoSize = true;
            this.StarSizeNumericUpDown.Location = new System.Drawing.Point(114, 16);
            this.StarSizeNumericUpDown.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.StarSizeNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StarSizeNumericUpDown.Name = "StarSizeNumericUpDown";
            this.StarSizeNumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.StarSizeNumericUpDown.TabIndex = 26;
            this.StarSizeNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.StarCountNumericUpDown);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.StarSpeedNumericUpDown);
            this.groupBox1.Controls.Add(this.StarSizeNumericUpDown);
            this.groupBox1.Location = new System.Drawing.Point(16, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 100);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stars";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Star Count :";
            // 
            // StarCountNumericUpDown
            // 
            this.StarCountNumericUpDown.Location = new System.Drawing.Point(114, 68);
            this.StarCountNumericUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.StarCountNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StarCountNumericUpDown.Name = "StarCountNumericUpDown";
            this.StarCountNumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.StarCountNumericUpDown.TabIndex = 28;
            this.StarCountNumericUpDown.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.StarColorBox);
            this.groupBox2.Controls.Add(this.BackgroundColorBox);
            this.groupBox2.Controls.Add(this.StarColorComboBox);
            this.groupBox2.Controls.Add(this.BackgroundColorComboBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(16, 151);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(191, 114);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Color";
            // 
            // StarColorBox
            // 
            this.StarColorBox.Location = new System.Drawing.Point(138, 60);
            this.StarColorBox.Name = "StarColorBox";
            this.StarColorBox.Size = new System.Drawing.Size(39, 37);
            this.StarColorBox.TabIndex = 23;
            this.StarColorBox.TabStop = false;
            // 
            // BackgroundColorBox
            // 
            this.BackgroundColorBox.Location = new System.Drawing.Point(138, 20);
            this.BackgroundColorBox.Name = "BackgroundColorBox";
            this.BackgroundColorBox.Size = new System.Drawing.Size(39, 37);
            this.BackgroundColorBox.TabIndex = 22;
            this.BackgroundColorBox.TabStop = false;
            // 
            // StarColorComboBox
            // 
            this.StarColorComboBox.FormattingEnabled = true;
            this.StarColorComboBox.Location = new System.Drawing.Point(9, 76);
            this.StarColorComboBox.Name = "StarColorComboBox";
            this.StarColorComboBox.Size = new System.Drawing.Size(121, 21);
            this.StarColorComboBox.TabIndex = 21;
            this.StarColorComboBox.SelectedIndexChanged += new System.EventHandler(this.StarColorComboBox_SelectedIndexChanged);
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
            // MouseMoveCheckBox
            // 
            this.MouseMoveCheckBox.AutoSize = true;
            this.MouseMoveCheckBox.Checked = true;
            this.MouseMoveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MouseMoveCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MouseMoveCheckBox.Location = new System.Drawing.Point(16, 271);
            this.MouseMoveCheckBox.Name = "MouseMoveCheckBox";
            this.MouseMoveCheckBox.Size = new System.Drawing.Size(91, 17);
            this.MouseMoveCheckBox.TabIndex = 13;
            this.MouseMoveCheckBox.Text = "Follow Mouse";
            this.MouseMoveCheckBox.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 291);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(241, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "( Uncheck to close screensaver on MouseMove )";
            // 
            // ScreenSaverSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(327, 320);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.MouseMoveCheckBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.screenSaverAuthor);
            this.Controls.Add(this.screenSaverTitle);
            this.Name = "ScreenSaverSettings";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 15, 15);
            this.ShowIcon = false;
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.StarSpeedNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StarSizeNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StarCountNumericUpDown)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StarColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundColorBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label screenSaverTitle;
        private System.Windows.Forms.Label screenSaverAuthor;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown StarSpeedNumericUpDown;
        private System.Windows.Forms.NumericUpDown StarSizeNumericUpDown;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox StarColorComboBox;
        private System.Windows.Forms.ComboBox BackgroundColorComboBox;
        private System.Windows.Forms.PictureBox StarColorBox;
        private System.Windows.Forms.PictureBox BackgroundColorBox;
        private System.Windows.Forms.CheckBox MouseMoveCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown StarCountNumericUpDown;
        private System.Windows.Forms.Label label6;
    }
}