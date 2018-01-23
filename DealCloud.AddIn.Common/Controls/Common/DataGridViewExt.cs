using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls
{
    public class DataGridViewExt : DataGridView
    {
        [Category("Custom")]
        [Description("Displays a message in the DataGridView when no records are displayed in it.")]
        [DefaultValue(typeof(string), "")]
        public string EmptyText { get; set; }

        public LoadingGridViewUtil Loading { get; set; }
        public Image CollapseIcon { get; internal set; }
        public Image ExpandIcon { get; internal set; }

        public DataGridViewExt()
        {
            
            Loading = new LoadingGridViewUtil(this) { ShowClose = false };
        }

        protected override void PaintBackground(Graphics graphics, Rectangle clipBounds, Rectangle gridBounds)
        {
            base.PaintBackground(graphics, clipBounds, gridBounds);
            if (Loading.Loading)
            {
                return;
            }
            if ((this.Enabled && (this.RowCount == 0)) && !string.IsNullOrEmpty(this.EmptyText))
            {
                string emptyText = this.EmptyText;
                var ef = new RectangleF(4f, this.ColumnHeadersHeight + 4, this.Width - 8, (this.Height - this.ColumnHeadersHeight) - 8);
                graphics.DrawString(emptyText, this.Font, Brushes.White, ef);
            }
        }
    }
}
