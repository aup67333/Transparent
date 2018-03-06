using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace CameraObjectROI
{
    class DivImg
    {
        public static void DivImage(Mat original, out Mat Diva1,out Mat Diva2)
        {
            int width = original.Width;
            Diva1 = new Mat(original, new Rectangle(0, 0, original.Width / 2, original.Height));
            Diva2 = new Mat(original, new Rectangle(original.Width / 2, 0, original.Width / 2, original.Height));
        }

    }

}