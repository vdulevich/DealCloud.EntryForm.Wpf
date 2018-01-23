using System;
using System.Windows.Forms;
using DealCloud.AddIn.Common.Controls;

namespace DealCloud.AddIn.Common.Utils
{
    public abstract class BaseOverlayFormExtUtil
    {
        protected FormExt Form { get; set; }

        protected abstract Control GetControl(bool create = true);

        protected BaseOverlayFormExtUtil(FormExt control)
        {
            Form = control;
        }

        public virtual void Show()
        {
            if (Form.InvokeRequired)
            {
                Form.BeginInvoke(new Action(Show));
                return;
            }
            GetControl().Visible = true;
        }

        public void Hide()
        {
            if (Form.InvokeRequired)
            {
                Form.BeginInvoke(new Action(Hide));
                return;
            }
            Control control = GetControl(false);
            if (control != null)
            {
                control.Visible = false;
            }
        }
    }
}
