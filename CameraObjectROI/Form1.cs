using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace CameraObjectROI
{
    public partial class Form1 : Form
    {
        Rectangle resolution = Screen.PrimaryScreen.Bounds;
        VideoCapture In_cam, Out_cam;
        int counter = 0;
        bool click = false;
        #region private variable
        HandleServerEye m_hdl_eye = null;
        HandleServerObj m_hdl_obj = null;
        delegate void SetTextCallback(string msg);
        delegate void SetTextCallback2(string msg);
        delegate void SetPicbxCallback(int oNum, int[] objIdx, double[] wPos);
        string filename = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\coco.names";
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        string[] lines;
        string str;
        #endregion
        #region Image variables
        int cx, cy, x_mm, y_mm;
        Mat camImage = new Mat();
        Mat camimage2 = new Mat();
        Mat DrawImg = new Mat();
        Mat ROI = new Mat();
        Mat Div1 = new Mat();
        Mat stereo = new Mat();
        Mat Div2 = new Mat();
        string path = @"c:\Webcamimgae";
        #endregion
        #region private method
        DataTable dt = new DataTable("Object");
        
        public void msg(string mesg)
        {
            tbEye.Text = tbEye.Text + Environment.NewLine + " >> " + mesg;
        }

        static void CloseExe(string exeName)
        {

            try
            {
                foreach (Process proc in Process.GetProcessesByName(exeName))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        static void OpenExe(string exeName)
        {
            Process eyeTracking = new Process();
            eyeTracking.StartInfo.FileName = exeName + ".exe";
            eyeTracking.StartInfo.UseShellExecute = false;
            eyeTracking.StartInfo.RedirectStandardOutput = true;
            eyeTracking.Start();
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            In_cam = new VideoCapture(0);
            Out_cam = new VideoCapture(0);
            Application.Idle += camera;
        }

        private void imageBox1_MouseClick(object sender, MouseEventArgs e)
        {

            sw.Reset();//碼表歸零
            sw.Start();//碼表開始計時
            counter = 0;
            timer1.Enabled = false;
            timer1.Enabled = true;
            cx = (int)(e.Location.X / imageBox1.ZoomScale / resolution.Width * 640);
            cy = (int)(e.Location.Y / imageBox1.ZoomScale / resolution.Height * 480);
            Bitmap bmp = new Bitmap(camImage.Bitmap);
            m_hdl_obj.GetOneObjReport(bmp);
            imageBox1.Image = camImage;
            click = true;
            sw.Stop();//碼錶停止
        }
        private void imageBox1_MouseMove(object sender, MouseEventArgs e)//pixel2mm
        {
            int x = (int)(e.Location.X / imageBox1.ZoomScale);
            int y = (int)(e.Location.Y / imageBox1.ZoomScale);
            double[] xymm = pixel2mm.pix2mm(x,y,702.65,397.3,1366,768);
            label1.Text = "X : " + x.ToString();
            label2.Text = "Y : " + y.ToString();
            label9.Text = "X : " + xymm[0].ToString() + "mm";
            label10.Text = "Y : " + xymm[1].ToString() + "mm";
        }
        private void testmode_CheckedChanged(object sender, EventArgs e)
        {
            label1.Visible = testmode.Checked;
            label2.Visible = testmode.Checked;
            label9.Visible = testmode.Checked;
            label10.Visible = testmode.Checked;
            label11.Visible = testmode.Checked;
            imageBox2.Visible = testmode.Checked;
            imageBox3.Visible = testmode.Checked;
            Stitch.Visible = testmode.Checked;

        }
        private void timer1_Tick(object sender, EventArgs e)
        {

            //if (counter >= 25)
            //{
            //    // Exit loop code.
            //    timer1.Enabled = false;
            //    DrawImg.SetTo(new MCvScalar(255, 255, 255, 0));
            //    counter = 0;
            //}
            //else
            //{
            //        camImage.CopyTo(DrawImg);
            //        DrawImg.SetTo(new MCvScalar(255, 255, 255, 0));
            //    //CvInvoke.Threshold(openingimg, openingimg, 127, 255, ThresholdType.BinaryInv);
            //    /* findcontours.boundingbox(camImage, camImage, DrawImg, cx, cy);*///改成drawimg show in other window
            //                                                                       //DrawImg.Save("C://Users//aup67//OneDrive//桌面//fishROI.jpg");
            //    Bitmap bmp = new Bitmap(camImage.Bitmap);
            //    m_hdl_obj.GetOneObjReport(bmp);
            //    imageBox1.Image = camImage;
            //    counter = counter + 1;
            //    label11.Text = "Procedures Run: " + counter.ToString();
            //}
        }
        private void Stitch_Click(object sender, EventArgs e)
        {
            Mat stitch = new Mat();
            Mat cam1 = In_cam.QueryFrame();
            Mat cam2 = Out_cam.QueryFrame();

            stitch = CameraObjectROI.Stitch.combineimage(cam1, cam2);
            CvInvoke.Imshow("stitch", stitch);
        }

        void camera(object sender, EventArgs e)
        {

            camImage = In_cam.QueryFrame();
            CvInvoke.Flip(camImage, camImage, FlipType.Horizontal);
            camimage2 = Out_cam.QueryFrame();

            if (came.Checked)
            {
                DivImg.DivImage(camImage, out Div1, out Div2);

                try
                {
                    //camImage = CameraObjectROI.Stitch.combineimage(Div1, Div2);
                    int mindisparity = 0;
                    int ndisparities = 160;
                    int SADWondowSize = 11;
                    int cn = 1;                    
                    int p1 = 8* SADWondowSize * SADWondowSize*cn;
                    int p2 = 32* SADWondowSize * SADWondowSize*cn;
                    int MaxDiff = 1;
                    int PreFilterCap = 63;
                    int uniquenessRatio= 10;
                    int speckleWindowSize = 100;
                    int speckleRange = 32;
                    Emgu.CV.StereoSGBM SGBM = new StereoSGBM(mindisparity, ndisparities, SADWondowSize,
                        p1,p2,MaxDiff,PreFilterCap,uniquenessRatio,speckleWindowSize,speckleRange,StereoSGBM.Mode.SGBM);
                    UMat left = new UMat(); UMat right = new UMat();
                    CvInvoke.CvtColor(Div1, left, ColorConversion.Bgr2Gray);
                    CvInvoke.CvtColor(Div2, right, ColorConversion.Bgr2Gray);
                    SGBM.Compute(left, right, stereo);
                    CvInvoke.Imshow("stereo", stereo);
                    SGBM.Dispose();
                }
                catch
                {
                    camImage = Div1;
                }

            }


            if (click == false)
            {
                imageBox1.Image = camImage;//camImage//DrawImg
                imageBox2.Image = camimage2;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Task.Run(() => {
                //1.Start Eye Report
                m_hdl_eye.StartEyeReport();
            });
        }

        public void ShowObjMsg(string msg)
        {
            //判斷這個TextBox的物件是否在同一個執行緒上
            if (tbObj.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作!!
                SetTextCallback2 d = new SetTextCallback2(ShowObjMsg);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                //表示在同一個執行緒上了，所以可以正常的呼叫到這個TextBox物件mj
                tbObj.Text = tbObj.Text + Environment.NewLine + " >> " + msg;
            }
        }
        public void ShowEyeMsg(string msg)
        {
            //判斷這個TextBox的物件是否在同一個執行緒上
            if (tbEye.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作!!
                SetTextCallback d = new SetTextCallback(ShowEyeMsg);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                //表示在同一個執行緒上了，所以可以正常的呼叫到這個TextBox物件
                tbEye.Text = tbEye.Text + Environment.NewLine + " >> " + msg;
            }
        }
        public void ShowObjImg(int oNum, int[] objIdx, double[] objInfo)
        {
            //判斷這個TextBox的物件是否在同一個執行緒上
            if (imageBox1.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作!!
                SetPicbxCallback del = new SetPicbxCallback(ShowObjImg);
                this.Invoke(del, new object[] { oNum, objIdx, objInfo });
            }
            else
            {
                camImage.CopyTo(DrawImg);
                if (showcheck.Checked)
                {
                    DrawImg.SetTo(new MCvScalar(255, 255, 255, 0));
                }

                for (int i = 0; i < oNum; i++)
                {
                    int x = (int)(objInfo[i * 5]);
                    int y = (int)(objInfo[i * 5 + 1]);
                    int width = (int)(objInfo[i * 5 + 2]);
                    int height = (int)(objInfo[i * 5 + 3]);
                    if (x < cx && cx < x + width && y < cy && cy < y + height && objInfo[i * 5 + 4] > 0.5 && lines[objIdx[i]] != "refrigerator")
                    {
                        if (lines != null)
                        {
                            str = lines[objIdx[i]] + ": " + objInfo[i * 5 + 4].ToString("0.000");
                        }
                        else
                        {
                            str = "object";
                        }

                        Rectangle ROI = new Rectangle(x, y, width, height);
                        CvInvoke.Rectangle(DrawImg, ROI, new MCvScalar(0, 0, 255), 3);
                        CvInvoke.PutText(DrawImg, str, new Point(x, y - 15), FontFace.HersheyComplex, 0.7, new MCvScalar(0, 0, 255), 2);

                    }
                }

                imageBox1.Image = DrawImg;
                // DrawImg.Dispose();
            }

        }
        public void EyeTrackingComplete(int pNum, double[] ePos)
        {
            string eyeResult = "pNum:" + pNum.ToString() + "\n";
            for (int i = 0; i < pNum; i++)
            {
                eyeResult += ("(" + (ePos[i * 3].ToString("#0.00")) + "," + (ePos[i * 3 + 1].ToString("#0.00")) + "," + (ePos[i * 3 + 2].ToString("#0.00")) + ")\n");
            }
            ShowEyeMsg(eyeResult);


            //Transport Fcn
            int oNum = pNum;
            double[] oPos = new double[oNum * 3];
            for (int i = 0; i < ePos.Length; i++)
            {
                oPos[i] = ePos[i];
            }
            //
            //m_hdl_obj.GetOneObjReport(oNum, oPos);

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_hdl_eye != null)
            {
                m_hdl_eye.Close();
            }
            if (m_hdl_obj != null)
            {
                m_hdl_obj.Close();
            }

        }
        private void button4_Click(object sender, EventArgs e)
        {
            click = false;
        }

        
        public Form1()
        {
            InitializeComponent();
            imageBox1.Size = new Size(resolution.Width, resolution.Height);
            //imageBox1.Size = new Size(1920, 1080);
            imageBox1.Location = new Point(0, 0);
            
            try
            {

                //msg("Client EyeTracking Started");
                //m_hdl_eye = new HandleServerEye("127.0.0.1", 8888);
                //m_hdl_eye.registerCallbackFcn(EyeTrackingComplete, ShowEyeMsg);
                //label1.Text = "Client Socket Program : EyeTracking - Server Connected ...";

                msg("Client ObjDetection Started");
                m_hdl_obj = new HandleServerObj("127.0.0.1", 9999);
                m_hdl_obj.registerCallbackFcn(ShowObjMsg);
                m_hdl_obj.registerCallbackFcn(ShowObjImg);
                label2.Text = "Client Socket Program : ObjDetection - Server Connected ...";


            }
            catch
            {
                MessageBox.Show("Client Socket Program : EyeTracking or ObjDetection - Server Connected fail");
            }

            bool visibility = testmode.Checked;
            label1.Visible = visibility;
            label2.Visible = visibility;
            label9.Visible = visibility;
            label10.Visible = visibility;
            label11.Visible = visibility;
            imageBox2.Visible = visibility;
            imageBox3.Visible = visibility;
            Stitch.Visible = visibility;
            button3.Visible = visibility;
            tbEye.Visible = visibility;
            tbObj.Visible = visibility;
            try
            { lines = File.ReadAllLines(filename); }
            catch
            { lines = null; }
           
        }
    }
}
