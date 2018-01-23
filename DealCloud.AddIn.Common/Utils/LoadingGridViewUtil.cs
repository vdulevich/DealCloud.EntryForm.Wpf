using System.Drawing;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls
{
    public class LoadingGridViewUtil : BaseLoadingControlUtil
    {
        private int BorderWidth
        {
            get
            {
                switch (DataGridView.BorderStyle)
                {
                    case BorderStyle.Fixed3D:
                        return SystemInformation.Border3DSize.Width;
                    case BorderStyle.FixedSingle:
                        return SystemInformation.BorderSize.Width;
                    default:
                        return 0;
                }
            }
        }

        private int BorderHeight
        {
            get
            {
                switch (DataGridView.BorderStyle)
                {
                    case BorderStyle.Fixed3D:
                        return SystemInformation.Border3DSize.Height;
                    case BorderStyle.FixedSingle:
                        return SystemInformation.BorderSize.Height;
                    default:
                        return 0;
                }
            }
        }

        public DataGridView DataGridView => (DataGridView)_control;

        public LoadingGridViewUtil(DataGridView control) : base(control)
        {

        }

        public override Size GetSize()
        {
            return new Size(DataGridView.Size.Width - 2 * BorderWidth, DataGridView.Size.Height - DataGridView.ColumnHeadersHeight - 2 * BorderHeight);
        }

        public override Point GetLocation()
        {
            return new Point(BorderWidth, DataGridView.ColumnHeadersHeight + BorderHeight);
        }

        public override void Show()
        {
            base.Show();
            LoadingControl.BackColor = DataGridView.BackgroundColor;
        }
    }
}
