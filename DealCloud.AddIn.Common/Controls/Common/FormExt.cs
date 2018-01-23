using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using DealCloud.AddIn.Common.Utils;

namespace DealCloud.AddIn.Common.Controls
{
    [ComVisible(true)]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public partial class FormExt : Form
    {
        public bool Resizable { get; set; }

        public LoadingFormExtUtil LoadingUtil { get; set; }

        public string Title
        {
            get { return FormPanelExt.lblTitle.Text; }
            set
            {
                FormPanelExt.lblTitle.Visible = !string.IsNullOrEmpty(value);
                FormPanelExt.lblTitle.Text = value;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        public bool ShowBorder { get; set; }

        public bool DisableCloseOnEscape { get; set; }

        public PictureBox CloseLabel => FormPanelExt.pictureBox1;

        private const int
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17;

        const int BorderWidth = 5;

        Rectangle TopBorder => new Rectangle(0, 0, this.ClientSize.Width, BorderWidth);
        Rectangle LeftBorder => new Rectangle(0, 0, BorderWidth, this.ClientSize.Height);
        Rectangle BottomBorder => new Rectangle(0, this.ClientSize.Height - BorderWidth, this.ClientSize.Width, BorderWidth);
        Rectangle RightBorder => new Rectangle(this.ClientSize.Width - BorderWidth, 0, BorderWidth, this.ClientSize.Height);

        Rectangle TopLeft => new Rectangle(0, 0, BorderWidth, BorderWidth);
        Rectangle TopRight => new Rectangle(this.ClientSize.Width - BorderWidth, 0, BorderWidth, BorderWidth);
        Rectangle BottomLeft => new Rectangle(0, this.ClientSize.Height - BorderWidth, BorderWidth, BorderWidth);
        Rectangle BottomRight => new Rectangle(this.ClientSize.Width - BorderWidth, this.ClientSize.Height - BorderWidth, BorderWidth, BorderWidth);

        public FormExt()
        {
            InitializeComponent();
            InitializeControls();
        }

        public void ShowHandled()
        {
            Show(new NativeWindowAssignHandle());
        }

        public DialogResult ShowHandledDialog()
        {
            return ShowDialog(new NativeWindowAssignHandle());
        }

        private void InitializeControls()
        {
            Resizable = false;
            ShowBorder = true;
            LoadingUtil = new LoadingFormExtUtil(this);
            FormPanelExt.lblTitle.MouseDown += control_MouseDown;
            FormPanelExt.tableLayoutPanel1.MouseDown += control_MouseDown;
            FormPanelExt.MouseDown += control_MouseDown;
            FormPanelExt.pictureBox1.Click += lblClose_Click;
        }

        private void control_MouseDown(object sender, MouseEventArgs e)
        {
            WinApi.ReleaseCapture();
            WinApi.SendMessage(this.Handle, WinApi.WM_NCLBUTTONDOWN, WinApi.HT_CAPTION, 0);
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (ShowBorder)
            {
                Color color = ThemeStyle.Current.Form.BorderColor;
                ControlPaint.DrawBorder(
                    e.Graphics, ClientRectangle,
                    color, Padding.Left, ButtonBorderStyle.Solid,
                    color, 0, ButtonBorderStyle.None,
                    color, Padding.Right, ButtonBorderStyle.Solid,
                    color, Padding.Bottom, ButtonBorderStyle.Solid);
            }
        }

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);
            if (Resizable && ShowBorder)
            {
                if (message.Msg == 0x84) // WM_NCHITTEST
                {
                    var cursor = this.PointToClient(Cursor.Position);

                    if (TopLeft.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                    else if (TopRight.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                    else if (BottomLeft.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                    else if (BottomRight.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;

                    else if (TopBorder.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                    else if (LeftBorder.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                    else if (RightBorder.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                    else if (BottomBorder.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape && !DisableCloseOnEscape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
