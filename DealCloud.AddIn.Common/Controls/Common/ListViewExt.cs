using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DealCloud.AddIn.Common.Utils;

namespace DealCloud.AddIn.Common.Controls
{
    public class ListViewExt: ListView
    {
        [Category("Custom")]
        [Description("Displays a message in the ListView when no records are displayed in it.")]
        [DefaultValue(typeof(string), "")]
        public string EmptyText { get; set; }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WinApi.WM_NOTIFY | WinApi.WM_REFLECT:
                    var nmhdr = (WinApi.NMHDR)m.GetLParam(typeof(WinApi.NMHDR));
                    switch (nmhdr.code)
                    {
                        case WinApi.LVN_GETEMPTYMARKUP:
                            var markup = (WinApi.NMLVEMPTYMARKUP)m.GetLParam(typeof(WinApi.NMLVEMPTYMARKUP));
                            markup.szMarkup = EmptyText;
                            Marshal.StructureToPtr(markup, m.LParam, false);
                            m.Result = new IntPtr(1);
                            return;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
    }
}
