using DealCloud.AddIn.Common.Utils;
using System;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls
{
    public partial class MessageErrorDialog : FormExt
    {
        public string Message
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        private bool _showDetails;
        public bool ShowDetails
        {
            get
            {
                return _showDetails;
            }
            set
            {
                _showDetails = value;
                textBox.Visible = _showDetails;
                Resizable = _showDetails;
                AutoSizeMode = _showDetails ? AutoSizeMode.GrowOnly : AutoSizeMode.GrowAndShrink;
            }
        }

        public string MessageDetails
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        public bool AllowReporting
        {
            get { return reportButton.Visible; }
            set { reportButton.Visible = value; }
        }

        public MessageErrorDialog()
        {
            InitializeComponent();
        }

        private void messageErrorDialog_Load(object sender, EventArgs e)
        {
            textBox.Visible = false;
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowDetails = !ShowDetails;
        }

        private void reportButton_Click(object sender, EventArgs e)
        {
            ReportUtil.Instance.ShowReport();
        }
    }
}
