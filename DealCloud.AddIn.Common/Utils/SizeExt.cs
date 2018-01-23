using System;
using System.Drawing;

namespace DealCloud.AddIn.Common.Utils
{
    public static class SizeExt
    {
        public static Size ScaleByDpi(this Size size)
        {
            float s = GetScalingFactor();
            return new Size((int) (size.Width* s), (int) (size.Height* s));
        }

        public static Bitmap ScaleByDpi(this Bitmap bitmap)
        {
            return new Bitmap(bitmap, bitmap.Size.ScaleByDpi());
        }

        private static float GetScalingFactor()
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                return g.DpiX / 96.0f;
            }
        }
    }
}
