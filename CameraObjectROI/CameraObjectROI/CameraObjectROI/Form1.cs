using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace CameraObjectROI
{
    public partial class Form1 : Form
    {
        VideoCapture webcam,webcam2;
        bool touch = true ;
        int BTheshold, cx, cy, x_mm, y_mm;
        Mat camImage = new Mat();
        Mat BlobImage = new Mat();
        Mat BlobGray = new Mat();
        Mat CannyImage = new Mat();
        Mat DrawImg = new Mat();
        Mat Invert = new Mat();
        Mat openingimg = new Mat();
        Mat BlueDst = new Mat();
        Mat ROI = new Mat();
        Mat Binaryimage = new Mat();
        string path = @"c:\Webcamimgae";

        private void Form1_Load(object sender, EventArgs e)
        {
           
            webcam2 = new VideoCapture(0);
            webcam = new VideoCapture(0);
            Application.Idle += camera;
        }

        private void imageBox1_MouseClick(object sender, MouseEventArgs e)
        {
            cx = (int)(e.Location.X / imageBox1.ZoomScale/3);
            cy = (int)(e.Location.Y / imageBox1.ZoomScale/2.25);
            touch = false;
            Emgu.CV.Features2D.SimpleBlobDetector detector = new Emgu.CV.Features2D.SimpleBlobDetector();
            MKeyPoint[] keypoints = detector.Detect(openingimg);
            camImage.CopyTo(DrawImg);
            DrawImg.SetTo(new MCvScalar(255, 255, 255,0));
            //CvInvoke.Threshold(openingimg, openingimg, 127, 255, ThresholdType.BinaryInv);
            findcontours.boundingbox(camImage, openingimg,DrawImg, cx, cy);//改成drawimg show in other window
            //DrawImg.Save("C://Users//aup67//OneDrive//桌面//fishROI.jpg");
            
            //CvInvoke.Imshow("test", DrawImg);
            String filename = "pic" + DateTime.Now.ToString("HHmmss") + ".jpg";
            //image.Save("C:\\Webcamimgae\\" + filename, System.Drawing.Imaging.ImageFormat.Jpeg);
            imageBox1.Image = DrawImg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            touch = true;

        }

        private void imageBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int x = (int)(e.Location.X / imageBox1.ZoomScale);
            int y = (int)(e.Location.Y / imageBox1.ZoomScale);
            label1.Text = x.ToString();
            label2.Text = y.ToString();
        }

        private void testmode_CheckedChanged(object sender, EventArgs e)
        {
           
                label1.Visible = testmode.Checked;
                label2.Visible = testmode.Checked;
                label3.Visible = testmode.Checked;
                label4.Visible = testmode.Checked;
                label5.Visible = testmode.Checked;
                label6.Visible = testmode.Checked;
                label7.Visible = testmode.Checked;
                label8.Visible = testmode.Checked;
                imageBox2.Visible = testmode.Checked;
                imageBox3.Visible = testmode.Checked;
                trackBar1.Visible = testmode.Checked;
                trackBar2.Visible = testmode.Checked;
                trackBar3.Visible = testmode.Checked;
                trackBar4.Visible = testmode.Checked;
                trackBar5.Visible = testmode.Checked;
                trackBar6.Visible = testmode.Checked;
                button2.Visible = testmode.Checked;
            
        }

        private void imageBox1_Click(object sender, EventArgs e)
        {

        }

        private void imageBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Mat stitch = new Mat();
            Mat cam1 = webcam.QueryFrame();
            Mat cam2 = webcam2.QueryFrame();
            
            stitch = Stitch.combineimage(cam1,cam2);
            CvInvoke.Imshow("stitch", stitch);
        }

        void camera(object sender, EventArgs e)
        {
            ScalarArray Low, high;
            if (true)
            {
                camImage = webcam.QueryFrame();
                Mat camimage2 = webcam2.QueryFrame();
                camImage.CopyTo(BlobImage);//color image
                CvInvoke.MedianBlur(BlobImage, BlobImage, 3);
                CvInvoke.CvtColor(BlobImage, BlobImage, ColorConversion.Bgr2Hsv);
                if (testmode.Checked == true)
                {
                    Low = new ScalarArray(new MCvScalar(trackBar1.Value, trackBar2.Value, trackBar3.Value));
                    high = new ScalarArray(new MCvScalar(trackBar4.Value, trackBar5.Value, trackBar6.Value));
                    label3.Text = trackBar1.Value.ToString();
                    label4.Text = trackBar2.Value.ToString();
                    label5.Text = trackBar3.Value.ToString();
                    label6.Text = trackBar4.Value.ToString();
                    label7.Text = trackBar5.Value.ToString();
                    label8.Text = trackBar6.Value.ToString();
                }
                else
                {
                    Low = new ScalarArray(new MCvScalar(86, 0, 0));
                    high = new ScalarArray(new MCvScalar(255, 170, 170));
                    //Low = new ScalarArray(new MCvScalar(trackBar1.Value, trackBar2.Value, trackBar3.Value));
                    //high = new ScalarArray(new MCvScalar(trackBar4.Value, trackBar5.Value, trackBar6.Value));
                }
                

                CvInvoke.InRange(BlobImage,Low,high,BlueDst);
                Size Gsize = new System.Drawing.Size(5, 5);
                CvInvoke.GaussianBlur(BlobImage, BlobImage, Gsize, 1);
                #region Morphology
                Size ksize = new System.Drawing.Size(3, 3);
                Point anchor = new Point(-1, -1);
                Mat element = CvInvoke.GetStructuringElement(ElementShape.Rectangle, ksize, anchor);
                CvInvoke.Erode(BlueDst, openingimg, element, anchor, 1, BorderType.Default, new MCvScalar(0, 0, 0));
                CvInvoke.Dilate(openingimg, openingimg, element, anchor, 2, BorderType.Default, new MCvScalar(0, 0, 0));
                #endregion

                imageBox1.Image = camImage;
                imageBox2.Image = camimage2;
                imageBox3.Image = openingimg;
            }
            #region release memory
            //BlobGray.Dispose();
            //CannyImage.Dispose();
            //DrawImg.Dispose();
            //Invert.Dispose();
            //openingimg.Dispose();
            //BlueDst.Dispose();
            //ROI.Dispose();
            //Binaryimage.Dispose();
            //camImage.Dispose();
            //BlobImage.Dispose();
            #endregion
        }

        public Form1()
        {
            InitializeComponent();
            imageBox1.Size = new Size(1920, 1080);
            imageBox1.Location = new Point(0, 0);

            bool visibility = testmode.Checked;
                label1.Visible = visibility;
                label2.Visible = visibility;
                label3.Visible = visibility;
                label4.Visible = visibility;
                label5.Visible = visibility;
                label6.Visible = visibility;
                label7.Visible = visibility;
                label8.Visible = visibility;
                imageBox2.Visible = visibility;
                imageBox3.Visible = visibility;
                trackBar1.Visible = visibility;
                trackBar2.Visible = visibility;
                trackBar3.Visible = visibility;
                trackBar4.Visible = visibility;
                trackBar5.Visible = visibility;
                trackBar6.Visible = visibility;
                button2.Visible = visibility;

        }
    }
}
