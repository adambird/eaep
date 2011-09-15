namespace eaep.generator
{
	partial class EAEPGenerator
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
            this.hostBox = new System.Windows.Forms.TextBox();
            this.appBox = new System.Windows.Forms.TextBox();
            this.eventBox = new System.Windows.Forms.TextBox();
            this.paramsBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timestampBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.multiSendButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // hostBox
            // 
            this.hostBox.Location = new System.Drawing.Point(90, 47);
            this.hostBox.Name = "hostBox";
            this.hostBox.Size = new System.Drawing.Size(100, 20);
            this.hostBox.TabIndex = 0;
            this.hostBox.Text = "host01";
            // 
            // appBox
            // 
            this.appBox.Location = new System.Drawing.Point(90, 73);
            this.appBox.Name = "appBox";
            this.appBox.Size = new System.Drawing.Size(100, 20);
            this.appBox.TabIndex = 1;
            this.appBox.Text = "webapp";
            // 
            // eventBox
            // 
            this.eventBox.Location = new System.Drawing.Point(90, 99);
            this.eventBox.Name = "eventBox";
            this.eventBox.Size = new System.Drawing.Size(100, 20);
            this.eventBox.TabIndex = 2;
            this.eventBox.Text = "PageLoad";
            // 
            // paramsBox
            // 
            this.paramsBox.Location = new System.Drawing.Point(90, 125);
            this.paramsBox.Multiline = true;
            this.paramsBox.Name = "paramsBox";
            this.paramsBox.Size = new System.Drawing.Size(149, 74);
            this.paramsBox.TabIndex = 3;
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(90, 205);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(68, 28);
            this.sendButton.TabIndex = 4;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Timestamp";
            // 
            // timestampBox
            // 
            this.timestampBox.Location = new System.Drawing.Point(90, 21);
            this.timestampBox.Name = "timestampBox";
            this.timestampBox.Size = new System.Drawing.Size(149, 20);
            this.timestampBox.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Host";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Application";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(49, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Event";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Parameters";
            // 
            // multiSendButton
            // 
            this.multiSendButton.Location = new System.Drawing.Point(164, 205);
            this.multiSendButton.Name = "multiSendButton";
            this.multiSendButton.Size = new System.Drawing.Size(68, 28);
            this.multiSendButton.TabIndex = 11;
            this.multiSendButton.Text = "Multi Send";
            this.multiSendButton.UseVisualStyleBackColor = true;
            this.multiSendButton.Click += new System.EventHandler(this.multiSendButton_Click);
            // 
            // EAEPGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 250);
            this.Controls.Add(this.multiSendButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.timestampBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.paramsBox);
            this.Controls.Add(this.eventBox);
            this.Controls.Add(this.appBox);
            this.Controls.Add(this.hostBox);
            this.Name = "EAEPGenerator";
            this.Text = "EAEP Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox hostBox;
		private System.Windows.Forms.TextBox appBox;
		private System.Windows.Forms.TextBox eventBox;
		private System.Windows.Forms.TextBox paramsBox;
		private System.Windows.Forms.Button sendButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox timestampBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button multiSendButton;
	}
}

