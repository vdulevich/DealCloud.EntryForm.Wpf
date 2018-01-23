using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls
{
    public class ButtonExt : Button
    {
        private string _text;

        public static ButtonStyle Style => ThemeStyle.Current.Buttons.Default;

        public static ButtonStyle StyleDisabled => ThemeStyle.Current.Buttons.DefaultDisabled;

        public new string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                base.Text = (FontUpper) ? _text?.ToUpper() : _text;
            }
        }

        public bool FontUpper { get; set; }

        private static bool DefaultFontUpper => Style.FontUpper;

        private bool ShouldSerializeFontUpper()
        {
            return FontUpper != DefaultFontUpper;
        }

        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        private new static Font DefaultFont => Style.Font;

        private bool ShouldSerializeFont()
        {
            return !this.Font.Equals(DefaultFont);
        }

        public override Color ForeColor
        {
            get { return Enabled ? base.ForeColor : StyleDisabled.ForeColor; }
            set { base.ForeColor = value; }
        }

        private new static Color DefaultForeColor => Style.ForeColor;

        private bool ShouldSerializeForeColor()
        {
            return !this.ForeColor.Equals(DefaultForeColor);
        }

        public override Color BackColor
        {
            get { return Enabled ? base.BackColor : StyleDisabled.BackColor; }
            set { base.BackColor = value;}
        }

        private new static Color DefaultBackColor => Style.BackColor;

        private bool ShouldSerializeBackColor()
        {
            return !this.BackColor.Equals(DefaultBackColor);
        }

        public override Cursor Cursor
        {
            get { return base.Cursor; }
            set { base.Cursor = value; }
        }

        private new static Cursor DefaultCursor => Style.Cursor;

        private bool ShouldSerializeCursor()
        {
            return !this.Cursor.Equals(DefaultCursor);
        }

        public new FlatStyle FlatStyle
        {
            get { return base.FlatStyle; }
            set { base.FlatStyle = value; }
        }

        private static FlatStyle DefaultFlatStyle => Style.FlatStyle;

        private bool ShouldSerializeFlatStyle()
        {
            return !this.FlatStyle.Equals(DefaultFlatStyle);
        }

        public new FlatButtonAppearance FlatAppearance
        {
            get
            {
                var fa = base.FlatAppearance;
                fa.BorderColor = Style.FlatBorderColor;
                fa.BorderSize = Style.FlatBorderSize;
                return fa;
            }
        }

        private bool ShouldSerializeFlatAppearance()
        {
            return !this.FlatAppearance.BorderColor.Equals(Style.FlatBorderColor) || 
                   !this.FlatAppearance.BorderSize.Equals(Style.FlatBorderSize);
        }

        public void ApplyStyle(ButtonStyle style)
        {
            Font = Style.Font;
            FontUpper = Style.FontUpper;
            ForeColor = Style.ForeColor;
            BackColor = Style.BackColor;
            Cursor = Style.Cursor;
            FlatStyle = Style.FlatStyle;
            FlatAppearance.BorderColor = Style.FlatBorderColor;
            FlatAppearance.BorderSize = Style.FlatBorderSize;
        }

        public ButtonExt()
        {
            ApplyStyle(Style);
        }
    }

    public class ButtonAcceptExt : Button
    {
        private string _text;

        public static ButtonStyle Style => ThemeStyle.Current.Buttons.Accept;

        public static ButtonStyle StyleDisabled => ThemeStyle.Current.Buttons.AcceptDisabled;

        public new string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                base.Text = (FontUpper) ? _text?.ToUpper() : _text;
            }
        }

        public bool FontUpper { get; set; }

        private static bool DefaultFontUpper => Style.FontUpper;

        private bool ShouldSerializeFontUpper()
        {
            return FontUpper != DefaultFontUpper;
        }

        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        private new static Font DefaultFont => Style.Font;

        private bool ShouldSerializeFont()
        {
            return !this.Font.Equals(DefaultFont);
        }

        public override Color ForeColor
        {
            get { return Enabled ? base.ForeColor : StyleDisabled.ForeColor; }
            set { base.ForeColor = value; }
        }

        private new static Color DefaultForeColor => Style.ForeColor;

        private bool ShouldSerializeForeColor()
        {
            return !this.ForeColor.Equals(DefaultForeColor);
        }

        public override Color BackColor
        {
            get { return Enabled ? base.BackColor : StyleDisabled.BackColor; }
            set { base.BackColor = value; }
        }

        private new static Color DefaultBackColor => Style.BackColor;

        private bool ShouldSerializeBackColor()
        {
            return !this.BackColor.Equals(DefaultBackColor);
        }

        public override Cursor Cursor
        {
            get { return base.Cursor; }
            set { base.Cursor = value; }
        }

        private new static Cursor DefaultCursor => Style.Cursor;

        private bool ShouldSerializeCursor()
        {
            return !this.Cursor.Equals(DefaultCursor);
        }

        public new FlatStyle FlatStyle
        {
            get { return base.FlatStyle; }
            set { base.FlatStyle = value; }
        }

        private static FlatStyle DefaultFlatStyle => Style.FlatStyle;

        private bool ShouldSerializeFlatStyle()
        {
            return !this.FlatStyle.Equals(DefaultFlatStyle);
        }

        public new FlatButtonAppearance FlatAppearance
        {
            get
            {
                var fa = base.FlatAppearance;
                fa.BorderColor = Style.FlatBorderColor;
                fa.BorderSize = Style.FlatBorderSize;
                return fa;
            }
        }

        private bool ShouldSerializeFlatAppearance()
        {
            return !this.FlatAppearance.BorderColor.Equals(Style.FlatBorderColor) ||
                   !this.FlatAppearance.BorderSize.Equals(Style.FlatBorderSize);
        }

        public void ApplyStyle(ButtonStyle style)
        {
            Font = Style.Font;
            FontUpper = Style.FontUpper;
            ForeColor = Style.ForeColor;
            BackColor = Style.BackColor;
            Cursor = Style.Cursor;
            FlatStyle = Style.FlatStyle;
            FlatAppearance.BorderColor = Style.FlatBorderColor;
            FlatAppearance.BorderSize = Style.FlatBorderSize;
        }

        public ButtonAcceptExt()
        {
            ApplyStyle(Style);
        }
    }
}
