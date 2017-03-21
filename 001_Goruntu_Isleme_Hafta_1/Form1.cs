using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _001_Goruntu_Isleme_Hafta_1
{
    public partial class Form1 : Form
    {
        Bitmap myImage;
        public Form1()
        {
            InitializeComponent();
        }
        public void ChangeRGB(Bitmap myImage, Byte R, Byte G, Byte B, Boolean sign)
        {
            Color myColor;
            progressBar1.Visible = true;
            progressBar1.Maximum = myImage.Width * myImage.Height;
            for (int i = 0; i < myImage.Width - 1; i++)
            {
                for (int j = 0; j < myImage.Height - 1; j++)
                {
                    myColor = myImage.GetPixel(i, j);
                    if (sign == false)
                        myColor = Color.FromArgb(myColor.A, R - myColor.R, G - myColor.G, B - myColor.B);
                    else
                        myColor = Color.FromArgb(myColor.A, (R + myColor.R) % 255, (G + myColor.G) % 255, (B + myColor.B) % 255);
                    //myColor = Color.FromArgb(myColor.A, (byte)~myColor.R, (byte)~myColor.G, (byte)~myColor.B);
                    myImage.SetPixel(i, j, myColor);
                    if (i % 10 == 0)
                    {
                        progressBar1.Value = i * myImage.Height + j;
                        Application.DoEvents();
                    }
                }
            }
            pictureBox2.Image = myImage;
            progressBar1.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select an image";
            openFileDialog1.Filter = "Text Files (*.jpg)|*.jpg|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                myImage = new Bitmap(openFileDialog1.OpenFile());
                pictureBox1.Image = myImage;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangeRGB(myImage, 255, 255, 255, false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChangeRGB(myImage, Convert.ToByte(textBoxR.Text), Convert.ToByte(textBoxR.Text), Convert.ToByte(textBoxR.Text), true);
        }

        private void textBoxR_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBoxG_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBoxB_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            myImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
            pictureBox2.Image = myImage;
        }
        public static Bitmap RotateImg(Bitmap bmp, float angle, Color bkColor)
        {
            angle = angle % 360;
            if (angle > 180)
                angle -= 360;

            System.Drawing.Imaging.PixelFormat pf = default(System.Drawing.Imaging.PixelFormat);
            if (bkColor == Color.Transparent)
            {
                pf = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }

            float sin = (float)Math.Abs(Math.Sin(angle * Math.PI / 180.0)); // this function takes radians
            float cos = (float)Math.Abs(Math.Cos(angle * Math.PI / 180.0)); // this one too
            float newImgWidth = sin * bmp.Height + cos * bmp.Width;
            float newImgHeight = sin * bmp.Width + cos * bmp.Height;
            float originX = 0f;
            float originY = 0f;

            if (angle > 0)
            {
                if (angle <= 90)
                    originX = sin * bmp.Height;
                else
                {
                    originX = newImgWidth;
                    originY = newImgHeight - sin * bmp.Width;
                }
            }
            else
            {
                if (angle >= -90)
                    originY = sin * bmp.Width;
                else
                {
                    originX = newImgWidth - sin * bmp.Height;
                    originY = newImgHeight;
                }
            }

            Bitmap newImg = new Bitmap((int)newImgWidth, (int)newImgHeight, pf);
            Graphics g = Graphics.FromImage(newImg);
            g.Clear(bkColor);
            g.TranslateTransform(originX, originY); // offset the origin to our calculated values
            g.RotateTransform(angle); // set up rotate
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(bmp, 0, 0); // draw the image at 0, 0
            g.Dispose();

            return newImg;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            float aci = 0;
            if (!String.IsNullOrEmpty(textBoxAci.Text))
                aci = (float)Convert.ToDouble(textBoxAci.Text);
            pictureBox2.Image = RotateImg(myImage, aci, Color.Transparent);

        }

        private void textBoxAci_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
