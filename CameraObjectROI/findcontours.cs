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
    public class findcontours
    {
        public static void boundingbox(Mat original, Mat src, Mat draw, int cx, int cy)
        {
            Rectangle BoundingBox = new Rectangle();
            using (Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(src, contours, null, Emgu.CV.CvEnum.RetrType.List, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                int count = contours.Size;
                for (int i = 0; i < count; i++)
                {

                    using (Emgu.CV.Util.VectorOfPoint contour = contours[i])
                    {
                        var arr = contour.ToArray();
                        //Mat x = contour.GetOutputArray;
                        if (CvInvoke.ContourArea(contour, false) > 200)
                        {
                            BoundingBox = CvInvoke.BoundingRectangle(contour);
                            int Boxwidth = BoundingBox.Width;
                            int BoxHeight = BoundingBox.Height;
                            int BoxX = BoundingBox.X;
                            int BoxY = BoundingBox.Y;
                            if (BoxX < cx && cx < BoxX + Boxwidth && BoxY < cy && cy < BoxY + BoxHeight)//BoxX < cx  && cx < cx + Boxwidth  && BoxY < cy && cy < cy + BoxHeight
                            {
                                CvInvoke.Rectangle(draw, BoundingBox, new MCvScalar(0, 0, 255), 3);
                                Mat ROI = Crop_Frame.crop_color_frame(original, BoundingBox);
                                CvInvoke.PutText(draw, "Bottle", new Point(BoxX + Boxwidth+2, BoxY + BoxHeight / 2), FontFace.HersheyComplex, 1, new MCvScalar(0, 0, 255), 2);
                                //CvInvoke.Imshow("ROI", ROI);
                            }
                        }
                    }
                }
            }
        }
    }
}
