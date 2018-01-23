using DealCloud.AddIn.Common.Properties;
using DealCloud.AddIn.Common.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls
{
    public partial class MessageDialog : FormExt
    {
        public event Action<FormExt> OkClick;

        public string Messsage
        {
            get { return mesageLbl.Text; }
            set { mesageLbl.Text = value; }
        }

        public string OkText
        {
            get { return continueBtn.Text; }
            set { continueBtn.Text = value; }
        }

        public string CancelText
        {
            get { return cancelBtn.Text; }
            set { cancelBtn.Text = value; }
        }

        public bool ShowOk
        {
            get { return continueBtn.Visible; }
            set { continueBtn.Visible = value; }
        }

        public bool ShowCancel
        {
            get { return cancelBtn.Visible; }
            set { cancelBtn.Visible = value; }
        }

        public bool OkActive
        {
            get { return ActiveControl == continueBtn; }
            set { ActiveControl = value ? (Control)continueBtn : (Control)cancelBtn; }
        }

        public bool AllowReporting
        {
            get { return reportButton.Visible; }
            set { reportButton.Visible = value; }
        }

        public MessageDialog()
        {
            InitializeComponent();
            ActiveControl = cancelBtn;
            this.Shown += messageDialog_Shown;
        }

        private void messageDialog_Shown(object sender, EventArgs e)
        {
            BringToFront();
        }

        public static DialogResult ShowMessage(string title, string message, Icon icon = null)
        {
            using (MessageDialog dialog = new MessageDialog() { Icon = icon })
            {
                dialog.Title = title;
                dialog.OkText = Resources.Ok;
                dialog.Messsage = message;
                dialog.ShowCancel = false;
                dialog.Resizable = false;
                return dialog.ShowHandledDialog();
            }
        }

        public static DialogResult ShowConfirmation(string title, string message, string okText = null, Icon icon = null)
        {
            using (MessageDialog dialog = new MessageDialog() { Icon = icon })
            {
                dialog.Title = title;
                dialog.OkText = okText ?? Resources.Ok;
                dialog.Messsage = message;
                dialog.Resizable = false;
                return dialog.ShowHandledDialog();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                OkClick?.Invoke(this);
            }
            catch (Exception)
            {

            }
        }

        private void lblMesage_MouseDown(object sender, MouseEventArgs e)
        {
            this.OnMouseDown(e);
        }

        private void messageDialog_Load(object sender, EventArgs e)
        {
            mesageLbl.MaximumSize = new Size(tableLayoutPanel1.Width - mesageLbl.Margin.Right - mesageLbl.Margin.Left, 9999);
        }

        private void reportButton_Click(object sender, EventArgs e)
        {
            ReportUtil.Instance.ShowReport();
        }
    }
}
