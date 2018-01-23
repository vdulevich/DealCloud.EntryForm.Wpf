using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using DealCloud.AddIn.Common.Controls.Common;
using DealCloud.AddIn.Common.Utils;
using System.Net;

namespace DealCloud.AddIn.Common.Controls
{
    public class TextBoxExt : TextBox
    {
        private System.Timers.Timer _delayTimer;

        private bool _timerElapsed;

        private bool _keysPressed;

        private int? _delay;

        public int? Delay
        {
            get { return _delay; }
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
                    if (_delayTimer != null)
                    {
                        _delayTimer.Stop();
                    }
                }
                _delay = value;
            }
        }

        private LoadingControlUtil _loadingControl;

        public event EventHandler<string> TextUpdateDelayed;

        public bool Loading
        {
            get { return _loadingControl != null && _loadingControl.Loading; }
            set { (_loadingControl = _loadingControl ?? new LoadingControlUtil(this)).Loading = value; }
        }

        public TextBoxExt()
        {
            base.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point);
            BorderStyle = BorderStyle.FixedSingle;
        }

        private void delayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _delayTimer.Enabled = false;
            _timerElapsed = true;
            this.Invoke(new Action(() => { TextUpdateDelayed?.Invoke(this, this.Text); }));
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

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (_delayTimer != null)
            {
                if (_timerElapsed || !_keysPressed)
                {
                    _timerElapsed = false;
                    _keysPressed = false;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_delayTimer != null)
            {
                _delayTimer.Dispose();
                _delayTimer = null;
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WinApi.WM_PAINT)
            {
                if (BorderStyle == BorderStyle.FixedSingle)
                {
                    Graphics g = Graphics.FromHwnd(Handle);
                    Rectangle bounds = new Rectangle(0, 0, Width, Height);
                    ControlPaint.DrawBorder(g, bounds, Color.DarkGray, ButtonBorderStyle.Solid);
                }
            }
        }
    }
}
