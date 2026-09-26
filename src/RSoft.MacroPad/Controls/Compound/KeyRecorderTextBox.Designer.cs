namespace RSoft.MacroPad.Controls.Compound
{
    partial class KeyRecorderTextBox
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>


        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            checkBox1 = new System.Windows.Forms.CheckBox();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            button3 = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            button1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            button1.Location = new System.Drawing.Point(1153, 5);
            button1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(39, 45);
            button1.TabIndex = 1;
            button1.Text = "X";
            toolTip1.SetToolTip(button1, "Clear all");
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            button2.Location = new System.Drawing.Point(1153, 62);
            button2.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(39, 45);
            button2.TabIndex = 1;
            button2.Text = "◄";
            toolTip1.SetToolTip(button2, "Delete last");
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.BackColor = System.Drawing.Color.White;
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1135, 239);
            panel1.TabIndex = 2;
            // 
            // checkBox1
            // 
            checkBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
            checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            checkBox1.ForeColor = System.Drawing.Color.Red;
            checkBox1.Location = new System.Drawing.Point(1153, 117);
            checkBox1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(39, 45);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "●";
            checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip1.SetToolTip(checkBox1, "Record");
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // button3
            // 
            button3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            button3.Location = new System.Drawing.Point(1153, 172);
            button3.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(39, 45);
            button3.TabIndex = 3;
            button3.Text = "≡";
            toolTip1.SetToolTip(button3, "Manually Enter Key");
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // KeyRecorderTextBox
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(button3);
            Controls.Add(checkBox1);
            Controls.Add(panel1);
            Controls.Add(button2);
            Controls.Add(button1);
            Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            MinimumSize = new System.Drawing.Size(667, 153);
            Name = "KeyRecorderTextBox";
            Size = new System.Drawing.Size(1197, 240);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button3;
    }
}
