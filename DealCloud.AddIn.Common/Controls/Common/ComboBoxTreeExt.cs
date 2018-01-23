using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DealCloud.AddIn.Common.Controls.Common;
using DealCloud.AddIn.Common.Utils;
using DealCloud.Common.Extensions;
using System.Net;

namespace DealCloud.AddIn.Common.Controls
{
    public enum ComboBoxTreeMode
    {
        ComboBox,
        Tree
    }

    public class DisplayTextFormatEventArgs
    {
        public string Text;

        public ComboBoxTreeExt ComboBoxTreeExt { get; set; }
    }

    public class ComboBoxTreeExt : ComboBoxToolStripExt
    {
        private readonly TreeViewExt _treeView;

        private readonly TextBoxExt _searchBox;

        private object _selectedTag;

        private TreeNode[] _nodesOriginal;

        public event Action<ComboBoxTreeExt, TreeView, TreeNode, CancelEventArgs> TreeViewNodeTagBeforeSelect;

        public event Action<ComboBoxTreeExt, TreeView, TreeNode> TreeViewNodeTagSelected;

        public event Action<ComboBoxTreeExt, TreeView, TreeNode, TreeViewEventArgs> TreeViewAfterExpand;

        public event Action<DisplayTextFormatEventArgs> FormatDisplayText;

        public Func<ComboBoxTreeExt, TreeView, string, Task<TreeNode[]>> TextFilterFn;

        public override ToolStripControlHost ToolStripControlHost => _toolStripHost;

        private readonly ToolStripControlHost _toolStripHost;

        public string SearchText
        {
            get { return HtmlDecode ? WebUtility.HtmlEncode(_searchBox.Text) : _searchBox.Text; }
            set { _searchBox.Text = value; }
        }

        public bool AllowLocalFiltering { get; set; }

        public TreeView TreeView => _treeView;

        public ComboBoxTreeMode Mode { get; set; }

        public bool DisableScrollSelection { get; set; }

        public bool SelectOnHover { get; set; }

        public string SetectedItemText => GetTextValue(SelectedItem);

        public new object SelectedItem
        {
            get { return TreeView.SelectedNode?.Tag; }
            set
            {
                if (TreeView.SelectedNode?.Tag == value) return;
                
                TreeNode node = FindNodeByTag(value);
                TreeView.SelectedNode = node;

                DisplayTextFormatEventArgs args = new DisplayTextFormatEventArgs() { ComboBoxTreeExt = this, Text = GetTextValue(value) };
                FormatDisplayText?.Invoke(args);
                Text = args.Text;

                TreeViewNodeTagSelected?.Invoke(this, TreeView, node);
            }
        }

        public override bool HtmlDecode
        {
            get { return base.HtmlDecode; }
            set { base.HtmlDecode = value; _treeView.HtmlDecode = value; }
        }

        public ComboBoxTreeExt()
        {
            _treeView = new TreeViewExt
            {
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                Font = Font
            };

            _treeView.NodeMouseClick += treeView_NodeMouseClick;
            _treeView.PreviewKeyDown += treeView_PreviewKeyDown;
            _treeView.AfterExpand += treeView_AfterExpand;

            _searchBox = new TextBoxExt() { Anchor = AnchorStyles.Left | AnchorStyles.Right, Delay = 500 };
            _searchBox.TextUpdateDelayed += (sender, e) => { FilterTree(); };

            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Controls.Add(_searchBox, 0, 0);
            panel.Controls.Add(_treeView, 0, 1);
            
            _toolStripHost = new ToolStripControlHost(panel) { AutoSize = false };

            ToolStripDropDown.Items.Add(_toolStripHost);
            ToolStripDropDown.VisibleChanged += toolStripDropDown_VisibleChanged;
            DisplayMember = "Text";
        }

        public void DataBind(TreeNode[] nodes)
        {
            _nodesOriginal = null;
            TreeView.BeginUpdate();
            TreeView.Nodes.Clear();
            TreeView.Nodes.AddRange(nodes);
            TreeView.SelectedNode = null;
            TreeView.EndUpdate();
            SelectedItem = null;
            Text = null;
            ApplyEmptyText();
        }

        private string GetTextValue(object value)
        {
            if (value == null) return null;
            string text = !DisplayMember.IsNullOrEmpty()
                    ? value.GetType().GetProperty(DisplayMember)?.GetValue(value) as string ?? value.ToString()
                    : value.ToString();
            return HtmlDecode ? text.HtmlDecode() : text;
        }

        public void Clear()
        {
            SelectedItem = null;
            this.TreeView.Nodes.Clear();
        }

        public override void ShowToolStrip()
        {
            base.ShowToolStrip();
            var selected = TreeView.SelectedNode;
            TreeView.Focus();
            if (selected == null)
            {
                TreeView.SelectedNode = null;
            }
        }

        public override void ApplyEmptyText()
        {
            if (_nodesOriginal != null && _nodesOriginal.Length == 0 && !string.IsNullOrEmpty(EmptyText))
            {
                Text = EmptyText;
            }
            else
            {
                if (HasEmptyText())
                {
                    Text = null;
                }
            }
        }

