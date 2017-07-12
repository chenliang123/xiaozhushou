using RueHelper.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RueHelper
{
    public partial class FormConfigSchool : Form
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public FormConfigSchool()
        {
            InitializeComponent();
            loadSchoolConfig();

            button_modify.Enabled = false;
        }

        public void loadSchoolConfig()
        {
            textBox_schoolid.Text = Global.getSchoolID()+"";
            textBox_schoolname.Text = Global.getSchoolname();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_modify_Click(object sender, EventArgs e)
        {
            //从配置文件中获取
            int schoolid = 0;
            try
            {
                schoolid = Int32.Parse(textBox_schoolid.Text);
            }
            catch (Exception e1)
            {
                MessageBox.Show("学校ID参数设置错误，请重试！", "警告");
                return;
            }
            Log.Info("Config.1 schoolid=" + schoolid);

            int nSchoolid = Int32.Parse(textBox_schoolid.Text);
            string authcode = textBox_schoolauthcode.Text;
            if (Global.loadSchoolInfo(nSchoolid, authcode) == 1)
            {
                //重置默认第一个班级
                Classes[] classlist = Global.g_szClasses;
                if(classlist.Length == 0)
                {
                    MessageBox.Show("学校更新成功.\r\n未获取到班级信息,请联系管理员!", "提示");
                    Classes c = new Classes();
                    c.id = 0;
                    c.name = "";
                    Global.saveClassConfig(c.id, c.name);
                }
                else
                {
                     Classes c = classlist[0];
                     Global.saveClassConfig(c.id, c.name);
                     Global.loadClassInfo();

                     MessageBox.Show("学校更新成功!", "提示");
                }
                Form1.updateFormConfig();
                textBox_schoolname.Text = Global.getSchoolname();
                this.Close();
            }
            else
            {
                MessageBox.Show("设置失败，请检查学校ID是否正确，或网络是否正常!", "提示");
            }
        }

        private void textBox_schoolid_TextChanged(object sender, EventArgs e)
        {
            button_modify.Enabled = true;
        }
    }
}
