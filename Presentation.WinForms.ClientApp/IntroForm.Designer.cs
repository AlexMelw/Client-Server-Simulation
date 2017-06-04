namespace Presentation.WinForms.ClientApp
{
    partial class IntroForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.pickupProtocolComboBox = new System.Windows.Forms.ComboBox();
            this.runClientButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pick up prefered communication type (TCP/UDP)";
            // 
            // pickupProtocolComboBox
            // 
            this.pickupProtocolComboBox.FormattingEnabled = true;
            this.pickupProtocolComboBox.Items.AddRange(new object[] {
            "UDP",
            "TCP"});
            this.pickupProtocolComboBox.Location = new System.Drawing.Point(99, 113);
            this.pickupProtocolComboBox.Name = "pickupProtocolComboBox";
            this.pickupProtocolComboBox.Size = new System.Drawing.Size(121, 21);
            this.pickupProtocolComboBox.TabIndex = 1;
            // 
            // runClientButton
            // 
            this.runClientButton.Location = new System.Drawing.Point(99, 165);
            this.runClientButton.Name = "runClientButton";
            this.runClientButton.Size = new System.Drawing.Size(121, 27);
            this.runClientButton.TabIndex = 0;
            this.runClientButton.Text = "RUN CLIENT";
            this.runClientButton.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(323, 276);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.runClientButton);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.pickupProtocolComboBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(315, 250);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Quick Configuration";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // IntroForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumAquamarine;
            this.ClientSize = new System.Drawing.Size(347, 300);
            this.Controls.Add(this.tabControl1);
            this.Name = "IntroForm";
            this.Text = "IntroForm";
            this.Load += new System.EventHandler(this.OnLoadIntroForm);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox pickupProtocolComboBox;
        private System.Windows.Forms.Button runClientButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
    }
}