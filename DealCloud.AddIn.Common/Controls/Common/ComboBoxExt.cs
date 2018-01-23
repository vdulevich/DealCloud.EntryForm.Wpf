using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;
using DealCloud.AddIn.Common.Utils;
using System.Net;
using DealCloud.Common.Extensions;
using System.ComponentModel;

namespace DealCloud.AddIn.Common.Controls
{
    [ComVisible(false)]
    public class ComboBoxExt : ComboBox
    {
        private int? _textUpdateDelay;

        private string _emptyText;

        private bool _timerElapsed;

        private bool _keysPressed;

        private System.Timers.Timer _delayTimer;

        private LoadingControlUtil _loadingControl;

        public event EventHandler<string> TextUpdateDelayed;

        private static ComboboxStyle Style => ThemeStyle.Current.Combobox.Default;

        public virtual bool HtmlDecode { get; set; }

        public bool ReadOnly { get; set; }

        public string EmptyText
        {
            get { return _emptyText; }
            set
            {
                _emptyText = value;
                ApplyEmptyText();
            }
        }

        public override string Text
        {
            get { return string.Compare(base.Text, EmptyText, false, CultureInfo.CurrentCulture) == 0 ? null : base.Text; }
            set { base.Text = value; }
        }

        public int? TextUpdateDelay
        {
            get { return _textUpdateDelay; }
            set
            {
                if (value != null && value > 0)
                {
                    if (_delayTimer == null)
                    {
                        _delayTimer = new System.Timers.Timer(value.Value);
                        _delayTimer.Elapsed += delayTimer_Elapsed;
                    }
                    else
                    {
                        _delayTimer.Interval = value.Value;
                    }
                }
                else
                {
                    _delayTimer?.Stop();
                }
                _textUpdateDelay = value;
            }
        }

        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        private new static Font DefaultFont
        {
            get { return Style.Font; }
        }

        private bool ShouldSerializeFont()
        {
            return !this.Font.Equals(DefaultFont);
        }

        public new FlatStyle FlatStyle
        {
            get { return base.FlatStyle; }
            set { base.FlatStyle = value; }
        }

        private static FlatStyle DefaultFlatStyle
        {
            get { return Style.FlatStyle; }
        }

        private bool ShouldSerializeFlatStyle()
        {
            return !this.FlatStyle.Equals(DefaultFlatStyle);
        }

        public bool Loading
        {
            get { return _loadingControl != null && _loadingControl.Loading; }
            set
            {
                (_loadingControl = _loadingControl ?? new LoadingControlUtil(this)).Loading = value;
            }
        }

        public ComboBoxExt()
        {
            MaxDropDownItems = 30;
        }

        protected bool HasEmptyText()
        {
            return string.Equals(EmptyText, base.Text);
        }

        public virtual void ApplyEmptyText()
        {
            if (Items.Count == 0 && !string.IsNullOrEmpty(EmptyText))
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

        private void delayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _delayTimer.Enabled = false;
            _timerElapsed = true;
            this.Invoke(new Action(() => { TextUpdateDelayed?.Invoke(this, this.Text); }));
        }

        protected override void WndProc(ref Message msg)
        {
            base.WndProc(ref msg);
            if (msg.Msg == WinApi.CB_ADDSTRING ||
                msg.Msg == WinApi.CB_INSERTSTRING ||
                msg.Msg == WinApi.CB_DELETESTRING ||
                msg.Msg == WinApi.CB_RESETCONTENT)
            {
                ApplyEmptyText();
            }
            if (msg.Msg == WinApi.WM_PAINT)
            {
                Graphics g = Graphics.FromHwnd(Handle);
                Rectangle bounds = new Rectangle(0, 0, Width, Height);
                ControlPaint.DrawBorder(g, bounds, Color.DarkGray, ButtonBorderStyle.Solid);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (ReadOnly)
            {
                e.Handled = true;
            }
            else
            {
                if (_delayTimer != null)
                {
                    if (!_delayTimer.Enabled)
                    {
                        _delayTimer.Enabled = true;
                    }
                    else
                    {
                        _delayTimer.Enabled = false;
                        _delayTimer.Enabled = true;
                    }
                    _keysPressed = true;
                }
                base.OnKeyPress(e);
            }
        }

        protected override void OnTextUpdate(EventArgs e)
        {
            base.OnTextUpdate(e);
            if (_delayTimer != null)
            {
                if (_timerElapsed || !_keysPressed)
                {
                    _timerElapsed = false;
                    _keysPressed = false;
                }
            }
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
            ApplyEmptyText();
        }

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
