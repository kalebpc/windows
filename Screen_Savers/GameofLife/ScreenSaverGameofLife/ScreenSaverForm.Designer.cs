namespace ScreenSaverGameofLife
{
    partial class ScreenSaverForm
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
            this.components = new System.ComponentModel.Container();
            this.Background = new System.Windows.Forms.PictureBox();
            this.PaintPictureBox = new System.Windows.Forms.PictureBox();
            this.MoveTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Background)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaintPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Background
            // 
            this.Background.Location = new System.Drawing.Point(0, 0);
            this.Background.Name = "Background";
            this.Background.Size = new System.Drawing.Size(1, 1);
            this.Background.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Background.TabIndex = 1;
            this.Background.TabStop = false;
            // 
            // PaintPictureBox
            // 
            this.PaintPictureBox.Location = new System.Drawing.Point(0, 0);
            this.PaintPictureBox.Name = "PaintPictureBox";
            this.PaintPictureBox.Size = new System.Drawing.Size(100, 50);
            this.PaintPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PaintPictureBox.TabIndex = 2;
            this.PaintPictureBox.TabStop = false;
            this.PaintPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.PaintPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PaintPictureBox_MouseClick);
            this.PaintPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PaintPictureBox_MouseMove);
            this.PaintPictureBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.PaintPictureBox_PreviewKeyDown);
            // 
            // MoveTimer
            // 
            this.MoveTimer.Interval = 125;
            this.MoveTimer.Tick += new System.EventHandler(this.MoveTimer_Tick);
            // 
            // ScreenSaverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InfoText;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PaintPictureBox);
            this.Controls.Add(this.Background);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ScreenSaverForm";
            this.Text = "ScreenSaverForm";
            this.Load += new System.EventHandler(this.ScreenSaverForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Background)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaintPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox Background;
        private System.Windows.Forms.PictureBox PaintPictureBox;
        private System.Windows.Forms.Timer MoveTimer;
    }
}

