using System.Drawing;

namespace DealCloud.AddIn.Common.Controls
{
    partial class LoginDialog
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
            this.webBrowser = new DealCloud.AddIn.Common.Controls.WebBrowserExt();
            this.SuspendLayout();
            // 
            // FormPanelExt
            // 
            this.FormPanelExt.Size = new System.Drawing.Size(625, 33);
            // 
            // webBrowser
            // 
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.IsLoading = false;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.Location = new System.Drawing.Point(5, 33);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.RestrictedUri = null;
            this.webBrowser.ScrollBarsEnabled = false;
            this.webBrowser.Size = new System.Drawing.Size(625, 512);
            this.webBrowser.TabIndex = 3;
            this.webBrowser.TimeoutInterval = -1;
            // 
            // LoginDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(635, 550);
            this.Controls.Add(this.webBrowser);
            this.DisableCloseOnEscape = true;
            this.Name = "LoginDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " ";
            this.Title = " ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.loginDialog_FormClosing);
            this.Load += new System.EventHandler(this.loginDialog_Load);
            this.Controls.SetChildIndex(this.FormPanelExt, 0);
            this.Controls.SetChildIndex(this.webBrowser, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DealCloud.AddIn.Common.Controls.WebBrowserExt webBrowser;
    }
}