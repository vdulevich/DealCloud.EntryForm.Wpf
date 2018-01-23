namespace DealCloud.AddIn.Common.Controls
{
    partial class MessageDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cancelBtn = new DealCloud.AddIn.Common.Controls.ButtonExt();
            this.continueBtn = new DealCloud.AddIn.Common.Controls.ButtonAcceptExt();
            this.mesageLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.reportButton = new DealCloud.AddIn.Common.Controls.ButtonExt();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormPanelExt
            // 
            this.FormPanelExt.Size = new System.Drawing.Size(573, 33);
            this.FormPanelExt.TabIndex = 0;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelBtn.Location = new System.Drawing.Point(352, 0);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(87, 37);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.TabStop = false;
            this.cancelBtn.Text = global::DealCloud.AddIn.Common.Properties.Resources.Cancel;
            this.cancelBtn.UseVisualStyleBackColor = false;
            // 
            // continueBtn
            // 
            this.continueBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.continueBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.continueBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.continueBtn.Location = new System.Drawing.Point(445, 0);
            this.continueBtn.Name = "continueBtn";
            this.continueBtn.Size = new System.Drawing.Size(122, 37);
            this.continueBtn.TabIndex = 2;
            this.continueBtn.TabStop = false;
            this.continueBtn.Text = "Continue";
            this.continueBtn.UseVisualStyleBackColor = false;
            this.continueBtn.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // mesageLbl
            // 
            this.mesageLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mesageLbl.AutoSize = true;
            this.mesageLbl.Location = new System.Drawing.Point(20, 3);
            this.mesageLbl.Margin = new System.Windows.Forms.Padding(20, 3, 20, 3);
            this.mesageLbl.MaximumSize = new System.Drawing.Size(525, 9999);
            this.mesageLbl.Name = "mesageLbl";
            this.mesageLbl.Size = new System.Drawing.Size(525, 76);
            this.mesageLbl.TabIndex = 0;
            this.mesageLbl.Text = "Message";
            this.mesageLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mesageLbl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblMesage_MouseDown);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.mesageLbl, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 33);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(573, 125);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.reportButton);
            this.panel1.Controls.Add(this.cancelBtn);
            this.panel1.Controls.Add(this.continueBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 85);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(567, 37);
            this.panel1.TabIndex = 1;
            // 
            // reportButton
            // 
            this.reportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.reportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reportButton.Location = new System.Drawing.Point(259, 0);
            this.reportButton.Name = "reportButton";
            this.reportButton.Size = new System.Drawing.Size(87, 37);
            this.reportButton.TabIndex = 0;
            this.reportButton.TabStop = false;
            this.reportButton.Text = "Report";
            this.reportButton.UseVisualStyleBackColor = false;
            this.reportButton.Visible = false;
            this.reportButton.Click += new System.EventHandler(this.reportButton_Click);
            // 
            // MessageDialog
            // 
            this.AcceptButton = this.cancelBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(583, 163);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DisableCloseOnEscape = true;
            this.Name = "MessageDialog";
            this.Resizable = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Message";
            this.Load += new System.EventHandler(this.messageDialog_Load);
            this.Controls.SetChildIndex(this.FormPanelExt, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        protected System.Windows.Forms.Label mesageLbl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        protected DealCloud.AddIn.Common.Controls.ButtonExt cancelBtn;
        protected DealCloud.AddIn.Common.Controls.ButtonAcceptExt continueBtn;
        protected ButtonExt reportButton;
    }
}