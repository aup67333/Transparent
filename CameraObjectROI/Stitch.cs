using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Stitching;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace CameraObjectROI
{
    class Stitch
    {
        public static Mat combineimage(Mat img1, Mat img2)
        {
            Mat result = new Mat();
            Image<Bgr, byte>[] sourceImages = new Image<Bgr, byte>[2];//number of camera
            sourceImages[0] = img1.ToImage<Bgr, byte>();
            sourceImages[1] = img1.ToImage<Bgr, byte>();
            try
            {
                using (Stitcher stitcher = new Stitcher(true))
                {
                    using (VectorOfMat vm = new VectorOfMat())
                    {
                        vm.Push(sourceImages);
                        Stitcher.Status stitchStatus = stitcher.Stitch(vm, result);
                        if (stitchStatus != Stitcher.Status.Ok)
                        {
                            //MessageBox.Show(String.Format("Stiching Error: {0}", stitchStatus));
                            throw new System.ArgumentException(String.Format("Stiching Error: {0}", stitchStatus), "original");
                        }
                    }
                }
            }
            finally
            {
                foreach (Image<Bgr, Byte> img in sourceImages)
                {
                    img.Dispose();
                }
            }

            return result;
        }
    }
}
