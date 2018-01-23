using System.Windows.Forms;
using DealCloud.AddIn.Common.Utils;

namespace DealCloud.AddIn.Common.Controls
{
    public class LoadingFormExtUtil : BaseOverlayFormExtUtil
    {
        private LoadingControl Control { get; set; }

        public LoadingFormExtUtil(FormExt control) : base(control)
        {

        }

        protected override Control GetControl(bool create = true)
        {
            if (Control == null && create)
            {
                Control = new LoadingControl {Dock = DockStyle.Fill};
                Form.Controls.Add(Control);
                Form.Controls.SetChildIndex(Control, 0);
            }
            return Control;
        }
    }
}
