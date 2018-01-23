using System;
using System.Drawing;
using System.Windows.Forms;
using DealCloud.AddIn.Common.Utils;

namespace DealCloud.AddIn.Common.Controls
{
    public enum ProgressBarDisplayText
    {
        Percentage,
        CustomText
    }

    public class ProgressBarExt : ProgressBar
    {
        public ProgressBarDisplayText DisplayStyle { get; set; }

        public string CustomText { get; set; }


        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WinApi.WM_PAINT:
                    int mPercent = Convert.ToInt32((Convert.ToDouble(Value)/Convert.ToDouble(Maximum))*100);
                    dynamic flags = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter |
                                    TextFormatFlags.SingleLine | TextFormatFlags.WordEllipsis;

                    using (Graphics g = Graphics.FromHwnd(Handle))
                    {
                        switch (DisplayStyle)
                        {
                            case ProgressBarDisplayText.CustomText:
                                TextRenderer.DrawText(g, CustomText,
                                    this.Font,
                                    new Rectangle(0, 0, this.Width, this.Height), ForeColor, flags);
                                break;
                            case ProgressBarDisplayText.Percentage:
                                TextRenderer.DrawText(g, $"{mPercent}%",
                                    this.Font,
                                    new Rectangle(0, 0, this.Width, this.Height), ForeColor, flags);
                                break;
                        }
                    }

                    break;
            }
        }
    }

    public static class ProgressBarExtension
    {
        public static void SetProgressNoAnimation(this ProgressBar pb, int value)
        {
            // To get around this animation, we need to move the progress bar backwards.
            if (value == pb.Maximum)
            {
                // Special case (can't set value > Maximum).
                pb.Value = value; // Set the value
                pb.Value = value - 1; // Move it backwards
            }
            else
            {
                pb.Value = value + 1; // Move past
            }
            pb.Value = value; // Move to correct value
        }
    }
}
