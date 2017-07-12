namespace RueHelper
{
    partial class FormGroup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGroup));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label_title = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_top3_3 = new System.Windows.Forms.Label();
            this.label_top3_2 = new System.Windows.Forms.Label();
            this.label_top3_1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(242)))), ((int)(((byte)(240)))));
            this.panel1.Location = new System.Drawing.Point(12, 246);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(635, 62);
            this.panel1.TabIndex = 6;
            this.panel1.DoubleClick += new System.EventHandler(this.panel1_DoubleClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::RueHelper.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(670, 80);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("微软雅黑", 28F);
            this.label_title.ForeColor = System.Drawing.Color.LimeGreen;
            this.label_title.Location = new System.Drawing.Point(246, 93);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(153, 50);
            this.label_title.TabIndex = 5;
            this.label_title.Text = "*组成员";
            this.label_title.Visible = false;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(12, 86);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(194, 144);
            this.webBrowser1.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(242)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.label_top3_3);
            this.panel2.Controls.Add(this.label_top3_2);
            this.panel2.Controls.Add(this.label_top3_1);
            this.panel2.Location = new System.Drawing.Point(12, 314);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(635, 74);
            this.panel2.TabIndex = 7;
            this.panel2.DoubleClick += new System.EventHandler(this.panel2_DoubleClick);
            // 
            // label_top3_3
            // 
            this.label_top3_3.AutoSize = true;
            this.label_top3_3.Font = new System.Drawing.Font("微软雅黑", 25F, System.Drawing.FontStyle.Bold);
            this.label_top3_3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(187)))), ((int)(((byte)(98)))));
            this.label_top3_3.Location = new System.Drawing.Point(421, 10);
            this.label_top3_3.Name = "label_top3_3";
            this.label_top3_3.Size = new System.Drawing.Size(143, 45);
            this.label_top3_3.TabIndex = 2;
            this.label_top3_3.Text = "Top3_3";
            // 
            // label_top3_2
            // 
            this.label_top3_2.AutoSize = true;
            this.label_top3_2.Font = new System.Drawing.Font("微软雅黑", 25F, System.Drawing.FontStyle.Bold);
            this.label_top3_2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(187)))), ((int)(((byte)(98)))));
            this.label_top3_2.Location = new System.Drawing.Point(244, 10);
            this.label_top3_2.Name = "label_top3_2";
            this.label_top3_2.Size = new System.Drawing.Size(143, 45);
            this.label_top3_2.TabIndex = 1;
            this.label_top3_2.Text = "Top3_2";
            // 
            // label_top3_1
            // 
            this.label_top3_1.AutoSize = true;
            this.label_top3_1.Font = new System.Drawing.Font("微软雅黑", 25F, System.Drawing.FontStyle.Bold);
            this.label_top3_1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(187)))), ((int)(((byte)(98)))));
            this.label_top3_1.Location = new System.Drawing.Point(51, 10);
            this.label_top3_1.Name = "label_top3_1";
            this.label_top3_1.Size = new System.Drawing.Size(143, 45);
            this.label_top3_1.TabIndex = 0;
            this.label_top3_1.Text = "Top3_1";
            // 
            // FormGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(670, 400);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.label_title);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormGroup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label_top3_3;
        private System.Windows.Forms.Label label_top3_2;
        private System.Windows.Forms.Label label_top3_1;


    }
}