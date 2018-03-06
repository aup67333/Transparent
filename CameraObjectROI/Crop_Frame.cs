using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace CameraObjectROI
{
    public class Crop_Frame
    {
        public static Mat crop_color_frame(Mat input, Rectangle crop_region)
        {
            /*
             * TODO(Ahmed): Figure out why I had to copy this into this class.
             * */
            Image<Bgr, Byte> buffer_im = input.ToImage<Bgr, Byte>();
            buffer_im.ROI = crop_region;

            Image<Bgr, Byte> cropped_im = buffer_im.Copy();


            return cropped_im.Mat;

        }
    }

}
