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
    public partial class Form6 : Form
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Form6(string url)
        {
            InitializeComponent();
            try
            {
                this.webBrowser1.Url = new Uri(url);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            webBrowser1.Navigate(webBrowser1.StatusText);
        }
    }
}
