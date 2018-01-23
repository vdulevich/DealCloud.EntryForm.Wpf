using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls
{
    public partial class ReportPreviewDialog : FormExt
    {
        public string Message
        {
            set { userDetailsTextBox.Text = value; }
        }

        public string SystemInfo
        {
            set { systemDetailsTextBox.Text = value; }
        }

        public List<string> LogFiles
        {
            set
            {
                logFilesListView.Items.Clear();
                foreach (var file in value)
                {
                    logFilesListView.Items.Add(file, Path.GetFileName(file), 0);
                }
            }
        }

        public ReportPreviewDialog()
        {
            InitializeComponent();
        }

        private void logFilesListView_ItemActivate(object sender, System.EventArgs e)
        {
            foreach (ListViewItem item in logFilesListView.SelectedItems)
            {
                Process.Start(item.Name);
            }
        }
    }
}
