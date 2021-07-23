
namespace zomFormNew
{
    partial class NicerForm
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
            this.extractIceButton = new System.Windows.Forms.Button();
            this.batchExtractIceButton = new System.Windows.Forms.Button();
            this.useGroupFolders = new System.Windows.Forms.CheckBox();
            this.searchSubCheck = new System.Windows.Forms.CheckBox();
            this.packIceButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // extractIceButton
            // 
            this.extractIceButton.Location = new System.Drawing.Point(13, 13);
            this.extractIceButton.Name = "extractIceButton";
            this.extractIceButton.Size = new System.Drawing.Size(75, 23);
            this.extractIceButton.TabIndex = 0;
            this.extractIceButton.Text = "Extract Ice";
            this.extractIceButton.UseVisualStyleBackColor = true;
            this.extractIceButton.Click += new System.EventHandler(this.extractIceButton_Click);
            // 
            // batchExtractIceButton
            // 
            this.batchExtractIceButton.Location = new System.Drawing.Point(13, 43);
            this.batchExtractIceButton.Name = "batchExtractIceButton";
            this.batchExtractIceButton.Size = new System.Drawing.Size(97, 23);
            this.batchExtractIceButton.TabIndex = 1;
            this.batchExtractIceButton.Text = "Batch Extract Ice";
            this.batchExtractIceButton.UseVisualStyleBackColor = true;
            this.batchExtractIceButton.Click += new System.EventHandler(this.batchExtractIceButton_Click);
            // 
            // useGroupFolders
            // 
            this.useGroupFolders.AutoSize = true;
            this.useGroupFolders.Checked = true;
            this.useGroupFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useGroupFolders.Location = new System.Drawing.Point(119, 18);
            this.useGroupFolders.Name = "useGroupFolders";
            this.useGroupFolders.Size = new System.Drawing.Size(109, 17);
            this.useGroupFolders.TabIndex = 2;
            this.useGroupFolders.Text = "Use group folders";
            this.useGroupFolders.UseVisualStyleBackColor = true;
            // 
            // searchSubCheck
            // 
            this.searchSubCheck.AutoSize = true;
            this.searchSubCheck.Checked = true;
            this.searchSubCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.searchSubCheck.Location = new System.Drawing.Point(119, 47);
            this.searchSubCheck.Name = "searchSubCheck";
            this.searchSubCheck.Size = new System.Drawing.Size(130, 17);
            this.searchSubCheck.TabIndex = 3;
            this.searchSubCheck.Text = "Search Subdirectories";
            this.searchSubCheck.UseVisualStyleBackColor = true;
            // 
            // packIceButton
            // 
            this.packIceButton.Location = new System.Drawing.Point(13, 73);
            this.packIceButton.Name = "packIceButton";
            this.packIceButton.Size = new System.Drawing.Size(97, 23);
            this.packIceButton.TabIndex = 4;
            this.packIceButton.Text = "Pack Ice";
            this.packIceButton.UseVisualStyleBackColor = true;
            this.packIceButton.Click += new System.EventHandler(this.packIceButton_Click);
            // 
            // NicerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(255, 107);
            this.Controls.Add(this.packIceButton);
            this.Controls.Add(this.searchSubCheck);
            this.Controls.Add(this.useGroupFolders);
            this.Controls.Add(this.batchExtractIceButton);
            this.Controls.Add(this.extractIceButton);
            this.Name = "NicerForm";
            this.Text = "Zamboni Lite";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button extractIceButton;
        private System.Windows.Forms.Button batchExtractIceButton;
        private System.Windows.Forms.CheckBox useGroupFolders;
        private System.Windows.Forms.CheckBox searchSubCheck;
        private System.Windows.Forms.Button packIceButton;
    }
}