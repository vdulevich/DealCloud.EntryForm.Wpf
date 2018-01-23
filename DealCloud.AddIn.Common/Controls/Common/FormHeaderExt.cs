using System;
using System.Drawing;

namespace DealCloud.AddIn.Common.Controls.Common
{
    public partial class FormHeaderExt : UserControlExt
	{
        protected override void InitLayout()
        {
            base.InitLayout();
            this.BackColor = ThemeStyle.Current.Form.BorderColor;
        }

        public FormHeaderExt()
		{
			InitializeComponent();
        }

	    protected override void OnResize(EventArgs e)
	    {
	        base.OnResize(e);
            pictureBox1.Size = new Size(pictureBox1.Height, pictureBox1.Height);
        }

	    protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            pictureBox1.Size = new Size(pictureBox1.Height, pictureBox1.Height);
        }
    }
}
