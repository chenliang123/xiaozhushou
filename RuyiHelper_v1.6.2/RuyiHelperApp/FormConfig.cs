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
    public partial class FormConfig : Form
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserControl_Config1 config1;
        public UserControl_Config2 config2;
        public FormConfig()
        {
            InitializeComponent();
            config1 = new UserControl_Config1();
            config2 = new UserControl_Config2();

        }
        private void FormConfig_Load(object sender, EventArgs e)
        {
            label1.BackColor = Color.White;
            label2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(config1);
            config1.Show();
        }
        public void refresh()
        {
            config1.loadCfg();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            label1.BackColor = Color.White;
            label2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(config1);
            config1.Show();
        }
        private void label2_Click(object sender, EventArgs e)
        {
            label1.BackColor = System.Drawing.SystemColors.Control;
            label2.BackColor = Color.White;
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(config2);
            config2.Show();
        }
        private void label2_MouseUp(object sender, MouseEventArgs e)
        {

        }




    }
}
