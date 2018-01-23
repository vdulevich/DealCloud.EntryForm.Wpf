using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls.Common
{
   public static class TableLayoutPanelExt
    {
        public static void RemoveRow(this TableLayoutPanel panel, int rowIndexToRemove)
        {
            if (rowIndexToRemove >= panel.RowCount)
            {
                return;
            }

            // delete all controls of row that we want to delete
            for (int i = 0; i < panel.ColumnCount; i++)
            {
                var control = panel.GetControlFromPosition(i, rowIndexToRemove);
                panel.Controls.Remove(control);
            }

            // move up row controls that comes after row we want to remove
            for (int i = rowIndexToRemove + 1; i < panel.RowCount; i++)
            {
                for (int j = 0; j < panel.ColumnCount; j++)
                {
                    var control = panel.GetControlFromPosition(j, i);
                    if (control != null)
                    {
                        panel.SetRow(control, i - 1);
                    }
                }
            }

            // remove last row
            panel.RowStyles.RemoveAt(panel.RowCount - 1);
            panel.RowCount--;
        }
    }
}
