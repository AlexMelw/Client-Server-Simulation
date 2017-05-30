namespace Presentation.WinForms.ClientApp
{
    partial class FlowClientForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.connectToServerButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.serverPortTextBox = new System.Windows.Forms.TextBox();
            this.serverIpAddressTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.registerButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.registerPassTextBox = new System.Windows.Forms.TextBox();
            this.registerLoginTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.authButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.authPassTextBox = new System.Windows.Forms.TextBox();
            this.authLoginTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.registerNameTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(722, 539);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(714, 513);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Connect";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(248, 374);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(226, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "It\'s quite recommended to leave default values";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.connectToServerButton);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.serverPortTextBox);
            this.groupBox2.Controls.Add(this.serverIpAddressTextBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(257, 161);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 186);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Connect";
            // 
            // connectToServerButton
            // 
            this.connectToServerButton.Location = new System.Drawing.Point(18, 146);
            this.connectToServerButton.Name = "connectToServerButton";
            this.connectToServerButton.Size = new System.Drawing.Size(165, 23);
            this.connectToServerButton.TabIndex = 4;
            this.connectToServerButton.Text = "CONNECT";
            this.connectToServerButton.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Server IP Address";
            // 
            // serverPortTextBox
            // 
            this.serverPortTextBox.Location = new System.Drawing.Point(18, 112);
            this.serverPortTextBox.Name = "serverPortTextBox";
            this.serverPortTextBox.Size = new System.Drawing.Size(165, 20);
            this.serverPortTextBox.TabIndex = 3;
            // 
            // serverIpAddressTextBox
            // 
            this.serverIpAddressTextBox.Location = new System.Drawing.Point(18, 50);
            this.serverIpAddressTextBox.Name = "serverIpAddressTextBox";
            this.serverIpAddressTextBox.Size = new System.Drawing.Size(165, 20);
            this.serverIpAddressTextBox.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Server Port";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(714, 513);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Register";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.registerNameTextBox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.registerButton);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.registerPassTextBox);
            this.groupBox1.Controls.Add(this.registerLoginTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(257, 161);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 255);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Register";
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(18, 213);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(165, 23);
            this.registerButton.TabIndex = 4;
            this.registerButton.Text = "REGISTER";
            this.registerButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Login";
            // 
            // registerPassTextBox
            // 
            this.registerPassTextBox.Location = new System.Drawing.Point(18, 112);
            this.registerPassTextBox.Name = "registerPassTextBox";
            this.registerPassTextBox.Size = new System.Drawing.Size(165, 20);
            this.registerPassTextBox.TabIndex = 3;
            // 
            // registerLoginTextBox
            // 
            this.registerLoginTextBox.Location = new System.Drawing.Point(18, 50);
            this.registerLoginTextBox.Name = "registerLoginTextBox";
            this.registerLoginTextBox.Size = new System.Drawing.Size(165, 20);
            this.registerLoginTextBox.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(714, 513);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Authentication";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.authButton);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.authPassTextBox);
            this.groupBox3.Controls.Add(this.authLoginTextBox);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(257, 161);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 190);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Authentication";
            // 
            // authButton
            // 
            this.authButton.Location = new System.Drawing.Point(18, 146);
            this.authButton.Name = "authButton";
            this.authButton.Size = new System.Drawing.Size(165, 23);
            this.authButton.TabIndex = 4;
            this.authButton.Text = "REGISTER";
            this.authButton.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Login";
            // 
            // authPassTextBox
            // 
            this.authPassTextBox.Location = new System.Drawing.Point(18, 112);
            this.authPassTextBox.Name = "authPassTextBox";
            this.authPassTextBox.Size = new System.Drawing.Size(165, 20);
            this.authPassTextBox.TabIndex = 3;
            // 
            // authLoginTextBox
            // 
            this.authLoginTextBox.Location = new System.Drawing.Point(18, 50);
            this.authLoginTextBox.Name = "authLoginTextBox";
            this.authLoginTextBox.Size = new System.Drawing.Size(165, 20);
            this.authLoginTextBox.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 93);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Password";
            // 
            // registerNameTextBox
            // 
            this.registerNameTextBox.Location = new System.Drawing.Point(18, 171);
            this.registerNameTextBox.Name = "registerNameTextBox";
            this.registerNameTextBox.Size = new System.Drawing.Size(165, 20);
            this.registerNameTextBox.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 152);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Full Name";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox6);
            this.tabPage4.Controls.Add(this.groupBox5);
            this.tabPage4.Controls.Add(this.groupBox4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(714, 513);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Translator";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(714, 513);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Chat Room";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(18, 22);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(434, 146);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "English",
            "Romanian",
            "Russian"});
            this.comboBox1.Location = new System.Drawing.Point(194, 46);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "English",
            "Romanian",
            "Russian",
            "Auto Detection"});
            this.comboBox2.Location = new System.Drawing.Point(18, 46);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "From";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(191, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "To";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.comboBox1);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.comboBox2);
            this.groupBox4.Location = new System.Drawing.Point(114, 22);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(469, 89);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Translate";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(343, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 43);
            this.button1.TabIndex = 6;
            this.button1.Text = "TRANSLATE";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.richTextBox1);
            this.groupBox5.Location = new System.Drawing.Point(114, 117);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(469, 184);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Input";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.richTextBox2);
            this.groupBox6.Location = new System.Drawing.Point(114, 307);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(469, 181);
            this.groupBox6.TabIndex = 10;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Output";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(18, 19);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(434, 146);
            this.richTextBox2.TabIndex = 1;
            this.richTextBox2.Text = "";
            // 
            // FlowClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGreen;
            this.ClientSize = new System.Drawing.Size(746, 563);
            this.Controls.Add(this.tabControl1);
            this.Name = "FlowClientForm";
            this.Text = "FlowClientForm";
            this.Load += new System.EventHandler(this.FlowClientForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox registerPassTextBox;
        private System.Windows.Forms.TextBox registerLoginTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button connectToServerButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox serverPortTextBox;
        private System.Windows.Forms.TextBox serverIpAddressTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button authButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox authPassTextBox;
        private System.Windows.Forms.TextBox authLoginTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox registerNameTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.GroupBox groupBox5;
    }
}

