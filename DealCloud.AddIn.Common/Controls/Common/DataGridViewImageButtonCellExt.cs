using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DealCloud.AddIn.Common.Controls
{
    public abstract class DataGridViewImageButtonCellExt : DataGridViewButtonCell
    {
        private PushButtonState _buttonState; // What is the button state
        protected Image _buttonImageHot;      // The hot image
        protected Image _buttonImageNormal;   // The normal image
        protected Image _buttonImageDisabled; // The disabled image
        private readonly int _buttonImageOffset;       // The amount of offset or border around the image

        protected DataGridViewImageButtonCellExt()
        {
            _buttonState = PushButtonState.Disabled;
            _buttonImageOffset = 3;
            LoadImages();
        }

        public PushButtonState ButtonState
        {
            get { return _buttonState; }
            set { _buttonState = value; }
        }

        public Image ButtonImage
        {
            get
            {
                switch (_buttonState)
                {
                    case PushButtonState.Disabled:
                        return _buttonImageDisabled;
                    case PushButtonState.Hot:
                        return _buttonImageHot;
                    case PushButtonState.Normal:
                    case PushButtonState.Pressed:
                    case PushButtonState.Default:
                        return _buttonImageNormal;

                    default:
                        return _buttonImageNormal;
                }
            }
        }

        private int _hoveredRow = int.MinValue;

        protected override void OnMouseEnter(int rowIndex)
        {

            base.OnMouseEnter(rowIndex);
            _hoveredRow = rowIndex;
            DataGridView.InvalidateCell(ColumnIndex, rowIndex);
        }

        protected override void OnMouseLeave(int rowIndex)
        {
            
            base.OnMouseLeave(rowIndex);
            _hoveredRow = int.MinValue;
            DataGridView.InvalidateCell(ColumnIndex, rowIndex);
        }

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value,
            object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // Draw the cell background, if specified.
            if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
            {
                bool flag = (elementState & DataGridViewElementStates.Selected) > DataGridViewElementStates.None;
                using (SolidBrush brush2 = new SolidBrush(flag ? cellStyle.SelectionBackColor : cellStyle.BackColor))
                {
                    graphics.FillRectangle(brush2, cellBounds);
                }
            }

            // Draw the cell borders, if specified.
            if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
            {
                PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
            }

			// Draw the cell button.
			PaintButton(graphics, rowIndex, cellBounds, advancedBorderStyle);
        }

	    protected virtual void PaintButton(Graphics graphics, int rowIndex, Rectangle cellBounds, DataGridViewAdvancedBorderStyle advancedBorderStyle)
	    {
			Rectangle buttonArea = cellBounds;
			Rectangle buttonAdjustment = BorderWidths(advancedBorderStyle);

			buttonArea.X += buttonAdjustment.X;
			buttonArea.Y += buttonAdjustment.Y;
			buttonArea.Height -= buttonAdjustment.Height;
			buttonArea.Width -= buttonAdjustment.Width;

			Rectangle imageAdjustment;
			if (buttonArea.Width > buttonArea.Height)
			{
				float k = (buttonArea.Height - 2 * _buttonImageOffset) / (float)ButtonImage.Height;
				imageAdjustment = new Rectangle(0, 0, (int)(ButtonImage.Width * k), (int)(ButtonImage.Height * k));
			}
			else
			{
				float k = (buttonArea.Width - 2 * _buttonImageOffset) / (float)ButtonImage.Width;
				imageAdjustment = new Rectangle(0, 0, (int)(ButtonImage.Width * k), (int)(ButtonImage.Height * k));
			}

			int imgWidth = Math.Min(buttonArea.Width - 2 * _buttonImageOffset, imageAdjustment.Width);
			int imgHeight = Math.Min(buttonArea.Height - 2 * _buttonImageOffset, imageAdjustment.Height);

			Rectangle imageArea = new Rectangle(
			  buttonArea.X + (buttonArea.Width - imgWidth) / 2,
			  buttonArea.Y + (buttonArea.Height - imgHeight) / 2,
			  imgWidth,
			  imgHeight);

			ButtonRenderer.DrawButton(graphics, buttonArea, ButtonImage, imageArea, false, _hoveredRow == rowIndex ? PushButtonState.Hot : ButtonState);
		}

	    public abstract void LoadImages();
    }
}
