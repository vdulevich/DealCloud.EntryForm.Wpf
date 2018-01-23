using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls
{
    public class LabelExt : Label
    {
        private string _text;

        private TextRenderingHint _hint = TextRenderingHint.SystemDefault;

        public bool FontUpperCase { get; set; }

        private static bool DefaultFontUpperCase()
        {
            return false;
        }

        private bool ShouldSerializeFontUpperCase()
        {
            return FontUpperCase != DefaultFontUpperCase();
        }

        public new string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                base.Text = FontUpperCase ? _text?.ToUpper() : _text;
            }
        }

        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        private new static Font DefaultFont
        {
            get { return ThemeStyle.Current.Lable.Default.Font; }
        }

        private bool ShouldSerializeFont()
        {
            return !this.Font.Equals(DefaultFont);
        }

        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        private new static Color DefaultForeColor
        {
            get { return ThemeStyle.Current.Lable.Default.ForeColor; }
        }

        private bool ShouldSerializeForeColor()
        {
            return !this.ForeColor.Equals(DefaultForeColor);
        }

        public TextRenderingHint TextRenderingHint
        {
            get { return this._hint; }
            set { this._hint = value; }
        }

        public LabelExt()
        {
            Font = ThemeStyle.Current.Lable.Default.Font;
            ForeColor = ThemeStyle.Current.Lable.Default.ForeColor;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.TextRenderingHint = TextRenderingHint;
            base.OnPaint(pe);
        }
    }
}
