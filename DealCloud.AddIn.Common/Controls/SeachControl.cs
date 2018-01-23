using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls
{
    public partial class SeachControl : UserControl
    {
        public SeachControl()
        {
            InitializeComponent();
            Radius = -1;
            txbSearch.Width = 0;
        }

        public TextBoxExt TextBox => txbSearch;

        public int Radius { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Radius > 0)
            {
                DrawRoundRect(e.Graphics, new Pen(Color.Gray, 1.5f), e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1, Radius);
            }
            else if (Radius == 0)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Gray, 1.5f), e.ClipRectangle);
            }
        }

        public void DrawRoundRect(Graphics g, Pen p, float X, float Y, float width, float height, float radius)
        {
            using (GraphicsPath gp = new GraphicsPath())
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                gp.AddLine(X, Y + height - (radius * 2), X, Y + radius);
                gp.AddArc(X, Y, radius * 2, radius * 2, 180, 90);

                gp.AddLine(X + radius, Y, X + width - (radius * 2), Y);
                gp.AddArc(X + width - (radius * 2), Y, radius * 2, radius * 2, 270, 90);

                gp.AddLine(X + width, Y + radius, X + width, Y + height - (radius * 2));
                gp.AddArc(X + width - (radius * 2), Y + height - (radius * 2), radius * 2, radius * 2, 0, 90);

                gp.AddLine(X + width - (radius * 2), Y + height, X + radius, Y + height);
                gp.AddArc(X, Y + height - (radius * 2), radius * 2, radius * 2, 90, 90);

                gp.CloseFigure();
                g.DrawPath(p, gp);
            }
        }
    }
}
