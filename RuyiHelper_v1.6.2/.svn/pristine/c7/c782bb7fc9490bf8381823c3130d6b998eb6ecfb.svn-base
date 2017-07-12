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
    public partial class FormStatistics : Form
    {
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        string _id = "";
        List<AnswerInfo> _answerList;
        Animate animate;
        public PictureBox itemValueCanvas;
        PictureBox answerBox;
        ArrayList canvases;
        int _opa;
        double _lOpa;
        double _rOpa;
        double _lineMax;
        int _itemW;
        int _aH;
        int _labelH;
        int _pTop;
        int _pLeft;
        int _rightAnswerIndex = 0;
        string[] answerList = { "A", "B", "C", "D", "R", "W","未答"};
        string[] answerList2 = { "A", "B", "C", "D", "✔", "✘", "未答" };
        decimal[] countValues = { 0, 0, 0, 0, 0, 0, 1};
        int[] peopleValues = { 0, 0, 0, 0, 0, 0, 0 };
        private string RESULT;
        public FormStatistics(string id, string type, string answer, string numberstr, string result)
        {
            _id = id;
            RESULT = result;
            InitializeComponent();
           
            this.panel2.Width = screenWidth - 40;
            this.panel2.Height = screenHeight - 100;
            this.panel2.Location = new System.Drawing.Point(20, 100);
            SetPanel(numberstr, answer);
            this.Text = "习题[" + id + "]统计结果";
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
#if DEBUG
            this.TopMost = false;//Form8习题结果统计， zzz
#endif
            
            SetItem(answer, answer);

        }
        /// <summary>
        /// 设置答题结果显示界面
        /// </summary>
        /// <param name="numberstr"></param>
        /// <param name="selectAnswer"></param>
        public void SetPanel(string numberstr, string selectAnswer)
        {
            string answerStr = RESULT;
            int answered = 0;
            ClassInfo ci = JsonOper.DeserializeJsonToObject<ClassInfo>(numberstr);
            peopleValues[peopleValues.Length - 1] = ci.Data.StudentCount;//diff
            if (answerStr != "")
            {
                _answerList = new List<AnswerInfo>();
                for (int i = 0; i < answerStr.Split(',').Length; i++)
                {
                    string tmp = answerStr.Split(',')[i];
                    if (tmp != "")
                    {
                        AnswerInfo ai = new AnswerInfo();
                        ai.CBID = Convert.ToInt16(tmp.Split(':')[0]);
                        ai.CBAnswer = tmp.Split(':')[1];
                        _answerList.Add(ai);
                        answered++;
                    }
                }
            }
            else
            {
                this.panel1.Visible = false;
            }
            int _firstAnswerCount = 0;
            List<AnswerInfo> _firstAnswerList = null;
            if (_answerList != null)
            {
                for (int i = 0; i < answerList.Length; i++)
                {
                    List<AnswerInfo> result;
                    if (answerList[i] == "未答")
                    {
                        result = (from x in _answerList where x.CBAnswer == "S" select x).ToList<AnswerInfo>();
                        countValues[i] = (decimal)(result.Count + (ci.Data.StudentCount - answered)) / (decimal)ci.Data.StudentCount;
                        peopleValues[i] = result.Count + ci.Data.StudentCount - answered;
                    }
                    else
                    {
                        result = (from x in _answerList where x.CBAnswer == answerList[i] select x).ToList<AnswerInfo>();
                        countValues[i] = (decimal)result.Count / (decimal)ci.Data.StudentCount;
                        peopleValues[i] = result.Count;
                    }

                    string _aConver = selectAnswer;
                    if (selectAnswer == "W")
                        _aConver = "✘";
                    else if (selectAnswer == "R")
                        _aConver = "✔";
                    else
                        _aConver = selectAnswer;

                    if (answerList2[i] == _aConver)
                    {
                        List<AnswerInfo> selectResult = null;
                        if (selectAnswer == "未答")
                        {
                            selectResult = new List<AnswerInfo>();
                            List<AnswerInfo> _temp = (from x in _answerList where x.CBAnswer != "S" select x).ToList<AnswerInfo>();
                            for (int j = 1; j <= ci.Data.StudentCount; j++)
                            {
                                if (_temp.Count == 0)
                                {
                                    AnswerInfo ai = new AnswerInfo();
                                    ai.CBID = j;
                                    ai.CBAnswer = "S";
                                    selectResult.Add(ai);
                                }
                                else
                                {
                                    bool ishas = true;
                                    for (int k = 0; k < _temp.Count; k++)
                                    {
                                        if (j == _temp[k].CBID)
                                        {
                                            ishas = false;
                                            break;
                                        }
                                    }
                                    if (ishas)
                                    {
                                        AnswerInfo ai = new AnswerInfo();
                                        ai.CBID = j;
                                        ai.CBAnswer = "S";
                                        selectResult.Add(ai);
                                    }
                                }
                            }
                        }
                        else
                        {
                            selectResult = (from x in _answerList where x.CBAnswer == selectAnswer select x).ToList<AnswerInfo>();
                        }
                        _firstAnswerCount = selectResult.Count;
                        _firstAnswerList = selectResult;
                    }
                }
            }
            int _count = screenWidth / 51;
            int _line = _firstAnswerCount % _count > 0 ? _firstAnswerCount / _count + 1 : _firstAnswerCount / _count;
            int panelHeight = _line * 50 + 10;
            int panelLH = this.screenHeight - panelHeight;
            this.panel2.Height = panelLH;
            this.panel1.Location = new System.Drawing.Point(0, panelLH);
            this.panel1.Size = new System.Drawing.Size(screenWidth, panelHeight);
            this.panel1.BringToFront();
            int _lw = 0;
            int _br = 1;
            int _locationWidth = 0;
            int _locationHeight = 40;
            int _tpi = 1;
            if (_firstAnswerList != null)
            {
                if (_firstAnswerList.Count > 0)
                {
                    this.panel1.Controls.Clear();
                    this.panel1.Visible = true;
                }
                foreach (AnswerInfo rl in _firstAnswerList)
                {
                    _locationHeight = (_lw * 40) + ((_lw + 1) * 10);
                    if (_tpi == 1 || _br % 2 == 0)
                    {
                        _locationWidth = (screenWidth - _count * 51) / 2 + 5;
                    }
                    else
                    {
                        _locationWidth += 51;
                    }
                    if (_tpi % _count == 0)
                    {
                        _br *= 2;
                        if (_lw <= _line)
                        {
                            _lw++;
                        }
                    }
                    else
                    {
                        _br = 1;
                    }
                    Label label = new Label();
                    label.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    label.ForeColor = System.Drawing.Color.White;
                    Image lbimg = global::RueHelper.Properties.Resources.select;
                    label.Image = lbimg;
                    label.Location = new System.Drawing.Point(_locationWidth, _locationHeight);
                    label.Name = "click_" + _tpi;
                    label.Size = new System.Drawing.Size(41, 40);
                    label.TabIndex = 0;
                    label.Text = rl.CBID + "";
                    label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    this.panel1.Controls.Add(label);
                    _tpi++;
                }
            }
        }

        public void SetItem(string answer,string selectAnswer)
        {
            _opa = screenWidth - 80;
            _lOpa = _opa * 0.65;
            _rOpa = _opa * 0.35;
            _lineMax = _lOpa - 125;
            _itemW = 125;
            _aH = this.panel2.Height - 20;
            _labelH = 34;
            _pTop = 20;
            _pLeft = 20;
            itemValueCanvas = new PictureBox();
            itemValueCanvas.BackColor = System.Drawing.Color.White;
            itemValueCanvas.Location = new System.Drawing.Point(_itemW, 60);

            itemValueCanvas.Name = "itemValue";
            itemValueCanvas.Size = new System.Drawing.Size((int)_lineMax, _aH);
            itemValueCanvas.TabIndex = 0;
            itemValueCanvas.TabStop = false;
            this.panel2.Controls.Add(itemValueCanvas);


            //////////////////////////////////////
            // 右侧正确答案
            //////////////////////////////////////
            answerBox = new PictureBox();
            answerBox.BackColor = System.Drawing.Color.White;
            answerBox.Location = new System.Drawing.Point((int)_lineMax + 125 + _pLeft, 0);
            answerBox.Name = "answerBox";
            answerBox.Size = new System.Drawing.Size((int)_rOpa, _aH);
            answerBox.TabIndex = 0;
            answerBox.TabStop = false;
            if (answer != null && answer.Length > 0)
                this.panel2.Controls.Add(answerBox);

            //
            PictureBox answerTextBox = new PictureBox();
            answerTextBox.BackColor = System.Drawing.Color.Transparent;
            answerTextBox.Location = new System.Drawing.Point(((int)_rOpa - 210) / 2, 70);
            answerTextBox.Name = "answerTextBox";
            answerTextBox.Size = new System.Drawing.Size(210, 210);
            answerTextBox.TabIndex = 0;
            answerTextBox.TabStop = false;
            if (answer != null && answer.Length > 0) 
                answerBox.Controls.Add(answerTextBox);


            //正确答案几个字
            PictureBox answerDescBox = new PictureBox();
            answerDescBox.BackColor = System.Drawing.Color.Transparent;
            answerDescBox.Location = new System.Drawing.Point(48, 40);
            answerDescBox.Name = "answerDescBox";
            answerDescBox.Size = new System.Drawing.Size(210, 210);
            answerDescBox.TabIndex = 0;
            answerDescBox.TabStop = false;
            if (answer != null && answer.Length > 0)
                answerTextBox.Controls.Add(answerDescBox);


            canvases = new ArrayList();
            for (int i = 0; i < answerList.Length; i++)
            {
                Label label = new Label();
                label.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                label.ForeColor = System.Drawing.Color.FromArgb(102, 194, 217);
                Image lbimg = global::RueHelper.Properties.Resources.unanswer;

                //更改想查看的答案的背景
                if (answerList[i] == selectAnswer)
                {
                    lbimg = global::RueHelper.Properties.Resources.answer;
                    label.ForeColor = System.Drawing.Color.White;
                }
                label.Location = new System.Drawing.Point(_pLeft, (i + 1) * _pTop + (i * _labelH) +60);


                label.Image = lbimg;
                label.Name = "answeritem_" + i;
                label.Size = new System.Drawing.Size(105, _labelH);
                label.TabIndex = 0;
                label.Text = answerList[i];
                label.Text = answerList2[i];
                label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                this.panel2.Controls.Add(label);
                PictureBox canvas = new PictureBox();

                canvas.BackColor = System.Drawing.Color.White;
                canvas.Location = new System.Drawing.Point(_pLeft, (i + 1) * _pTop + (i * _labelH)  );
                canvas.Name = "itemValueCanvas" + i;
                canvas.Size = new System.Drawing.Size((int)_lineMax, _labelH);
                canvas.TabIndex = 0;
                canvas.TabStop = false;
                itemValueCanvas.Controls.Add(canvas);
                canvases.Add(canvas);
            }
            
            AnswerCount ac = new AnswerCount();
            ac.ImageWidth = answerBox.Width;
            ac.ImagesHeight = answerBox.Height;
            if (answer=="W")
            {
                ac.AnswerText = "✘";
            }
            else if (answer == "R")
            {
                ac.AnswerText = "✔";
            }
            else
            {
                ac.AnswerText = answer;
            }
            ac.AnswerFamily = "Arial";
            ac.AnswerFontSize = 86f;
            ac.FontStyle = System.Drawing.FontStyle.Bold;
            ac.AnswerColor = Brushes.ForestGreen;
            ac.TextX = 46;
            ac.TextY = 70;
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
            animate = new Animate(canvases, _lineMax, _rightAnswerIndex, _labelH, _rOpa, countValues, answerBox, itemValueCanvas, peopleValues);
            animate.StartAnimate();
        }

        private void Form8_FormClosing(object sender, FormClosingEventArgs e)
        {
            animate.StopT1();
            animate.StopT2();
            this.Controls.Clear();
        }

        public void StopAnimate()
        {
            animate.StopT1();
            animate.StopT2();
        }
    }
}
