namespace DealCloud.AddIn.Common.Controls
{
    partial class ReportPreviewDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportPreviewDialog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.userDetailsLabel = new System.Windows.Forms.Label();
            this.userDetailsTextBox = new System.Windows.Forms.TextBox();
            this.systemDetailsLabel = new System.Windows.Forms.Label();
            this.systemDetailsTextBox = new System.Windows.Forms.TextBox();
            this.logFilesLabel = new System.Windows.Forms.Label();
            this.logFilesListView = new DealCloud.AddIn.Common.Controls.ListViewExt();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.cancelButton = new DealCloud.AddIn.Common.Controls.ButtonExt();
            this.tableLayoutPanel1.SuspendLayout();
            this.buttonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormPanelExt
            // 
            this.FormPanelExt.Size = new System.Drawing.Size(573, 33);
            this.FormPanelExt.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.userDetailsLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.userDetailsTextBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.systemDetailsLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.systemDetailsTextBox, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.logFilesLabel, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.logFilesListView, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.buttonsPanel, 0, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 33);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(573, 348);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // userDetailsLabel
            // 
            this.userDetailsLabel.AutoSize = true;
            this.userDetailsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userDetailsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.userDetailsLabel.Location = new System.Drawing.Point(7, 4);
            this.userDetailsLabel.Name = "userDetailsLabel";
            this.userDetailsLabel.Size = new System.Drawing.Size(559, 15);
            this.userDetailsLabel.TabIndex = 0;
            this.userDetailsLabel.Text = "User Details";
            // 
            // userDetailsTextBox
            // 
            this.userDetailsTextBox.AcceptsReturn = true;
            this.userDetailsTextBox.AcceptsTab = true;
            this.userDetailsTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.userDetailsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.userDetailsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userDetailsTextBox.Location = new System.Drawing.Point(7, 22);
            this.userDetailsTextBox.Multiline = true;
            this.userDetailsTextBox.Name = "userDetailsTextBox";
            this.userDetailsTextBox.ReadOnly = true;
            this.userDetailsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.userDetailsTextBox.Size = new System.Drawing.Size(559, 78);
            this.userDetailsTextBox.TabIndex = 1;
            // 
            // systemDetailsLabel
            // 
            this.systemDetailsLabel.AutoSize = true;
            this.systemDetailsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.systemDetailsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.systemDetailsLabel.Location = new System.Drawing.Point(7, 103);
            this.systemDetailsLabel.Name = "systemDetailsLabel";
            this.systemDetailsLabel.Size = new System.Drawing.Size(559, 15);
            this.systemDetailsLabel.TabIndex = 2;
            this.systemDetailsLabel.Text = "System and Application Details";
            // 
            // systemDetailsTextBox
            // 
            this.systemDetailsTextBox.AcceptsReturn = true;
            this.systemDetailsTextBox.AcceptsTab = true;
            this.systemDetailsTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.systemDetailsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.systemDetailsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.systemDetailsTextBox.Location = new System.Drawing.Point(7, 121);
            this.systemDetailsTextBox.Multiline = true;
            this.systemDetailsTextBox.Name = "systemDetailsTextBox";
            this.systemDetailsTextBox.ReadOnly = true;
            this.systemDetailsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.systemDetailsTextBox.Size = new System.Drawing.Size(559, 78);
            this.systemDetailsTextBox.TabIndex = 3;
            // 
            // logFilesLabel
            // 
            this.logFilesLabel.AutoSize = true;
            this.logFilesLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logFilesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.logFilesLabel.Location = new System.Drawing.Point(7, 202);
            this.logFilesLabel.Name = "logFilesLabel";
            this.logFilesLabel.Size = new System.Drawing.Size(559, 15);
            this.logFilesLabel.TabIndex = 4;
            this.logFilesLabel.Text = "Log Files";
            // 
            // logFilesListView
            // 
            this.logFilesListView.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.logFilesListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logFilesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logFilesListView.EmptyText = null;
            this.logFilesListView.LargeImageList = this.imageList;
            this.logFilesListView.Location = new System.Drawing.Point(7, 220);
            this.logFilesListView.MultiSelect = false;
            this.logFilesListView.Name = "logFilesListView";
            this.logFilesListView.Size = new System.Drawing.Size(559, 80);
            this.logFilesListView.SmallImageList = this.imageList;
            this.logFilesListView.TabIndex = 5;
            this.logFilesListView.UseCompatibleStateImageBehavior = false;
            this.logFilesListView.View = System.Windows.Forms.View.Tile;
            this.logFilesListView.ItemActivate += new System.EventHandler(this.logFilesListView_ItemActivate);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "file_empty_m.png");
            // 
            // buttonsPanel
            // 
            this.buttonsPanel.Controls.Add(this.cancelButton);
            this.buttonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonsPanel.Location = new System.Drawing.Point(4, 303);
            this.buttonsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(565, 41);
            this.buttonsPanel.TabIndex = 6;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Location = new System.Drawing.Point(478, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(87, 37);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.TabStop = false;
            this.cancelButton.Text = global::DealCloud.AddIn.Common.Properties.Resources.Cancel;
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // ReportPreviewDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 386);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DisableCloseOnEscape = true;
            this.Name = "ReportPreviewDialog";
            this.Resizable = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Issue Report - Preview";
            this.Title = "Issue Report - Preview";
            this.Controls.SetChildIndex(this.FormPanelExt, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.buttonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label userDetailsLabel;
        private System.Windows.Forms.Label systemDetailsLabel;
        private System.Windows.Forms.Label logFilesLabel;
        private ListViewExt logFilesListView;
        private System.Windows.Forms.Panel buttonsPanel;
        protected ButtonExt cancelButton;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TextBox userDetailsTextBox;
        private System.Windows.Forms.TextBox systemDetailsTextBox;
    }
}