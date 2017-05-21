namespace TestDriveWinForms
{
    partial class Form1
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
            this.originalTextRichTextBox = new System.Windows.Forms.RichTextBox();
            this.translatedTextRichTextBox = new System.Windows.Forms.RichTextBox();
            this.translateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // originalTextRichTextBox
            // 
            this.originalTextRichTextBox.Location = new System.Drawing.Point(12, 12);
            this.originalTextRichTextBox.Name = "originalTextRichTextBox";
            this.originalTextRichTextBox.Size = new System.Drawing.Size(260, 96);
            this.originalTextRichTextBox.TabIndex = 0;
            this.originalTextRichTextBox.Text = "";
            // 
            // translatedTextRichTextBox
            // 
            this.translatedTextRichTextBox.Location = new System.Drawing.Point(12, 123);
            this.translatedTextRichTextBox.Name = "translatedTextRichTextBox";
            this.translatedTextRichTextBox.Size = new System.Drawing.Size(260, 96);
            this.translatedTextRichTextBox.TabIndex = 1;
            this.translatedTextRichTextBox.Text = "";
            // 
            // translateButton
            // 
            this.translateButton.Location = new System.Drawing.Point(12, 226);
            this.translateButton.Name = "translateButton";
            this.translateButton.Size = new System.Drawing.Size(260, 23);
            this.translateButton.TabIndex = 2;
            this.translateButton.Text = "TRANSLATE";
            this.translateButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.translateButton);
            this.Controls.Add(this.translatedTextRichTextBox);
            this.Controls.Add(this.originalTextRichTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox originalTextRichTextBox;
        private System.Windows.Forms.RichTextBox translatedTextRichTextBox;
        private System.Windows.Forms.Button translateButton;
    }
}

