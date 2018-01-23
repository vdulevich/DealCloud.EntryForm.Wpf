using System.Drawing;
using System.Windows.Forms;
using DealCloud.AddIn.Common.Utils;
using DealCloud.Common.Extensions;

namespace DealCloud.AddIn.Common.Controls
{
    public class TreeViewExt : TreeView
    {
        private ToolTip _toolTip1 = new ToolTip() { InitialDelay = 100 };

        public bool HtmlDecode { get; set; }

        public TreeViewExt()
        {
            // .NET Bug: Unless LineColor is set, Win32 treeview returns -1 (default), .NET returns Color.Black!
            base.LineColor = SystemColors.GrayText;
            base.DrawMode = TreeViewDrawMode.OwnerDrawText;
            base.ShowNodeToolTips = false;
        }

        public Color GetItemBackColor(DrawTreeNodeEventArgs e)
        {
            Color backColor;

            if ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected ||
                (e.State & TreeNodeStates.Focused) == TreeNodeStates.Focused)
            {
                backColor = SystemColors.Highlight;
            }
            else if ((e.State & TreeNodeStates.Hot) == TreeNodeStates.Hot)
            {
                backColor = SystemColors.HotTrack;
            }
            else
            {
                backColor = this.BackColor;//e.Node.BackColor;
            }
            return backColor;
        }

        public Color GetItemForeColor(DrawTreeNodeEventArgs e)
        {
            Color foreColor;
            if ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected ||
                (e.State & TreeNodeStates.Focused) == TreeNodeStates.Focused)
            {
                foreColor = SystemColors.HighlightText;
            }
            else if ((e.State & TreeNodeStates.Hot) == TreeNodeStates.Hot)
            {
                foreColor = SystemColors.HighlightText;
            }
            else
            {
                foreColor = e.Node.ForeColor;
            }
            return foreColor;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Get the node at the current mouse pointer location.
            TreeNode node = this.GetNodeAt(e.X, e.Y);
            // Set a ToolTip only if the mouse pointer is actually paused on a node.
            if ((node != null))
            {
                // Verify that the tag property is not "null".
                if (node.Text != null)
                {
                    // Change the ToolTip only if the pointer moved to a new node.
                    var nodeText = HtmlDecode ? node.Text.HtmlDecode() : node.Text;
                    using (var graphics = this.CreateGraphics())
                    {
                        SizeF stringSize = graphics.MeasureString(nodeText, node.NodeFont ?? this.Font);
                        if (stringSize.Width + node.Bounds.X > this.Width)
                        {
                            if (!string.Equals(nodeText, _toolTip1.GetToolTip(this)))
                            {
                                _toolTip1.SetToolTip(this, nodeText);
                            }
                        }
                        else
                        {
                            _toolTip1.SetToolTip(this, "");
                        }
                    }
                }
                else
                {
                    _toolTip1.SetToolTip(this, "");
                }
            }
            else     // Pointer is not over a node so clear the ToolTip.
            {
                _toolTip1.SetToolTip(this, "");
            }
        }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            int iconOffset = e.Node.ImageIndex == e.Node.TreeView.ImageList?.Images.Count ? e.Node.TreeView.ImageList.ImageSize.Width + 3: 0;
            string nodeText = this.HtmlDecode ? e.Node.Text.HtmlDecode() : e.Node.Text;
            Font nodeFont = e.Node.NodeFont ?? this.Font;
            SizeF nodeTextSize = TextRenderer.MeasureText(nodeText, nodeFont);
            Color backColor = GetItemBackColor(e), foreColor = GetItemForeColor(e);
            
            //Calculate the text rectangle.
            Rectangle nodeTextRect = new Rectangle(
                e.Node.Bounds.X,
                e.Node.Bounds.Y,
                (int)nodeTextSize.Width,
                e.Node.Bounds.Height);

            if (FullRowSelect)
            {
                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    nodeTextRect.Offset(-iconOffset, 0);
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
            }
            else
            {
                using (SolidBrush brushClear = new SolidBrush(this.BackColor))
                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    e.Graphics.FillRectangle(brushClear, nodeTextRect);
                    nodeTextRect.Offset(-iconOffset, 0);
                    e.Graphics.FillRectangle(brush, nodeTextRect);
                }
            }

            // Draw the text.
            TextRenderer.DrawText(
                e.Graphics,
                nodeText,
                nodeFont, 
                nodeTextRect, 
                foreColor, 
                backColor,
                (FullRowSelect ? TextFormatFlags.Left : TextFormatFlags.HorizontalCenter) | TextFormatFlags.NoPrefix);

            //Draw the focused rectangle.
            if ((e.State & TreeNodeStates.Focused) == TreeNodeStates.Focused && !FullRowSelect)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, nodeTextRect, foreColor, backColor);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x8000; // TVS_NOHSCROLL
                cp.Style |= 0x0080; // TVS_NOTOOLTIPS 
                return cp;
            }
        }
    }
}
