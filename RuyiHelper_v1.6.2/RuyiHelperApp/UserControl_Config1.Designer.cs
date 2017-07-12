namespace RueHelper
{
    partial class UserControl_Config1
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_hdip = new System.Windows.Forms.TextBox();
            this.button_apply = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_360ip = new System.Windows.Forms.TextBox();
            this.textBox_schoolname = new System.Windows.Forms.TextBox();
            this.button_refresh = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.button_exit = new System.Windows.Forms.Button();
            this.comboBox_classlist = new System.Windows.Forms.ComboBox();
            this.button_changeSchool = new System.Windows.Forms.Button();
            this.textBox_wifi = new System.Windows.Forms.TextBox();
            this.button_chgWifi = new System.Windows.Forms.Button();
            this.button_reloadClasses = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(25, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "学校";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(25, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "班级";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(25, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "采集器IP";
            // 
            // textBox_hdip
            // 
            this.textBox_hdip.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_hdip.Location = new System.Drawing.Point(105, 96);
            this.textBox_hdip.Name = "textBox_hdip";
            this.textBox_hdip.Size = new System.Drawing.Size(165, 23);
            this.textBox_hdip.TabIndex = 5;
            this.textBox_hdip.TextChanged += new System.EventHandler(this.textBox_hdip_TextChanged);
            // 
            // button_apply
            // 
            this.button_apply.Enabled = false;
            this.button_apply.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_apply.Location = new System.Drawing.Point(283, 270);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(75, 23);
            this.button_apply.TabIndex = 9;
            this.button_apply.Text = "应用";
            this.button_apply.UseVisualStyleBackColor = true;
            this.button_apply.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(283, 94);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "测试";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(25, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "本机IP";
            // 
            // textBox_360ip
            // 
            this.textBox_360ip.Enabled = false;
            this.textBox_360ip.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_360ip.Location = new System.Drawing.Point(105, 134);
            this.textBox_360ip.Name = "textBox_360ip";
            this.textBox_360ip.Size = new System.Drawing.Size(165, 23);
            this.textBox_360ip.TabIndex = 12;
            // 
            // textBox_schoolname
            // 
            this.textBox_schoolname.Enabled = false;
            this.textBox_schoolname.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_schoolname.Location = new System.Drawing.Point(106, 25);
            this.textBox_schoolname.Name = "textBox_schoolname";
            this.textBox_schoolname.Size = new System.Drawing.Size(164, 23);
            this.textBox_schoolname.TabIndex = 15;
            // 
            // button_refresh
            // 
            this.button_refresh.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_refresh.Location = new System.Drawing.Point(105, 270);
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button_refresh.Size = new System.Drawing.Size(75, 23);
            this.button_refresh.TabIndex = 18;
            this.button_refresh.Text = "确定";
            this.button_refresh.UseVisualStyleBackColor = true;
            this.button_refresh.Click += new System.EventHandler(this.button_refresh_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(25, 171);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 17);
            this.label8.TabIndex = 20;
            this.label8.Text = "班级WiFi";
            // 
            // button_exit
            // 
            this.button_exit.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_exit.Location = new System.Drawing.Point(195, 270);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(75, 23);
            this.button_exit.TabIndex = 21;
            this.button_exit.Text = "取消";
            this.button_exit.UseVisualStyleBackColor = true;
            this.button_exit.Click += new System.EventHandler(this.button_exit_Click);
            // 
            // comboBox_classlist
            // 
            this.comboBox_classlist.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_classlist.FormattingEnabled = true;
            this.comboBox_classlist.Location = new System.Drawing.Point(106, 57);
            this.comboBox_classlist.Name = "comboBox_classlist";
            this.comboBox_classlist.Size = new System.Drawing.Size(164, 25);
            this.comboBox_classlist.TabIndex = 24;
            this.comboBox_classlist.SelectedIndexChanged += new System.EventHandler(this.comboBox_classlist_SelectedIndexChanged);
            // 
            // button_changeSchool
            // 
            this.button_changeSchool.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_changeSchool.Location = new System.Drawing.Point(283, 25);
            this.button_changeSchool.Name = "button_changeSchool";
            this.button_changeSchool.Size = new System.Drawing.Size(75, 23);
            this.button_changeSchool.TabIndex = 25;
            this.button_changeSchool.Text = "更改";
            this.button_changeSchool.UseVisualStyleBackColor = true;
            this.button_changeSchool.Click += new System.EventHandler(this.button_changeSchool_Click);
            // 
            // textBox_wifi
            // 
            this.textBox_wifi.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_wifi.Location = new System.Drawing.Point(106, 171);
            this.textBox_wifi.Name = "textBox_wifi";
            this.textBox_wifi.Size = new System.Drawing.Size(164, 23);
            this.textBox_wifi.TabIndex = 26;
            // 
            // button_chgWifi
            // 
            this.button_chgWifi.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_chgWifi.Location = new System.Drawing.Point(283, 169);
            this.button_chgWifi.Name = "button_chgWifi";
            this.button_chgWifi.Size = new System.Drawing.Size(75, 23);
            this.button_chgWifi.TabIndex = 27;
            this.button_chgWifi.Text = "修改";
            this.button_chgWifi.UseVisualStyleBackColor = true;
            this.button_chgWifi.Click += new System.EventHandler(this.button_chgWifi_Click);
            // 
            // button_reloadClasses
            // 
            this.button_reloadClasses.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_reloadClasses.Location = new System.Drawing.Point(283, 60);
            this.button_reloadClasses.Name = "button_reloadClasses";
            this.button_reloadClasses.Size = new System.Drawing.Size(75, 23);
            this.button_reloadClasses.TabIndex = 28;
            this.button_reloadClasses.Text = "刷新";
            this.button_reloadClasses.UseVisualStyleBackColor = true;
            this.button_reloadClasses.Click += new System.EventHandler(this.button_reloadClasses_Click);
            // 
            // UserControl_Config1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.button_reloadClasses);
            this.Controls.Add(this.button_chgWifi);
            this.Controls.Add(this.textBox_wifi);
            this.Controls.Add(this.button_changeSchool);
            this.Controls.Add(this.comboBox_classlist);
            this.Controls.Add(this.button_exit);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button_refresh);
            this.Controls.Add(this.textBox_schoolname);
            this.Controls.Add(this.textBox_360ip);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button_apply);
            this.Controls.Add(this.textBox_hdip);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "UserControl_Config1";
            this.Size = new System.Drawing.Size(400, 330);
            this.Load += new System.EventHandler(this.UserControl_Config1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_hdip;
        private System.Windows.Forms.Button button_apply;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_360ip;
        private System.Windows.Forms.TextBox textBox_schoolname;
        private System.Windows.Forms.Button button_refresh;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button_exit;
        private System.Windows.Forms.ComboBox comboBox_classlist;
        private System.Windows.Forms.Button button_changeSchool;
        private System.Windows.Forms.TextBox textBox_wifi;
        private System.Windows.Forms.Button button_chgWifi;
        private System.Windows.Forms.Button button_reloadClasses;
    }
}
