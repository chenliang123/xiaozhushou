namespace RueHelper
{
    partial class FormCamera
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCamera));
            this.btnCapt = new System.Windows.Forms.Button();
            this.btn_GetDevice = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnCapt
            // 
            this.btnCapt.Location = new System.Drawing.Point(12, 217);
            this.btnCapt.Name = "btnCapt";
            this.btnCapt.Size = new System.Drawing.Size(92, 33);
            this.btnCapt.TabIndex = 0;
            this.btnCapt.Text = "截屏";
            this.btnCapt.UseVisualStyleBackColor = true;
            this.btnCapt.Visible = false;
            this.btnCapt.Click += new System.EventHandler(this.btnCapt_Click);
            // 
            // btn_GetDevice
            // 
            this.btn_GetDevice.Location = new System.Drawing.Point(11, 256);
            this.btn_GetDevice.Name = "btn_GetDevice";
            this.btn_GetDevice.Size = new System.Drawing.Size(93, 32);
            this.btn_GetDevice.TabIndex = 1;
            this.btn_GetDevice.Text = "获取设备";
            this.btn_GetDevice.UseVisualStyleBackColor = true;
            this.btn_GetDevice.Visible = false;
            this.btn_GetDevice.Click += new System.EventHandler(this.btn_GetDevice_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Location = new System.Drawing.Point(1, -1);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(503, 329);
            this.flowLayoutPanel2.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 66);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(147, 145);
            this.flowLayoutPanel1.TabIndex = 3;
            this.flowLayoutPanel1.Visible = false;
            // 
            // FormCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 326);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.btn_GetDevice);
            this.Controls.Add(this.btnCapt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCamera";
            this.Text = "摄像头";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCapt;
        private System.Windows.Forms.Button btn_GetDevice;
        private System.Windows.Forms.Panel flowLayoutPanel2;
        private System.Windows.Forms.Panel flowLayoutPanel1;
    }
}