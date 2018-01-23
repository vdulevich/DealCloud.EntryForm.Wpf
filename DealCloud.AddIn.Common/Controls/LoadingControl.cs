using System;
using System.Windows.Forms;
using DealCloud.AddIn.Common.Controls.Common;
using DealCloud.AddIn.Common.Utils;

namespace DealCloud.AddIn.Common.Controls
{
    public partial class LoadingControl : UserControlExt
    {
        public LoadingControl()
        {
            InitializeComponent();
            pbLoader.Size = pbLoader.Size.ScaleByDpi();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.Parent is Form)
                {
                    WinApi.ReleaseCapture();
                    WinApi.SendMessage(this.ParentForm.Handle, WinApi.WM_NCLBUTTONDOWN, WinApi.HT_CAPTION, 0);
                }
            }
            base.OnMouseDown(e);
        }
	}
}
