using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;


namespace Pagatpatan_DIP2
{
    public partial class Form1 : Form
    {
        private Bitmap greenScreenImage;
        private Bitmap backgroundImage;
        public Form1()
        {
            InitializeComponent();
            this.Text = "Act_2 Image Subtraction Pagatpatan";
        }


        private void btnToggleCamera_Click(object sender, EventArgs e)
        {

        }

        private void ResizeImageToFitPictureBox(Bitmap originalImage, PictureBox pictureBox)
        {
          
            float aspectRatio = (float)originalImage.Width / originalImage.Height;

            // new picturebox width and height
            int newWidth = pictureBox.Width;
            int newHeight = (int)(newWidth / aspectRatio);

            
            if (newHeight > pictureBox.Height)
            {
                newHeight = pictureBox.Height;
                newWidth = (int)(newHeight * aspectRatio);
            }

            Bitmap resizedImage = new Bitmap(originalImage, new Size(newWidth, newHeight));

            pictureBox.Image = resizedImage;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                greenScreenImage = new Bitmap(openFileDialog.FileName);
                ResizeImageToFitPictureBox(greenScreenImage, pictureBox1);
            }
        }

        private void btnLoadBackground_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                backgroundImage = new Bitmap(openFileDialog.FileName);
                ResizeImageToFitPictureBox(backgroundImage, pictureBox2);
            }
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            if (greenScreenImage == null || backgroundImage == null)
            {
                MessageBox.Show("Please upload both images before subtracting.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Bitmap resultImage = SubtractImages(greenScreenImage, backgroundImage);
            ResizeImageToFitPictureBox(resultImage, pictureBox3);
        }

        private Bitmap SubtractImages(Bitmap greenScreenImage, Bitmap backgroundImage)
        {
            Bitmap resultImage = new Bitmap(greenScreenImage.Width, greenScreenImage.Height);

            for (int x = 0; x < greenScreenImage.Width; x++)
            {
                for (int y = 0; y < greenScreenImage.Height; y++)
                {
                    Color greenScreenPixel = greenScreenImage.GetPixel(x, y);
                    Color backgroundPixel = backgroundImage.GetPixel(x, y);

                    // 0,0,255
                    if (greenScreenPixel.R != 0 || greenScreenPixel.G != 255 || greenScreenPixel.B != 0)
                    {
                        resultImage.SetPixel(x, y, greenScreenPixel);
                    }
                    else
                    {
                        resultImage.SetPixel(x, y, backgroundPixel);
                    }
                }
            }

            return resultImage;
        }
    }
}
