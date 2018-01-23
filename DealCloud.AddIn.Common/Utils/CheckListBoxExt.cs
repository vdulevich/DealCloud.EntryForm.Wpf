using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Utils
{
    public static class CheckListBoxExt
    {
        public static void UnCheckAll(this CheckedListBox checkListBox)
        {
            for (int i = 0; i < checkListBox.Items.Count; i++)
            {
                checkListBox.SetItemChecked(i, false);
            }
        }

        public static void CheckAll(this CheckedListBox checkListBox)
        {
            for (int i = 0; i < checkListBox.Items.Count; i++)
            {
                checkListBox.SetItemChecked(i, true);
            }
        }
    }
}
