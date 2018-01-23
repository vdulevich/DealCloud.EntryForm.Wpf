namespace DealCloud.AddIn.Common.Controls
{
    partial class QueryBuilderDialog
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
            this.webBrowser = new DealCloud.AddIn.Common.Controls.WebBrowserExt();
            this.SuspendLayout();
            // 
            // FormPanelExt
            // 
            this.FormPanelExt.Size = new System.Drawing.Size(1163, 33);
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(5, 33);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScrollBarsEnabled = false;
            this.webBrowser.Size = new System.Drawing.Size(1163, 759);
            this.webBrowser.TabIndex = 11;
            this.webBrowser.TimeoutInterval = -1;
            // 
            // QueryBuilderDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1173, 797);
            this.Controls.Add(this.webBrowser);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "QueryBuilderDialog";
            this.Resizable = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Title = "View Builder";
            this.VisibleChanged += new System.EventHandler(this.queryBuilderDialog_VisibleChanged);
            this.Controls.SetChildIndex(this.FormPanelExt, 0);
            this.Controls.SetChildIndex(this.webBrowser, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WebBrowserExt webBrowser;
    }
}
