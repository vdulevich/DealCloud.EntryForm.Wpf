using System;
using System.Drawing;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls
{
    public partial class LoadingErrorControl : UserControl
    {
        public event EventHandler RefreshClick;

        public bool ShowRefresh
        {
            get { return lblRefresh.Visible; }

            set
            {
                if (!value)
                {
                    pnlFailed.RowStyles[pnlFailed.GetRow(lblRefresh)].Height = 0;
                    pnlFailed.RowStyles[pnlFailed.GetRow(lblError)].Height = 100;
                    lblError.TextAlign = ContentAlignment.MiddleCenter;
                }
                else
                {
                    pnlFailed.RowStyles[pnlFailed.GetRow(lblRefresh)].Height = 50;
                    pnlFailed.RowStyles[pnlFailed.GetRow(lblError)].Height = 50;
                    lblError.TextAlign = ContentAlignment.TopCenter;
                }
            }
        }

        public void Show(string text, bool showRefresh = false)
        {
            lblError.Text = text.Length > 1024 ? $"{text.Substring(0, 1024)}..." : text;
            ShowRefresh = showRefresh;
            pnlFailed.Show();
        }

        public LoadingErrorControl()
        {
            InitializeComponent();
            lblRefresh.Click += LblRefresh_Click;
        }

        private void LblRefresh_Click(object sender, EventArgs e)
        {
            RefreshClick?.Invoke(sender, e);
        }
    }
}
