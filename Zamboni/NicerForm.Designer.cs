
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
            this.useGroupFolders.CheckedChanged += new System.EventHandler(this.noGroupFolders_CheckedChanged);
            // 
            // NicerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 74);
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
    }
}