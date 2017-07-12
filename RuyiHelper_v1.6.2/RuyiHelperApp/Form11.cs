using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace RueHelper
{
    /// <summary>
    /// 抢答结果
    /// </summary>
    public partial class Form11 : Form
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        int _opa;
        double _lOpa;
        double _rOpa;
        double _lineMax;
        int _aH;
        int _labelH;
        int _pLeft;
        Animate animate;
        public Form11(string answer,ArrayList al)
        {
            InitializeComponent();

            
            this.panel2.Width = screenWidth - 40;
            this.panel2.Height = screenHeight - 100;
            this.panel2.Location = new System.Drawing.Point(20, 100);
            this.pictureBox2.Size = new System.Drawing.Size(423, 540);//恭喜以下同学获得奖励
            _opa = screenWidth - 80;
            _lOpa = _opa * 0.65;
            _rOpa = _opa * 0.35;
            _lineMax = _lOpa - 125;
            _aH = this.panel2.Height - 20;
            _labelH = 34;
            _pLeft = 20;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
#if DEBUG
            this.TopMost = false;//form11,zzz
#endif

            PictureBox answerBox = new PictureBox();
            answerBox.BackColor = System.Drawing.Color.White;
            answerBox.Location = new System.Drawing.Point((int)_lineMax + 125 + _pLeft, 0);
            answerBox.Name = "answerBox";
            answerBox.Size = new System.Drawing.Size((int)_rOpa, _aH);
            answerBox.TabIndex = 0;
            answerBox.TabStop = false;
            PictureBox answerTextBox = new PictureBox();
            answerTextBox.BackColor = System.Drawing.Color.Transparent;
            answerTextBox.Location = new System.Drawing.Point(((int)_rOpa - 210) / 2, 70);
            answerTextBox.Name = "answerTextBox";
            answerTextBox.Size = new System.Drawing.Size(210, 210);
            answerTextBox.TabIndex = 0;
            answerTextBox.TabStop = false;
            answerBox.Controls.Add(answerTextBox);
            PictureBox answerDescBox = new PictureBox();
            answerDescBox.BackColor = System.Drawing.Color.Transparent;
            answerDescBox.Location = new System.Drawing.Point(48, 40);
            answerDescBox.Name = "answerDescBox";
            answerDescBox.Size = new System.Drawing.Size(210, 210);
            answerDescBox.TabIndex = 0;
            answerDescBox.TabStop = false;
            answerTextBox.Controls.Add(answerDescBox);
            this.panel2.Controls.Add(answerBox);
            AnswerCount ac = new AnswerCount();
            ac.ImageWidth = answerBox.Width;
            ac.ImagesHeight = answerBox.Height;
            ac.TextX = 46;
            ac.TextY = 70;
            if (answer == "W")
            {
                ac.TextX = 35;
                ac.AnswerText = "✘";//×
            }
            else if (answer == "R")
            {
                ac.TextX = 35;
                ac.AnswerText = "✔";//√
            }
            else
            {
                ac.AnswerText = answer;
            }
            ac.AnswerFamily = "Arial";
            ac.AnswerFontSize = 86f;
            ac.FontStyle = System.Drawing.FontStyle.Bold;
            ac.AnswerColor = Brushes.ForestGreen;

            answerTextBox.Image = ac.DrawingString();
            AnswerCount acd = new AnswerCount();
            acd.ImageWidth = answerDescBox.Width;
            acd.ImagesHeight = answerDescBox.Height;
            acd.AnswerText = "正确答案";
            acd.AnswerFamily = "微软雅黑";
            acd.AnswerFontSize = 20f;
            acd.FontStyle = System.Drawing.FontStyle.Regular;
            acd.AnswerColor = Brushes.Black;
            acd.TextX = 0;
            acd.TextY = 0;
            answerDescBox.Image = acd.DrawingString();
            animate = new Animate(_lineMax, _labelH, _rOpa, answerBox, System.Drawing.Color.FromArgb(68, 176, 102));
            animate.StartAnimate();

            if(al.Count==0)
            {
                this.pictureBox2.Image = global::RueHelper.Properties.Resources.goog0;//很遗憾，没有答对哦
            }

            for (int i = 0; i < al.Count; i++)
            {
                Label l = new Label();
                if (i == 0)
                {
                    l.Location = new System.Drawing.Point(160, 200);
                    l.Image = global::RueHelper.Properties.Resources.jp3;
                }
                else if (i == 1)
                {
                    l.Location = new System.Drawing.Point(34, 264);
                    l.Image = global::RueHelper.Properties.Resources.jp2;
                }
                else if (i == 2)
                {
                    l.Location = new System.Drawing.Point(290, 290);
                    l.Image = global::RueHelper.Properties.Resources.jp1;
                }
                l.Size = new System.Drawing.Size(114, 100);
                l.AutoSize = false;
                l.Text = al[i].ToString();
                l.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                l.Font = new System.Drawing.Font("微软雅黑", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.pictureBox2.Controls.Add(l);
            }
        }
    }
}
