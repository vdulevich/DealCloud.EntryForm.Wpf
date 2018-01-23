namespace DealCloud.AddIn.Common.Controls
{
    partial class LoadingErrorControl
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
            this.lblRefresh = new System.Windows.Forms.LinkLabel();
            this.pnlFailed = new System.Windows.Forms.TableLayoutPanel();
            this.lblError = new System.Windows.Forms.Label();
            this.pnlFailed.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRefresh
            // 
            this.lblRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblRefresh.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRefresh.Image = global::DealCloud.AddIn.Common.Properties.Resources.refresh_m;
            this.lblRefresh.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblRefresh.LinkColor = System.Drawing.Color.White;
            this.lblRefresh.Location = new System.Drawing.Point(3, 191);
            this.lblRefresh.Name = "lblRefresh";
            this.lblRefresh.Size = new System.Drawing.Size(619, 48);
            this.lblRefresh.TabIndex = 13;
            this.lblRefresh.TabStop = true;
            this.lblRefresh.Text = "Refresh";
            this.lblRefresh.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            //this.lblRefresh.Click += new System.EventHandler(this.picRefresh_Click);
            // 
            // pnlFailed
            // 
            this.pnlFailed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(47)))), ((int)(((byte)(93)))));
            this.pnlFailed.ColumnCount = 1;
            this.pnlFailed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlFailed.Controls.Add(this.lblError, 0, 1);
            this.pnlFailed.Controls.Add(this.lblRefresh, 0, 0);
            this.pnlFailed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFailed.Location = new System.Drawing.Point(5, 33);
            this.pnlFailed.Name = "pnlFailed";
            this.pnlFailed.Padding = new System.Windows.Forms.Padding(0, 0, 0, 33);
            this.pnlFailed.RowCount = 2;
            this.pnlFailed.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlFailed.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlFailed.Size = new System.Drawing.Size(625, 512);
            this.pnlFailed.TabIndex = 14;
            this.pnlFailed.Visible = false;
            // 
            // lblError
            // 
            this.lblError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblError.AutoSize = true;
            this.lblError.ForeColor = System.Drawing.Color.White;
            this.lblError.Location = new System.Drawing.Point(3, 239);
            this.lblError.Name = "lblError";
            this.lblError.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblError.Size = new System.Drawing.Size(619, 240);
            this.lblError.TabIndex = 15;
            this.lblError.Text = "Error message";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // LoginDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(635, 550);
            this.Controls.Add(this.pnlFailed);
            this.Name = "LoginErrorControl";
            this.Controls.SetChildIndex(this.pnlFailed, 0);
            this.pnlFailed.ResumeLayout(false);
            this.pnlFailed.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lblRefresh;
        private System.Windows.Forms.TableLayoutPanel pnlFailed;
        private System.Windows.Forms.Label lblError;
    }
}
