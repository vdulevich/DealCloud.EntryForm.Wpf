using System;
using System.Drawing;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls
{
    public class BaseLoadingControlUtil
    {
        protected readonly Control _control;

        protected LoadingControl LoadingControl;

        public BaseLoadingControlUtil(Control control)
        {
            _control = control;
            ShowClose = true;
        }

        public bool Loading => LoadingControl != null && LoadingControl.Visible;

        public bool ShowClose { get; set; }

        public virtual void Show()
        {
            if (_control.InvokeRequired)
            {
                _control.BeginInvoke(new Action(Show));
                return;
            }
            if (LoadingControl == null)
            {
                LoadingControl = new LoadingControl ();
                _control.Controls.Add(LoadingControl);
                _control.Controls.SetChildIndex(LoadingControl, 0);
            }
            LoadingControl.Size = GetSize();
            LoadingControl.Location = GetLocation();
            LoadingControl.Visible = true;
        }

        public virtual Size GetSize()
        {
            return _control.Size;
        }

        public virtual Point GetLocation()
        {
            return new Point(0, 0);
        }

        public virtual void Hide()
        {
            if (_control.InvokeRequired)
            {
                _control.BeginInvoke(new Action(Hide));
                return;
            }
            if (LoadingControl != null)
            {
                LoadingControl.Visible = false;
            }
        }
    }
}
