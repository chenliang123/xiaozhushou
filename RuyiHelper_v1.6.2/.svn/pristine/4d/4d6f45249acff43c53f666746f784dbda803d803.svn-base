using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace RueHelper
{
    public partial class UserControl_Config2 : UserControl
    {
        private MyPPT myppt;
        public UserControl_Config2()
        {
            InitializeComponent();
            this.label1.Text = "1) 检查本机IP是否配置成172.18.201.3";
            this.label2.Text = "2) 检查本机IP与采集器IP是否在同一网段";
            this.label3.Text = "3) 测试是否可以正常访问采集器";
            this.label4.Text = "4) 检查PAD的wifi连接，确保连接到正确的wifi，并尝试登陆";
            this.label5.Text = "注：初次配置时，请确保本PC可以正常访问公网！";
        }

        private void button_pptopen_Click(object sender, EventArgs e)
        {
            string filepath = "";
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "请选择教学资源文件";
            //op.Filter = "All Files(*.*)|*.*|ppt Files(*.ppt)|*.ppt|word 2007 Files(*.doc)|*.doc|excel 2007 Files(*.xls)|*.xls|word Files(*.docx)|*.docx|excel Files(*.xlsx)|*.xlsx";
            op.Filter = "ppt Files(*.ppt,*.pptx)|*.ppt;*.pptx";
            //op.Filter = "ppt Files(*.ppt,*.pptx)|*.ppt;*.pptx|Image Files(*.jpg,*.jpeg,*.bmp,*.png,*.gif)|*.jpg;*.jpeg;*.bmp;*.png;*.gif";

            if (op.ShowDialog() == DialogResult.OK)
            {
                filepath = op.FileName;
                string dir = filepath.Substring(0, filepath.LastIndexOf("\\"));
                string fname = op.SafeFileName;
                string extension = Path.GetExtension(op.FileName);
            }
            else
            {
                return;
            }


            if (myppt == null)
            {
                myppt = new MyPPT();
                myppt.PPTOpen(filepath);
            }
            else
            {
                myppt.PPTClose();
                myppt.PPTOpen(filepath);
            }
        }

        private void button_pptnext_Click(object sender, EventArgs e)
        {
            if (myppt != null)
            {
                myppt.NextSlide();
            }
        }

        private void button_pptmin_Click(object sender, EventArgs e)
        {
            if (myppt != null)
            {
                myppt.minisizeProc();
            }
        }

        private void button_pptmax_Click(object sender, EventArgs e)
        {
            if (myppt != null)
            {
                myppt.maxisizeProc();
            }
        }

        private void button_pptclose_Click(object sender, EventArgs e)
        {
            if (myppt != null)
            {
                myppt.PPTClose();
                myppt = null;
            }
        }
    }
}
