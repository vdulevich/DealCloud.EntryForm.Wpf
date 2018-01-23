using System.Drawing;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls
{
    public class ButtonStyle
    {
        public Color ForeColor { get; set; }

        public Color BackColor { get; set; }

        public bool FontUpper { get; set; }

        public Font Font { get; set; }

        public FlatStyle FlatStyle { get; set; }

        public int FlatBorderSize { get; set; }

        public Color FlatBorderColor { get; set; }

        public int Height { get; set; }

        public Cursor Cursor { get; set; }
    }

    public class ThemeStyleButtons
    {
        public ButtonStyle Default { get; set; }

        public ButtonStyle DefaultDisabled { get; set; }

        public ButtonStyle Accept { get; set; }

        public ButtonStyle AcceptDisabled { get; set; }
    }

    public class ComboboxStyle
    {
        public FlatStyle FlatStyle { get; set; }

        public Font Font { get; set; }
    }

    public class LabelStyle
    {
        public Font Font { get; set; }

        public Color ForeColor { get; set; }
    }

    public class ThemeStyleLabel
    {
        public LabelStyle Default { get; set; }

        public LabelStyle WordDefault { get; set; }
    }

    public class ThemeStyleCombobox
    {
        public ComboboxStyle Default { get; set; }
    }

    public class FormStyle
    {
        public Color BorderColor { get; set; }
    }

    public class ThemeStyle
    {
        public FormStyle Form { get; set; }

        public ThemeStyleButtons Buttons { get; set; }

        public ThemeStyleCombobox Combobox { get; set; }

        public ThemeStyleLabel Lable { get; set; }

        public static ThemeStyle Current => Default;

        public static readonly ThemeStyle Default = new ThemeStyle
        {
            Form = new FormStyle
            {
                BorderColor = Color.FromArgb(28, 47, 93)
            },
            Buttons = new ThemeStyleButtons()
            {
                Default = new ButtonStyle
                {
                    BackColor = Color.FromArgb(248, 248, 248),
                    Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Underline, GraphicsUnit.Point),
                    FontUpper = true,
                    Cursor = Cursors.Hand,
                    FlatStyle = FlatStyle.Flat,
                    FlatBorderSize = 0,
                    FlatBorderColor = Color.FromArgb(0, 255, 255, 255), //transparent
                    ForeColor = Color.Black,
                    Height = 32
                },
                DefaultDisabled = new ButtonStyle()
                {
                    BackColor = Color.LightGray,
                    Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point),
                    ForeColor = Color.DarkGray
                },
                Accept = new ButtonStyle
                {
                    BackColor = Color.FromArgb(28, 47, 93),
                    Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular | FontStyle.Bold, GraphicsUnit.Point),
                    FontUpper = true,
                    Cursor = Cursors.Hand,
                    FlatStyle = FlatStyle.Flat,
                    FlatBorderSize = 0,
                    FlatBorderColor = Color.FromArgb(0, 255, 255, 255), //transparent
                    ForeColor = Color.White,
                    Height = 32
                },
                AcceptDisabled = new ButtonStyle()
                {
                    BackColor = Color.LightGray,
                    Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point),
                    ForeColor = Color.Gray
                },
            },
            Combobox = new ThemeStyleCombobox()
            {
                Default = new ComboboxStyle()
                {
                    Font = new Font("Microsoft Sans Serif", 8.5F, FontStyle.Regular, GraphicsUnit.Point),
                    FlatStyle = FlatStyle.Flat
                }
            },
            Lable = new ThemeStyleLabel()
            {
                Default = new LabelStyle()
                {
                    Font = new Font("Microsoft Sans Serif", 8.5F, FontStyle.Regular, GraphicsUnit.Point),
                    ForeColor = Color.Black
                }
            }
        };
    }
}
