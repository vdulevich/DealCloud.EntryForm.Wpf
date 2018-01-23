using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Controls
{
	partial class FormExt
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FormPanelExt = new DealCloud.AddIn.Common.Controls.Common.FormHeaderExt();
            this.SuspendLayout();
            // 
            // FormPanelExt
            // 
            this.FormPanelExt.AutoSize = true;
            this.FormPanelExt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(47)))), ((int)(((byte)(93)))));
            this.FormPanelExt.Dock = System.Windows.Forms.DockStyle.Top;
            this.FormPanelExt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormPanelExt.Location = new System.Drawing.Point(5, 0);
            this.FormPanelExt.Name = "FormPanelExt";
            this.FormPanelExt.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.FormPanelExt.Size = new System.Drawing.Size(641, 26);
            this.FormPanelExt.TabIndex = 16;
            // 
            // FormExt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(651, 536);
            this.Controls.Add(this.FormPanelExt);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormExt";
            this.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.PictureBox pictureBox1;
        public Common.FormHeaderExt FormPanelExt;
    }
}
