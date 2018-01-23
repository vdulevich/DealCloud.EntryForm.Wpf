using System;
using System.Drawing;
using System.Windows.Forms;
using DealCloud.AddIn.Common.Utils;

namespace DealCloud.AddIn.Common.Controls.Common
{
    public class ComboBoxToolStripExt : ComboBoxExt, IMessageFilter
    {
        protected ToolStripDropDown ToolStripDropDown = new ToolStripDropDown
        {
            CanOverflow = true,
            AutoClose = true,
            DropShadowEnabled = true
        };

        public virtual ToolStripControlHost ToolStripControlHost { get; }

        public ComboBoxToolStripExt()
        {
            base.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Application.AddMessageFilter(this);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ToolStripControlHost.Width = this.DropDownWidth - 2;
            ToolStripControlHost.Height = this.DropDownHeight;
        }

        public bool PreFilterMessage(ref Message msg)
        {
            if (ToolStripDropDown.Visible)
            {
                switch (msg.Msg)
                {
                    case WinApi.WM_LBUTTONDOWN:
                    case WinApi.WM_RBUTTONDOWN:
                    case WinApi.WM_MBUTTONDOWN:
                    case WinApi.WM_NCLBUTTONDOWN:
                    case WinApi.WM_NCRBUTTONDOWN:
                    case WinApi.WM_NCMBUTTONDOWN:
                        {
                            int i = unchecked((int)(long)msg.LParam);
                            short x = (short)(i & 0xFFFF);
                            short y = (short)((i >> 16) & 0xFFFF);
                            Point pt = new Point(x, y);
                            WinApi.MapWindowPoints(msg.HWnd, ToolStripDropDown.Handle, ref pt, 1);
                            if (!ToolStripDropDown.ClientRectangle.Contains(pt))
                            {
                                if (!ToolStripDropDown.ClientRectangle.Contains(ToolStripDropDown.PointToClient(new Point(x, y))))
                                {
                                    pt = new Point(x, y);
                                    WinApi.MapWindowPoints(msg.HWnd, Handle, ref pt, 1);
                                    if (!ClientRectangle.Contains(pt))
                                    {
                                        HideToolStrip();
                                    }
                                }
                            }
                            break;
                        }
                }
            }
            return false;
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == WinApi.WM_LBUTTONDOWN ||
                msg.Msg == WinApi.WM_LBUTTONDBLCLK)
            {
                int x = msg.LParam.ToInt32() & 0xFFFF;
                if (x >= (Width - SystemInformation.VerticalScrollBarWidth))
                {
                    TogleToolStrip();
                    return;
                }
            }
            base.WndProc(ref msg);
        }

        public virtual void TogleToolStrip()
        {
            if (ToolStripControlHost.Visible)
            {
                HideToolStrip();
            }
            else
            {
                ShowToolStrip();
            }
        }

        public virtual void ShowToolStrip()
        {
            ToolStripDropDown.Show(this, 0, Height - 1);
        }

        public virtual void HideToolStrip()
        {
            ToolStripDropDown.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Application.RemoveMessageFilter(this);
                if (ToolStripDropDown != null)
                {
                    ToolStripDropDown.Dispose();
                    ToolStripDropDown = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
