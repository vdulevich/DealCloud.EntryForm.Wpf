using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DealCloud.AddIn.Common.Utils;
using DealCloud.Common.Extensions;
using System.Net;

namespace DealCloud.AddIn.Common.Controls.Common
{
    public class ComboBoxCheckListExt : ComboBoxToolStripExt
    {
        public override ToolStripControlHost ToolStripControlHost => _toolStripHost;

        private readonly ToolStripControlHost _toolStripHost;

        private readonly TextBoxExt _txtSearch;

        private readonly CheckListBoxExt _checkedListBox;

        private IList<object> _itemsOriginal;

        private readonly List<object> _checkedItems = new List<object>();

        public event Action<object, IEnumerable<object>> CheckChanged;

        private bool _ignoreCheck;

        public string SearchText
        {
            get { return _checkedListBox.HtmlDecode ? WebUtility.HtmlEncode(_txtSearch.Text) : _txtSearch.Text; }
            set { _txtSearch.Text = value; }
        }

        public override bool HtmlDecode
        {
            get { return base.HtmlDecode; }
            set { base.HtmlDecode = value; _checkedListBox.HtmlDecode = value; }
        }

        public IEnumerable<object> CheckedItems
        {
            get { return _checkedItems; }
            set
            {
                _checkedItems.Clear();
                _checkedListBox.UnCheckAll();
                if (value != null)
                {
                    foreach (var newItem in value)
                    {
                        var newItemValue = newItem.GetType().GetProperty(this.ValueMember)?.GetValue(newItem);
                        var originalItem = _itemsOriginal.FirstOrDefault(item =>
                        {
                            var itemValue = item.GetType().GetProperty(this.ValueMember)?.GetValue(item);
                            return itemValue == newItemValue || itemValue?.Equals(newItem) == true;
                        });
                        if (originalItem != null)
                        {
                            _checkedItems.Add(originalItem);
                            _checkedListBox.SetItemChecked(_checkedListBox.Items.IndexOf(originalItem), true);
                        }
                    }
                }
            }
        }

        public ComboBoxCheckListExt()
        {
            _checkedListBox = new CheckListBoxExt()
            {
                BorderStyle = BorderStyle.None,
                Font = this.Font,
                Dock = DockStyle.Fill,
                CheckOnClick = true,
                IntegralHeight = false,
                FormattingEnabled = true
            };

            ToolStripDropDown.VisibleChanged += ToolStripDropDown_VisibleChanged;

            _checkedListBox.ItemCheck += checkedList_ItemCheck;
            _checkedListBox.MouseDown += _checkedListBox_MouseDown;
            _checkedListBox.MouseUp += _checkedListBox_MouseUp;
            _checkedListBox.KeyDown += _checkedListBox_KeyDown;
            _checkedListBox.KeyUp += _checkedListBox_KeyUp;

            _txtSearch = new TextBoxExt { Anchor = AnchorStyles.Left | AnchorStyles.Right, Delay = 500 };
            _txtSearch.TextUpdateDelayed += txtSearch_TextUpdateDelayed;

            this.TextUpdate += comboBoxCheckListExt_TextUpdate;

            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.Controls.Add(_txtSearch, 0, 0);
            panel.Controls.Add(_checkedListBox, 0, 1);
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            _toolStripHost = new ToolStripControlHost(panel) { AutoSize = false };
            ToolStripDropDown.Items.Add(_toolStripHost);
        }

        private void ToolStripDropDown_VisibleChanged(object sender, EventArgs e)
        {
            if (ToolStripDropDown.Visible)
            {
                this.BeginInvoke(new Action(() => _checkedListBox.Focus()));
            }
            else
            {
                if (!SearchText.IsNullOrEmpty())
                {
                    SearchText = null;
                    txtSearch_TextUpdateDelayed(_txtSearch, null);
                }
            }
        }

        private static List<object> Filter(string filetr, string displayMember, object[] items)
        {
            return items.Where(item =>
            {
                string text = item.GetType().GetProperty(displayMember)?.GetValue(item) as string;
                return text?.IndexOf(filetr, 0, StringComparison.CurrentCultureIgnoreCase) > -1;
            }).ToList();
        }

        public void DataBind(object[] items)
        {
            _itemsOriginal = items;
            _checkedListBox.DisplayMember = DisplayMember;
            _checkedListBox.ValueMember = ValueMember;
            _checkedListBox.Items.AddRange(items);
            ApplyEmptyText();
        }

        private void comboBoxCheckListExt_TextUpdate(object sender, EventArgs e)
        {
            CheckedItems = null;
        }

        private void txtSearch_TextUpdateDelayed(object sender, string e)
        {
            _itemsOriginal = _itemsOriginal ?? _checkedListBox.Items.Cast<object>().ToList();
            _checkedListBox.BeginUpdate();
            _checkedListBox.Items.Clear();
            if (!string.IsNullOrEmpty(SearchText))
            {
                var filtered = Filter(SearchText, DisplayMember, _itemsOriginal.ToArray());
                _checkedListBox.Items.AddRange(filtered.ToArray());
                foreach (var checkedItem in _checkedItems)
                {
                    int index = filtered.IndexOf(checkedItem);
                    if (index >= 0)
                    {
                        _checkedListBox.SetItemChecked(index, true);
                    }
                }
            }
            else
            {
                _checkedListBox.Items.AddRange(_itemsOriginal.ToArray());
                foreach (var checkedItem in _checkedItems)
                {
                    _checkedListBox.SetItemChecked(_itemsOriginal.IndexOf(checkedItem), true);
                }
            }
            _checkedListBox.EndUpdate();
        }

        private void checkedList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_ignoreCheck) { e.NewValue = e.CurrentValue; return; }

            if (e.NewValue == CheckState.Checked)
            {
                if (!_checkedItems.Contains(_checkedListBox.Items[e.Index]))
                {
                    _checkedItems.Add(_checkedListBox.Items[e.Index]);
                }
            }
            else
            {
                _checkedItems.Remove(_checkedListBox.Items[e.Index]);
            }
            var text = string.Join(", ", _checkedItems.Select(item => $"[{item.GetType().GetProperty(this.DisplayMember)?.GetValue(item)}]").ToArray());
            Text = HtmlDecode ? WebUtility.HtmlDecode(text) : text;
            try
            {
                CheckChanged?.Invoke(this, _checkedItems);
            }
            catch (Exception)
            {
                //throw;
            }
        }

        private void _checkedListBox_MouseDown(object sender, MouseEventArgs e)
        {
            _ignoreCheck = e.X > SystemInformation.MenuCheckSize.Width;
        }

        private void _checkedListBox_MouseUp(object sender, MouseEventArgs e)
        {
            _ignoreCheck = false;
        }

        private void _checkedListBox_KeyUp(object sender, KeyEventArgs e)
        {
            _ignoreCheck = false;
        }

        private void _checkedListBox_KeyDown(object sender, KeyEventArgs e)
        {
            _ignoreCheck = true;
        }
    }
}

