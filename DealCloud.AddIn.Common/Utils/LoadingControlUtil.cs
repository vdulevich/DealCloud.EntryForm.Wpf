using DealCloud.AddIn.Common.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Utils
{
    public class LoadingControlUtil
    {
        private readonly Control _control;

        public LoadingControlUtil(Control control)
        {
            _control = control;
        }

        public FlowDirection Direction { get; set; }

        private PictureBox _pictureBox;

        public bool Loading
        {
            get { return _pictureBox != null && _pictureBox.Visible; }
            set { GetLoader().Visible = value; LoaderLocate(); }
        }

        private PictureBox GetLoader()
        {
            if (_pictureBox == null)
            {
                _pictureBox = _pictureBox ?? new PictureBox();
                _pictureBox.Image = Properties.Resources.load_s;
                _pictureBox.ImeMode = ImeMode.NoControl;
                _pictureBox.Size = Properties.Resources.load_s.Size;
                _pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                _pictureBox.TabIndex = 4;
                _pictureBox.TabStop = false;
                _pictureBox.Parent = _control;
                _control.Controls.Add(_pictureBox);
                _pictureBox.Location = new Point(_control.Size.Width - _pictureBox.Width - 3, (int)((_control.Size.Height - _pictureBox.Height) / 2.0));
                _control.Resize += _control_Resize;
            }
            return _pictureBox;
        }

        private void _control_Resize(object sender, EventArgs e)
        {
            LoaderLocate();
        }

        private void LoaderLocate()
        {
            if (Loading)
            {
                GetLoader().Location = new Point(_control.Size.Width - GetLoader().Width - 3, (int)((_control.Size.Height - GetLoader().Height) / 2.0));
            }
        }
    }
}
