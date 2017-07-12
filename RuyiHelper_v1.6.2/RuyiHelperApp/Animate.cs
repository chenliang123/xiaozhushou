using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RueHelper
{
    public delegate void InvokeCallback(int x, PictureBox pb);
    public class Animate
    {
        System.Timers.Timer t1;
        System.Timers.Timer t2;
        PictureBox itemValueCanvas;
        ArrayList canvases;
        int[] ww = { 0, 0, 0, 0, 0, 0, 0};
        int rr = 0;
        int i = 0;
        double lineMax;
        int rightAnswerIndex;
        int labelH;
        double rOpa;
        decimal[] countValues = { };
        int[] peopleValues = { };
        PictureBox answerBox;
        int inTimer1 = 0;
        int inTimer2 = 0;
        Color color = System.Drawing.Color.FromArgb(253, 142, 51);
        public Animate(ArrayList _canvases, double _lineMax, int _rightAnswerIndex, int _labelH, double _rOpa, decimal[] _countValues, PictureBox _answerBox, PictureBox _itemValueCanvas, int[] _peopleValues)
        {
            canvases = _canvases;
            lineMax = _lineMax;
            rightAnswerIndex = _rightAnswerIndex;
            labelH = _labelH;
            rOpa = _rOpa;
            countValues = _countValues;
            answerBox = _answerBox;
            itemValueCanvas = _itemValueCanvas;
            peopleValues = _peopleValues;
        }

        public Animate(ArrayList _canvases, double _lineMax, int _rightAnswerIndex, int _labelH, double _rOpa, decimal[] _countValues, PictureBox _answerBox, int _i, PictureBox _itemValueCanvas, int[] _peopleValues)
        {
            canvases = _canvases;
            lineMax = _lineMax;
            rightAnswerIndex = _rightAnswerIndex;
            labelH = _labelH;
            rOpa = _rOpa;
            countValues = _countValues;
            answerBox = _answerBox;
            i = _i;
            itemValueCanvas = _itemValueCanvas;
            peopleValues = _peopleValues;
        }

        public Animate(double _lineMax, int _labelH, double _rOpa, PictureBox _answerBox, Color _color)
        {
            lineMax = _lineMax;
            labelH = _labelH;
            rOpa = _rOpa;
            answerBox = _answerBox;
            color = _color;
        }

        private void Theout(object sender, System.Timers.ElapsedEventArgs e)
        {
            PictureBox pb = (PictureBox)canvases[i];
            if (Interlocked.Exchange(ref inTimer1, 1) == 0)
            {
                if (ww[i] < ((int)((decimal)(lineMax-70) * countValues[i])))
                {
                    AnswerCount ac = new AnswerCount();
                    ac.LineColor = System.Drawing.Color.FromArgb(102, 194, 217);
                    if (i == rightAnswerIndex)
                    {
                        ac.LineColor = System.Drawing.Color.FromArgb(100, 204, 117);
                    }
                    if (i == canvases.Count - 2)
                    {
                        ac.LineColor = System.Drawing.Color.FromArgb(142, 142, 142);
                    }
                    if (i == canvases.Count - 1)
                    {
                        ac.LineColor = System.Drawing.Color.FromArgb(254, 136, 134);
                    }
                    ac.ImageWidth = pb.Width;
                    ac.ImagesHeight = pb.Height;
                    ac.LineStartX = 0;
                    ac.LineStartY = 18;
                    ac.LineEndX = ww[i];
                    ac.LineEndY = 18;
                    ac.LineWidth = labelH;
                    pb.Image = ac.DrawingLine();
                    ww[i] = ww[i] + 10;
                }
                else
                {
                    LabelEvent(ww[i], pb);
                    System.Timers.Timer t = (System.Timers.Timer)sender;
                    t.Stop();
                }
                Interlocked.Exchange(ref inTimer1, 0);
            }
        }

        private void TheoutArc(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Interlocked.Exchange(ref inTimer2, 1) == 0)
            {
                if (rr <= 360)
                {
                    AnswerCount ac = new AnswerCount();
                    ac.ImageWidth = answerBox.Width;
                    ac.ImagesHeight = answerBox.Height;
                    ac.AnswerBackColor = System.Drawing.Color.FromArgb(212, 214, 213);
                    ac.AnswerBorderColor = color;
                    ac.AnswerBorderWidth = 10;
                    ac.AnswerW = 216;
                    ac.AnswerH = 216;
                    ac.AnswerX = ((int)rOpa - 210) / 2;
                    ac.AnswerY = 70;
                    ac.AnswerSweep = rr;
                    answerBox.Image = ac.DrawingArc();
                    rr = rr + 6;
                }
                else
                {
                    t2.Stop();
                }
            }
            Interlocked.Exchange(ref inTimer2, 0); 
        }

        public void StartAnimate()
        {
            if (canvases != null)
            {
                for (int i = 0; i < canvases.Count; i++)
                {
                    t1 = new System.Timers.Timer(1);
                    t1.AutoReset = true;
                    t1.Elapsed += new Animate(canvases, lineMax, rightAnswerIndex, labelH, rOpa, countValues, answerBox, i, itemValueCanvas, peopleValues).Theout;
                    t1.Start();
                }
            }
            t2 = new System.Timers.Timer(1);
            t2.AutoReset = true;
            t2.Elapsed += TheoutArc;
            t2.Start();
        }

        public void StopT1()
        {
            t1.Stop();
        }

        public void StopT2()
        {
            t2.Stop();
        }

        public void LabelEvent(int x, PictureBox pb)
        {
            if (pb.InvokeRequired)
            {
                InvokeCallback labelCallback = new InvokeCallback(LabelEvent);
                pb.Invoke(labelCallback, new object[] { x, pb });
            }
            else
            {
                PictureBox text = new PictureBox();
                AnswerCount acd = new AnswerCount();
                acd.ImageWidth = 50;
                acd.ImagesHeight = 40;
                acd.AnswerText = peopleValues[i] + "人";
                acd.AnswerFamily = "微软雅黑";
                acd.AnswerFontSize = 16f;
                acd.FontStyle = System.Drawing.FontStyle.Regular;
                acd.AnswerColor = Brushes.Black;
                acd.TextX = 0;
                acd.TextY = 4;
                text.Image = acd.DrawingString();
                itemValueCanvas.Controls.Add(text);
                text.BackColor = System.Drawing.Color.Transparent;
                text.Location = new System.Drawing.Point(x + 20, (i + 1) * 20 + (i * labelH));
                text.BringToFront();

                //pb.Size = new System.Drawing.Size((int)lineMax - 100, labelH);
                //pb.Location = new System.Drawing.Point(x - 100);
            }
        }
    }
}
