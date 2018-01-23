using System;
using System.Windows.Forms;
using DealCloud.AddIn.Common.Controls;

namespace DealCloud.AddIn.Common.Utils
{
    public class LoadingErrorFormExtUtil: BaseOverlayFormExtUtil
    {
        private LoadingErrorControl Control { get; set; }

        public event EventHandler RefreshClick;

        public LoadingErrorFormExtUtil(FormExt control) : base(control)
        {

        }

        public void Show(string text, bool showRefresh = false)
        {
            base.Show();
            Control.Show(text, showRefresh);
        }

        protected override Control GetControl(bool create = true)
        {
            if (Control == null && create)
            {
                Control = new LoadingErrorControl { Dock = DockStyle.Fill };
                Control.RefreshClick += RefreshClick;
                Form.Controls.Add(Control);
                Form.Controls.SetChildIndex(Control, 0);
            }
            return Control;
        }
    }
}
