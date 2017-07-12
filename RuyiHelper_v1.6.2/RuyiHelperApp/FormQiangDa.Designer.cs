using System.Drawing;
namespace RueHelper
{
    partial class FormQiangDa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQiangDa));
            this.pictureBox_headImage = new System.Windows.Forms.PictureBox();
            this.label_name = new System.Windows.Forms.Label();
            this.label_title = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_headImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_headImage
            // 
            this.pictureBox_headImage.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_headImage.Image = global::RueHelper.Properties.Resources.qd_start;
            this.pictureBox_headImage.Location = new System.Drawing.Point(189, 153);
            this.pictureBox_headImage.Name = "pictureBox_headImage";
            this.pictureBox_headImage.Size = new System.Drawing.Size(242, 242);
            this.pictureBox_headImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_headImage.TabIndex = 14;
            this.pictureBox_headImage.TabStop = false;
            // 
            // label_name
            // 
            this.label_name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(95)))), ((int)(((byte)(255)))));
            this.label_name.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_name.ForeColor = System.Drawing.Color.White;
            this.label_name.Location = new System.Drawing.Point(161, 438);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(293, 88);
            this.label_name.TabIndex = 22;
            this.label_name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_name.Paint += new System.Windows.Forms.PaintEventHandler(this.label_name_Paint);
            // 
            // label_title
            // 
            this.label_title.BackColor = System.Drawing.Color.White;
            this.label_title.Font = new System.Drawing.Font("黑体", 32F, System.Drawing.FontStyle.Bold);
            this.label_title.ForeColor = System.Drawing.Color.White;
            this.label_title.Image = global::RueHelper.Properties.Resources.qd_title;
            this.label_title.Location = new System.Drawing.Point(58, 31);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(494, 77);
            this.label_title.TabIndex = 23;
            this.label_title.Text = "开 始 抢 答";
            this.label_title.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // FormQiangDa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(670, 525);
            this.Controls.Add(this.label_title);
            this.Controls.Add(this.label_name);
            this.Controls.Add(this.pictureBox_headImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormQiangDa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.FormQiangDa_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormVote_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_headImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_headImage;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label_title;


    }
}