using DealCloud.AddIn.Common.Utils;

namespace DealCloud.AddIn.Common.Controls
{
    partial class ReportDialog : FormExt
    {
        public string Message
        {
            get { return messageTextBox.Text; }
        }

        public ReportDialog()
        {
            InitializeComponent();
        }

        private void submitButton_Click(object sender, System.EventArgs e)
        {
            ReportUtil.Instance.Report(Message);
        }

        private void detailsLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            new ReportPreviewDialog()
            {
                Icon = Icon,
                Message = Message,
                SystemInfo = ReportUtil.Instance.GetSystemInfo(),
                LogFiles = ReportUtil.Instance.GetLogFiles(),
            }.ShowHandledDialog();
        }
    }
}
