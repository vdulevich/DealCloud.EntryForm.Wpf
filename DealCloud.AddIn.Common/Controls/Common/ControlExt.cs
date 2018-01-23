using System.Drawing;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls.Common
{
    public static class ControlExt
    {
        public static Point GetFormLocation(this Control control)
        {
            return control.FindForm().PointToClient(control.Parent.PointToScreen(control.Location));
        }

        public static Point GetFormTooltipLocation(this Control control)
        {
            return control.FindForm().PointToClient(control.Parent.PointToScreen(new Point(control.Location.X + control.Width + 5, control.Location.Y)));
        }
    }
}
