using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace RueHelper
{
    public sealed class AnswerCount
    {
        private string answertext;
        public int ImageWidth { get; set; }
        public int ImagesHeight { get; set; }
        public string AnswerFamily { get; set; }
        public float AnswerFontSize { get; set; }
        public FontStyle FontStyle { get; set; }
        public string AnswerText{get;set;}
        public int TextX { get; set; }
        public int TextY { get; set; }
        public Brush AnswerColor { get; set; }
        public Color AnswerBorderColor { get; set; }
        public Color AnswerBackColor { get; set; }
        public int AnswerBorderWidth { get; set; }
        public int AnswerSweep { get; set; }
        public int AnswerX { get; set; }
        public int AnswerY { get; set; }
        public int AnswerW { get; set; }
        public int AnswerH { get; set; }
        public int LineWidth { get; set; }
        public Color LineColor { get; set; }
        public int LineStartX { get; set; }
        public int LineStartY { get; set; }
        public int LineEndX { get; set; }
        public int LineEndY { get; set; }

        private SmoothingMode drawMode = SmoothingMode.HighQuality;
        private TextRenderingHint textMode = TextRenderingHint.AntiAlias;
        private Graphics g;

        public AnswerCount()
        {

        }

        public Image DrawingArc()
        {
            Bitmap bmp = new Bitmap(this.ImageWidth, this.ImagesHeight);
            g = Graphics.FromImage(bmp);
            g.SmoothingMode = drawMode;
            Pen pen = new Pen(AnswerBorderColor, AnswerBorderWidth);
            Pen backpen = new Pen(AnswerBackColor, AnswerBorderWidth);
            //g.DrawArc(backpen, AnswerX, AnswerY, AnswerW, AnswerH, 0, 360);
            g.DrawArc(pen, AnswerX, AnswerY, AnswerW, AnswerH, 0, AnswerSweep);
            g.Dispose();
            return bmp;
        }

        public Image DrawingArcWithText(int r, Color backgroundColor, Color fontColor, string text1, string text2)
        {
            int BrushWidth = 10;
            int r1 = r + 2 * BrushWidth;
            int r0 = r - BrushWidth;

            Bitmap bmp = new Bitmap(r1, r1);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = drawMode;
            //Pen pen = new Pen(AnswerBorderColor, AnswerBorderWidth);
            Pen pen = new Pen(backgroundColor, AnswerBorderWidth);
            Pen borderPen = new Pen(fontColor, AnswerBorderWidth);
            g.DrawArc(pen, AnswerX, AnswerY, r0, r0, 270, 360);
            g.DrawArc(borderPen, AnswerX, AnswerY, r0, r0, 270, AnswerSweep);

            int r3 = r0 - 10;
            Rectangle rect = new Rectangle(4 + (r0 - r3) / 2, 4 + (r0 - r3) / 2, r3, r3);
            Pen p = new Pen(backgroundColor);
            g.DrawEllipse(p, rect);
            Brush b = new SolidBrush(backgroundColor);
            g.FillEllipse(b, rect);

            /////////////////////////////////////////////////////////////////////////
            float fontSize1 = r0 * 30 / 100;
            float fontSize2 = r0 * 16 / 100;
            g.TextRenderingHint = textMode;


            double pointX1 = 0.20 * r0;
            if (text1.Length < 2)
                pointX1 = 0.30 * r0;//100%
            double pointY1 = 0.18 * r0;
            double pointX2 = 0.25 * r0;
            double pointY2 = 0.65 * r0;

            if (text2.Length > 5)
                pointX2 = 0.16 * r0;//100.0%
            else if (text2.Length > 4)
                pointX2 = 0.20 * r0;//100.0%
            {
                Font _font = new Font(AnswerFamily, fontSize1, FontStyle);
                Brush _brush = new SolidBrush(fontColor);
                SizeF size = g.MeasureString(text1, _font);



                PointF _point = new PointF((float)pointX1, (float)pointY1);
                System.Drawing.StringFormat _sf = new System.Drawing.StringFormat();
                _sf.FormatFlags = StringFormatFlags.DisplayFormatControl;
                g.DrawString(text1, _font, _brush, _point, _sf);
            }
            {
                Font _font1 = new Font(AnswerFamily, fontSize2, FontStyle);
                SizeF size = g.MeasureString(text1, _font1);
                Brush _brush = new SolidBrush(fontColor);
                PointF _point = new PointF((float)pointX2, (float)pointY2);
                System.Drawing.StringFormat _sf = new System.Drawing.StringFormat();
                _sf.FormatFlags = StringFormatFlags.DisplayFormatControl;
                g.DrawString(text2, _font1, _brush, _point, _sf);
            }
            g.Dispose();
            return bmp;
        }

        public Image DrawingArcFill(int arcX,int arcY,Color arcColor,int borderW,int bmpW,Color fillColor)
        {
            Bitmap bmp = new Bitmap(bmpW, bmpW);
            g = Graphics.FromImage(bmp);
            g.SmoothingMode = drawMode;
            if (borderW != 0)
            {
                Pen backpen = new Pen(arcColor, borderW);
                g.DrawArc(backpen, arcX, arcY, ImageWidth - borderW, ImagesHeight - borderW, 0, 360);
            }
            Brush bush = new SolidBrush(fillColor);
            g.FillEllipse(bush, borderW, borderW, this.ImageWidth - borderW * 2, this.ImagesHeight - borderW * 2);
            g.Dispose();
            return bmp;
        }

        public Image DrawingLine()
        {
            Bitmap bmp = new Bitmap(this.ImageWidth, this.ImagesHeight);
            g = Graphics.FromImage(bmp);
            g.SmoothingMode = drawMode;
            Pen pen = new Pen(LineColor, LineWidth);
            g.DrawLine(pen, LineStartX, LineStartY, LineEndX, LineEndY);
            g.Dispose();
            return bmp;
        }


        public Image DrawingString()
        {
            Bitmap bmp = new Bitmap(this.ImageWidth, this.ImagesHeight);
            g = Graphics.FromImage(bmp);
            g.TextRenderingHint = textMode;
            Font _font = new Font(AnswerFamily, AnswerFontSize, FontStyle);
            Brush _brush = AnswerColor;
            PointF _point = new PointF(TextX, TextY);
            System.Drawing.StringFormat _sf = new System.Drawing.StringFormat();
            _sf.FormatFlags = StringFormatFlags.DisplayFormatControl;
            g.DrawString(AnswerText, _font, _brush, _point, _sf);
            g.Dispose();
            return bmp;
        }

        public Image DrawingString(Brush fontColor,string text)
        {
            Bitmap bmp = new Bitmap(this.ImageWidth, this.ImagesHeight);
            g = Graphics.FromImage(bmp);
            g.TextRenderingHint = textMode;
            Font _font = new Font(AnswerFamily, AnswerFontSize, FontStyle);
            SizeF size = g.MeasureString(text, _font);
            Brush _brush = fontColor;
            PointF _point = new PointF((ImageWidth - size.Width) / 2, (ImagesHeight - size.Height) / 2 + 1);
            System.Drawing.StringFormat _sf = new System.Drawing.StringFormat();
            _sf.FormatFlags = StringFormatFlags.DisplayFormatControl;
            g.DrawString(text, _font, _brush, _point, _sf);
            g.Dispose();
            return bmp;
        }

        public Image DrawingCall()
        {
            Bitmap bmp = new Bitmap(800, 600);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, 800, 600));
            Pen pen = new Pen(LineColor, LineWidth);
            Rectangle rect = new Rectangle(100, 100, 100, 100);
            GraphicsPath path = CreateRoundedRectanglePath(rect, 8);
            g.DrawPath(pen, path);
            g.Dispose();
            return bmp;
        }

        internal static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        }
    }
}
