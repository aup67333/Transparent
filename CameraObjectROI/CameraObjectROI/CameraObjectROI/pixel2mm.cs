using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraObjectROI
{
    class pixel2mm
    {

        public static int[] pix2mm(int x, int y, int res_width, int res_height, int screen_width, int screen_height)
        {
            int x_mm, y_mm;
            x_mm = x / res_width * screen_width;
            y_mm = y / res_height * screen_height;
            int[] XY_MM = new int[] { x_mm, y_mm };
            return XY_MM;
        }
        }
}
