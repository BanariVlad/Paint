using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Paint
{
    public class Draw
    {
        private static string type = "pencil";
        private static PictureBox box = new PictureBox{Dock = DockStyle.Fill, Location = new Point(0 ,24), Size = new Size(1920, 1080)};
        private Color paintColor = Color.Black;
        private Pen pen = new Pen(Color.Black, 5);
        private bool draw;
        private int x;
        private int y;
        private int weight = 5;
        
        private void Drawing(MouseEventArgs e)
        {
            draw = true;
            x = e.X;
            y = e.Y;
        }

        public void SetWeight(int value)
        {
            weight = value;
            pen.Width = value;
        }

        private void SetFigure(MouseEventArgs e)
        {
            draw = false;
            if (type == "line")
            {
                var g = box.CreateGraphics();
                g.DrawLine(new Pen(new SolidBrush(paintColor), weight), new Point(x ,y), new Point(e.X, e.Y));
                g.Dispose();
            }
        }

        private void DrawFigure(MouseEventArgs e)
        {
            if (draw)
            {
                var g = box.CreateGraphics();
                switch (type)
                {
                    case "square":
                        g.FillRectangle(new SolidBrush(paintColor), x, y, e.X -x, e.Y -y);
                        break;
                    case "ellipse":
                        g.FillEllipse(new SolidBrush(paintColor), x, y, e.X -x, e.Y -y);
                        break;
                    case "pencil":
                        g.FillEllipse(new SolidBrush(paintColor), e.X - x + x, e.Y - y + y, weight, weight);
                        break;
                    case "erase":
                        g.FillEllipse(new SolidBrush(box.BackColor), e.X - x + x, e.Y - y + y, weight, weight);
                        break;
                }
                g.Dispose();
            }
        }

        public void SaveImage()
        {
            var bitmap = new Bitmap(box.Width, box.Height);
            var g = Graphics.FromImage(bitmap);
            var rectangle = box.RectangleToScreen(box.ClientRectangle);
            g.CopyFromScreen(rectangle.Location, Point.Empty, box.Size);
            g.Dispose();
            var dialog = new SaveFileDialog();
            dialog.Filter = @"Png files|*.png|jpeg files|*.jpg|bitmaps|*.bmp";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.FileName.Contains(".jpg"))
                {
                    bitmap.Save(dialog.FileName, ImageFormat.Jpeg);
                } else if (dialog.FileName.Contains(".png"))
                {
                    bitmap.Save(dialog.FileName, ImageFormat.Png);
                } else if (dialog.FileName.Contains(".bmp"))
                {
                    bitmap.Save(dialog.FileName, ImageFormat.Bmp);
                }
            }

        }

        public void OpenImage()
        {
           var dialog = new OpenFileDialog();
           dialog.Filter = @"Png files|*.png|jpeg files|*.jpg|bitmaps|*.bmp";
           if (dialog.ShowDialog() == DialogResult.OK)
           {
               box.Image = (Image) Image.FromFile(dialog.FileName).Clone();
           }
        }

        private void SetColor(Button btn)
        {
            var color = new ColorDialog();
            if (color.ShowDialog() == DialogResult.OK)
            {
                paintColor = color.Color;
                btn.BackColor = color.Color;
                pen.Color = color.Color;
            }
        }


        public void GenerateMenu(Form form)
        {
            var buttonsSize = new Size(35, 30);
            var panel = new Panel{Size = new Size(40, 420), Dock = DockStyle.Left, Location = new Point(0, 24), TabIndex = 1};
            var pencil = new Button{Size = buttonsSize, Text = @"✏", Location = new Point(3, 3)};
            pencil.Click += (obj, e) => type = "pencil";
            var line = new Button{Size = buttonsSize, Text = @"➖", Location = new Point(3, 93)};
            line.Click += (obj, e) => type = "line"; 
            var square = new Button{Size = buttonsSize, Text = @"⬜", Location = new Point(3, 33)};
            square.Click += (obj, e) => type = "square";
            var ellipse = new Button{Size = buttonsSize, Text = @"⚪", Location = new Point(3, 63)};
            ellipse.Click += (obj, e) => type = "ellipse";
            var erase = new Button{Size = buttonsSize, Text = @"🧽", Location = new Point(3, 123)};
            erase.Click += (obj, e) => type = "erase";
            var color = new Button{Size = buttonsSize, Text = @"", Location = new Point(3, 153), BackColor = Color.Black};
            color.Click += (obj, e) => SetColor(color);
            var clearAll = new Button{Size = buttonsSize, Text = @"C", Location = new Point(3, 183)};
            clearAll.Click += (obj, e) => box.Image = null;
            box.MouseDown += (sender, e) => Drawing(e);
            box.MouseMove += (sender, e) => DrawFigure(e);
            box.MouseUp += (sender, e) => SetFigure(e);
            panel.Controls.Add(pencil);
            panel.Controls.Add(square);
            panel.Controls.Add(ellipse);
            panel.Controls.Add(line);
            panel.Controls.Add(erase);
            panel.Controls.Add(color);
            panel.Controls.Add(clearAll);
            form.Controls.Add(panel);
            form.Controls.Add(box);
        }
    }
}