        public TreeNode FindNodeByTag(object tag)
        {
            if (tag == null)
            {
                return null;
            }
            Func<TreeNode, bool> equals = node => Equals(node.Tag, tag);
            foreach (TreeNode node in TreeView.Nodes)
            {
                if (equals(node))
                {
                    return node;
                }
                TreeNode findNode = FindNode(equals, node);
                if (findNode != null)
                {
                    return findNode;
                }
            }
            return null;
        }

        public TreeNode FindNodeByText(string text)
        {
            text = HtmlDecode ? WebUtility.HtmlEncode(text) : text;
            Func<TreeNode, bool> equals = node => string.Equals(node.Text, text, StringComparison.OrdinalIgnoreCase);
            foreach (TreeNode node in TreeView.Nodes)
            {
                if (equals(node))
                {
                    return node;
                }
                TreeNode findNode = FindNode(equals, node);
                if (findNode != null)
                {
                    return findNode;
                }
            }
            return null;
        }

        private TreeNode FindNode(Func<TreeNode, bool> equals, TreeNode rootNode)
        {
            foreach (TreeNode node in rootNode.Nodes)
            {
                if (equals(node))
                {
                    return node;
                }
                TreeNode next = FindNode(equals, node);
                if (next != null)
                {
                    return next;
                }
            }
            return null;
        }

        public async void FilterTree()
        {
            _nodesOriginal = _nodesOriginal ?? TreeView.Nodes.Cast<TreeNode>().ToArray();
            TreeView.BeginUpdate();
            TreeView.Nodes.Clear();
            if (!SearchText.IsNullOrEmpty())
            {
                if (TextFilterFn != null)
                {
                    TreeView.Nodes.AddRange(await TextFilterFn(this, TreeView, _searchBox.Text));
                    TreeView.ExpandAll();
                }
                else
                {
                    TreeView.Nodes.AddRange(_nodesOriginal.Filter(SearchText).ToArray());
                    TreeView.ExpandAll();
                }
            }
            else
            {
                TreeView.Nodes.AddRange(_nodesOriginal);
            }
            if (TreeView.Nodes.Count > 0) TreeView.Nodes[0].EnsureVisible();
            TreeView.EndUpdate();
        }

        public override bool PreProcessMessage(ref Message msg)
        {
            if (msg.Msg == WinApi.WM_SYSKEYDOWN)
            {
                if (Mode == ComboBoxTreeMode.Tree &&
                    ((Keys)msg.WParam & Keys.KeyCode) == Keys.Down)
                {
                    ShowToolStrip();
                    return true;
                }
            }
            else if (msg.Msg == WinApi.WM_KEYDOWN)
            {
                if (TreeView.Visible && !TreeView.Focused &&
                    (((Keys)msg.WParam & Keys.KeyCode) == Keys.Down ||
                    ((Keys)msg.WParam & Keys.KeyCode) == Keys.Up))
                {
                    TreeView.Focus();
                    return true;
                }

                if (TreeView.Visible && !TreeView.Focused &&
                    (((Keys)msg.WParam & Keys.KeyCode) == Keys.Tab) ||
                    ((Keys)msg.WParam & Keys.KeyCode) == Keys.Escape)
                {
                    HideToolStrip();
                    return true;
                }

                if (DisableScrollSelection &&
                    (((Keys)msg.WParam & Keys.KeyCode) == Keys.Down || ((Keys)msg.WParam & Keys.KeyCode) == Keys.Up))
                {
                    return true;
                }
            }
            return base.PreProcessMessage(ref msg);
        }

        private void toolStripDropDown_VisibleChanged(object sender, EventArgs e)
        {
            if (!AllowLocalFiltering) return;

            if (ToolStripDropDown.Visible)
            {
                this.BeginInvoke(new Action(() => _treeView.Focus()));
            }
            else
            {
                if (!SearchText.IsNullOrEmpty())
                {
                    SearchText = null;
                    FilterTree();
                }
            }
        }

        private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            TreeViewAfterExpand?.Invoke(this, (TreeView)sender, e.Node, e);
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            CancelEventArgs args = new CancelEventArgs(false);
            TreeViewNodeTagBeforeSelect?.Invoke(this, (TreeView)sender, e.Node, args);
            if (!args.Cancel)
            {
                SelectedItem = e.Node.Tag;
                HideToolStrip();
            }
        }

        private void treeView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CancelEventArgs args = new CancelEventArgs(false);
                TreeViewNodeTagBeforeSelect?.Invoke(this, (TreeView)sender, TreeView.SelectedNode, args);
                if (!args.Cancel)
                {
                    SelectedItem = TreeView.SelectedNode.Tag;
                    HideToolStrip();
                }
            }
            if (e.KeyCode == Keys.Escape ||
                e.KeyCode == Keys.Back)
            {
                HideToolStrip();
            }
        }
    }
}
