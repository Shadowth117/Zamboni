
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
            this.batchListIceContentsButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.compressCheckBox = new System.Windows.Forms.CheckBox();
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
            this.packIceButton.Location = new System.Drawing.Point(13, 101);
            this.packIceButton.Name = "packIceButton";
            this.packIceButton.Size = new System.Drawing.Size(97, 23);
            this.packIceButton.TabIndex = 4;
            this.packIceButton.Text = "Pack Ice";
            this.packIceButton.UseVisualStyleBackColor = true;
            this.packIceButton.Click += new System.EventHandler(this.packIceButton_Click);
            // 
            // batchListIceContentsButton
            // 
            this.batchListIceContentsButton.Location = new System.Drawing.Point(13, 72);
            this.batchListIceContentsButton.Name = "batchListIceContentsButton";
            this.batchListIceContentsButton.Size = new System.Drawing.Size(138, 23);
            this.batchListIceContentsButton.TabIndex = 5;
            this.batchListIceContentsButton.Text = "Batch List Ice Contents";
            this.batchListIceContentsButton.UseVisualStyleBackColor = true;
            this.batchListIceContentsButton.Click += new System.EventHandler(this.batchListIceContentsButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(116, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Warning, batch operations";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(116, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "on all Ice take a LONG time.";
            // 
            // compressCheckBox
            // 
            this.compressCheckBox.AutoSize = true;
            this.compressCheckBox.Checked = true;
            this.compressCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.compressCheckBox.Location = new System.Drawing.Point(157, 76);
            this.compressCheckBox.Name = "compressCheckBox";
            this.compressCheckBox.Size = new System.Drawing.Size(72, 17);
            this.compressCheckBox.TabIndex = 8;
            this.compressCheckBox.Text = "Compress";
            this.compressCheckBox.UseVisualStyleBackColor = true;
            // 
            // NicerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(255, 129);
            this.Controls.Add(this.compressCheckBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.batchListIceContentsButton);
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
        private System.Windows.Forms.Button batchListIceContentsButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox compressCheckBox;
    }
}