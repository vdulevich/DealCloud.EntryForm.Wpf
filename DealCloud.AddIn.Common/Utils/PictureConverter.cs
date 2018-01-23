using System;
using System.Drawing;
using System.Windows.Forms;
using stdole;

namespace DealCloud.AddIn.Common.Utils
{
    public class PictureConverter : AxHost
    {
        private PictureConverter() : base(String.Empty) { }

        public static IPictureDisp ImageToPictureDisp(Image image)
        {
            return (IPictureDisp)GetIPictureDispFromPicture(image);
        }

        public static IPictureDisp IconToPictureDisp(Icon icon)
        {
            return ImageToPictureDisp(icon.ToBitmap());
        }

        public static Image PictureDispToImage(IPictureDisp picture)
        {
            return GetPictureFromIPicture(picture);
        }
    }
}
