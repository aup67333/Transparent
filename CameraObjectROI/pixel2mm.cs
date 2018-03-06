using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraObjectROI
{
    class pixel2mm
    {

        public static double[] pix2mm(float x, float y, double res_width, double res_height, float screen_width, float screen_height)
        {
            double x_mm, y_mm;
            x_mm = x / res_width * screen_width;
            y_mm = y / res_height * screen_height;
            double[] XY_MM = new double[] { x_mm, y_mm };
            return XY_MM;
        }
        }
}
