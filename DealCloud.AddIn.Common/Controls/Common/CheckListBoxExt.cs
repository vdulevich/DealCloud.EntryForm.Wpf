using DealCloud.Common.Extensions;
using System.Net;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls.Common
{
    public class CheckListBoxExt : CheckedListBox
    {
        public bool HtmlDecode { get; set; }

        protected override void OnFormat(ListControlConvertEventArgs e)
        {
            base.OnFormat(e);
            if (HtmlDecode)
            {
                if (!DisplayMember.IsNullOrEmpty())
                {
                    var property = e.ListItem.GetType().GetProperty(DisplayMember);
                    if (property != null)
                    {
                        e.Value = WebUtility.HtmlDecode(property.GetValue(e.ListItem) as string);
                    }
                }
            }
        }
    }
}
