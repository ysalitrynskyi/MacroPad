namespace RSoft.MacroPad.Forms
{
	partial class ManualKeyForm
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
            components = new System.ComponentModel.Container();
            gbModifiers = new System.Windows.Forms.GroupBox();
            cbShiftL = new System.Windows.Forms.CheckBox();
            cbCtrlL = new System.Windows.Forms.CheckBox();
            cbWinR = new System.Windows.Forms.CheckBox();
            cbShiftR = new System.Windows.Forms.CheckBox();
            cbAltR = new System.Windows.Forms.CheckBox();
            cbAltL = new System.Windows.Forms.CheckBox();
            cbWinL = new System.Windows.Forms.CheckBox();
            cbCtrlR = new System.Windows.Forms.CheckBox();
            button1 = new System.Windows.Forms.Button();
            listBox1 = new System.Windows.Forms.ListBox();
            keyStrokeBindingSource = new System.Windows.Forms.BindingSource(components);
            gbModifiers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)keyStrokeBindingSource).BeginInit();
            SuspendLayout();
            // 
            // gbModifiers
            // 
            gbModifiers.Controls.Add(cbShiftL);
            gbModifiers.Controls.Add(cbCtrlL);
            gbModifiers.Controls.Add(cbWinR);
            gbModifiers.Controls.Add(cbShiftR);
            gbModifiers.Controls.Add(cbAltR);
            gbModifiers.Controls.Add(cbAltL);
            gbModifiers.Controls.Add(cbWinL);
            gbModifiers.Controls.Add(cbCtrlR);
            gbModifiers.Location = new System.Drawing.Point(225, 12);
            gbModifiers.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            gbModifiers.Name = "gbModifiers";
            gbModifiers.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            gbModifiers.Size = new System.Drawing.Size(372, 210);
            gbModifiers.TabIndex = 4;
            gbModifiers.TabStop = false;
            gbModifiers.Text = "Modifiers";
            // 
            // cbShiftL
            // 
            cbShiftL.AutoSize = true;
            cbShiftL.Location = new System.Drawing.Point(12, 40);
            cbShiftL.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            cbShiftL.Name = "cbShiftL";
            cbShiftL.Size = new System.Drawing.Size(118, 29);
            cbShiftL.TabIndex = 1;
            cbShiftL.Text = "Left SHIFT";
            cbShiftL.UseVisualStyleBackColor = true;
            cbShiftL.Click += ModifierChanged;
            // 
            // cbCtrlL
            // 
            cbCtrlL.AutoSize = true;
            cbCtrlL.Location = new System.Drawing.Point(12, 84);
            cbCtrlL.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            cbCtrlL.Name = "cbCtrlL";
            cbCtrlL.Size = new System.Drawing.Size(111, 29);
            cbCtrlL.TabIndex = 1;
            cbCtrlL.Text = "Left CTRL";
            cbCtrlL.UseVisualStyleBackColor = true;
            cbCtrlL.Click += ModifierChanged;
            // 
            // cbWinR
            // 
            cbWinR.AutoSize = true;
            cbWinR.Location = new System.Drawing.Point(224, 172);
            cbWinR.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            cbWinR.Name = "cbWinR";
            cbWinR.Size = new System.Drawing.Size(120, 29);
            cbWinR.TabIndex = 1;
            cbWinR.Text = "Right WIN";
            cbWinR.UseVisualStyleBackColor = true;
            cbWinR.Click += ModifierChanged;
            // 
            // cbShiftR
            // 
            cbShiftR.AutoSize = true;
            cbShiftR.Location = new System.Drawing.Point(224, 40);
            cbShiftR.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            cbShiftR.Name = "cbShiftR";
            cbShiftR.Size = new System.Drawing.Size(131, 29);
            cbShiftR.TabIndex = 1;
            cbShiftR.Text = "Right SHIFT";
            cbShiftR.UseVisualStyleBackColor = true;
            cbShiftR.Click += ModifierChanged;
            // 
            // cbAltR
            // 
            cbAltR.AutoSize = true;
            cbAltR.Location = new System.Drawing.Point(224, 128);
            cbAltR.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            cbAltR.Name = "cbAltR";
            cbAltR.Size = new System.Drawing.Size(113, 29);
            cbAltR.TabIndex = 1;
            cbAltR.Text = "Right ALT";
            cbAltR.UseVisualStyleBackColor = true;
            cbAltR.Click += ModifierChanged;
            // 
            // cbAltL
            // 
            cbAltL.AutoSize = true;
            cbAltL.Location = new System.Drawing.Point(12, 128);
            cbAltL.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            cbAltL.Name = "cbAltL";
            cbAltL.Size = new System.Drawing.Size(100, 29);
            cbAltL.TabIndex = 1;
            cbAltL.Text = "Left ALT";
            cbAltL.UseVisualStyleBackColor = true;
            cbAltL.Click += ModifierChanged;
            // 
            // cbWinL
            // 
            cbWinL.AutoSize = true;
            cbWinL.Location = new System.Drawing.Point(12, 172);
            cbWinL.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            cbWinL.Name = "cbWinL";
            cbWinL.Size = new System.Drawing.Size(107, 29);
            cbWinL.TabIndex = 1;
            cbWinL.Text = "Left WIN";
            cbWinL.UseVisualStyleBackColor = true;
            cbWinL.Click += ModifierChanged;
            // 
            // cbCtrlR
            // 
            cbCtrlR.AutoSize = true;
            cbCtrlR.Location = new System.Drawing.Point(224, 84);
            cbCtrlR.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            cbCtrlR.Name = "cbCtrlR";
            cbCtrlR.Size = new System.Drawing.Size(124, 29);
            cbCtrlR.TabIndex = 1;
            cbCtrlR.Text = "Right CTRL";
            cbCtrlR.UseVisualStyleBackColor = true;
            cbCtrlR.Click += ModifierChanged;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(490, 397);
            button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(107, 38);
            button1.TabIndex = 5;
            button1.Text = "Select";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 25;
            listBox1.Location = new System.Drawing.Point(12, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(205, 429);
            listBox1.TabIndex = 6;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // keyStrokeBindingSource
            // 
            keyStrokeBindingSource.DataSource = typeof(Model.KeyStroke);
            // 
            // ManualKeyForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(611, 450);
            Controls.Add(listBox1);
            Controls.Add(button1);
            Controls.Add(gbModifiers);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "ManualKeyForm";
            ShowInTaskbar = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Select A Key";
            gbModifiers.ResumeLayout(false);
            gbModifiers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)keyStrokeBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.GroupBox gbModifiers;
		private System.Windows.Forms.CheckBox cbShiftL;
		private System.Windows.Forms.CheckBox cbCtrlL;
		private System.Windows.Forms.CheckBox cbWinR;
		private System.Windows.Forms.CheckBox cbShiftR;
		private System.Windows.Forms.CheckBox cbAltR;
		private System.Windows.Forms.CheckBox cbAltL;
		private System.Windows.Forms.CheckBox cbWinL;
		private System.Windows.Forms.CheckBox cbCtrlR;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.BindingSource keyStrokeBindingSource;
	}
}