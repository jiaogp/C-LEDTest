namespace LEDTestSysterm
{
	partial class PmacTest
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
			this.btnCLoseForm = new System.Windows.Forms.Button();
			this.cbbMotionSelect = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.gbFunctionSelect = new System.Windows.Forms.GroupBox();
			this.btnStopMotion = new System.Windows.Forms.Button();
			this.btnJogNegative = new System.Windows.Forms.Button();
			this.btnJogPositive = new System.Windows.Forms.Button();
			this.btnGoZero = new System.Windows.Forms.Button();
			this.gbCurState = new System.Windows.Forms.GroupBox();
			this.tbZPosition = new System.Windows.Forms.TextBox();
			this.tbYPosition = new System.Windows.Forms.TextBox();
			this.tbXPosition = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cbbGoTo = new System.Windows.Forms.ComboBox();
			this.lbGoTo = new System.Windows.Forms.Label();
			this.gbFunctionSelect.SuspendLayout();
			this.gbCurState.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCLoseForm
			// 
			this.btnCLoseForm.Location = new System.Drawing.Point(236, 278);
			this.btnCLoseForm.Name = "btnCLoseForm";
			this.btnCLoseForm.Size = new System.Drawing.Size(75, 23);
			this.btnCLoseForm.TabIndex = 0;
			this.btnCLoseForm.Text = "关闭窗口";
			this.btnCLoseForm.UseVisualStyleBackColor = true;
			this.btnCLoseForm.Click += new System.EventHandler(this.btnCLoseForm_Click);
			// 
			// cbbMotionSelect
			// 
			this.cbbMotionSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbMotionSelect.FormattingEnabled = true;
			this.cbbMotionSelect.Items.AddRange(new object[] {
            "X轴",
            "Y轴",
            "Z轴"});
			this.cbbMotionSelect.Location = new System.Drawing.Point(99, 52);
			this.cbbMotionSelect.Name = "cbbMotionSelect";
			this.cbbMotionSelect.Size = new System.Drawing.Size(82, 20);
			this.cbbMotionSelect.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(39, 55);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "运动轴";
			// 
			// gbFunctionSelect
			// 
			this.gbFunctionSelect.Controls.Add(this.btnStopMotion);
			this.gbFunctionSelect.Controls.Add(this.btnJogNegative);
			this.gbFunctionSelect.Controls.Add(this.btnJogPositive);
			this.gbFunctionSelect.Controls.Add(this.btnGoZero);
			this.gbFunctionSelect.Location = new System.Drawing.Point(41, 133);
			this.gbFunctionSelect.Name = "gbFunctionSelect";
			this.gbFunctionSelect.Size = new System.Drawing.Size(219, 121);
			this.gbFunctionSelect.TabIndex = 3;
			this.gbFunctionSelect.TabStop = false;
			this.gbFunctionSelect.Text = "功能选择";
			// 
			// btnStopMotion
			// 
			this.btnStopMotion.Location = new System.Drawing.Point(127, 77);
			this.btnStopMotion.Name = "btnStopMotion";
			this.btnStopMotion.Size = new System.Drawing.Size(75, 23);
			this.btnStopMotion.TabIndex = 3;
			this.btnStopMotion.Text = "停止电机";
			this.btnStopMotion.UseVisualStyleBackColor = true;
			this.btnStopMotion.Click += new System.EventHandler(this.btnStopMotion_Click);
			// 
			// btnJogNegative
			// 
			this.btnJogNegative.Location = new System.Drawing.Point(127, 21);
			this.btnJogNegative.Name = "btnJogNegative";
			this.btnJogNegative.Size = new System.Drawing.Size(75, 23);
			this.btnJogNegative.TabIndex = 2;
			this.btnJogNegative.Text = "负向点动";
			this.btnJogNegative.UseVisualStyleBackColor = true;
			this.btnJogNegative.Click += new System.EventHandler(this.btnJogNegative_Click);
			// 
			// btnJogPositive
			// 
			this.btnJogPositive.Location = new System.Drawing.Point(20, 20);
			this.btnJogPositive.Name = "btnJogPositive";
			this.btnJogPositive.Size = new System.Drawing.Size(75, 23);
			this.btnJogPositive.TabIndex = 1;
			this.btnJogPositive.Text = "正向点动";
			this.btnJogPositive.UseVisualStyleBackColor = true;
			this.btnJogPositive.Click += new System.EventHandler(this.btnJogPositive_Click);
			// 
			// btnGoZero
			// 
			this.btnGoZero.Location = new System.Drawing.Point(20, 77);
			this.btnGoZero.Name = "btnGoZero";
			this.btnGoZero.Size = new System.Drawing.Size(75, 23);
			this.btnGoZero.TabIndex = 0;
			this.btnGoZero.Text = "回零";
			this.btnGoZero.UseVisualStyleBackColor = true;
			this.btnGoZero.Click += new System.EventHandler(this.btnGoZero_Click);
			// 
			// gbCurState
			// 
			this.gbCurState.Controls.Add(this.tbZPosition);
			this.gbCurState.Controls.Add(this.tbYPosition);
			this.gbCurState.Controls.Add(this.tbXPosition);
			this.gbCurState.Controls.Add(this.label4);
			this.gbCurState.Controls.Add(this.label3);
			this.gbCurState.Controls.Add(this.label2);
			this.gbCurState.Location = new System.Drawing.Point(293, 133);
			this.gbCurState.Name = "gbCurState";
			this.gbCurState.Size = new System.Drawing.Size(172, 121);
			this.gbCurState.TabIndex = 4;
			this.gbCurState.TabStop = false;
			this.gbCurState.Text = "实时信息";
			// 
			// tbZPosition
			// 
			this.tbZPosition.Location = new System.Drawing.Point(75, 79);
			this.tbZPosition.Name = "tbZPosition";
			this.tbZPosition.ReadOnly = true;
			this.tbZPosition.Size = new System.Drawing.Size(64, 21);
			this.tbZPosition.TabIndex = 5;
			// 
			// tbYPosition
			// 
			this.tbYPosition.Location = new System.Drawing.Point(75, 52);
			this.tbYPosition.Name = "tbYPosition";
			this.tbYPosition.ReadOnly = true;
			this.tbYPosition.Size = new System.Drawing.Size(64, 21);
			this.tbYPosition.TabIndex = 4;
			// 
			// tbXPosition
			// 
			this.tbXPosition.Location = new System.Drawing.Point(75, 20);
			this.tbXPosition.Name = "tbXPosition";
			this.tbXPosition.ReadOnly = true;
			this.tbXPosition.Size = new System.Drawing.Size(64, 21);
			this.tbXPosition.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(16, 88);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(23, 12);
			this.label4.TabIndex = 2;
			this.label4.Text = "Z轴";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(23, 12);
			this.label3.TabIndex = 1;
			this.label3.Text = "Y轴";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 23);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(23, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "X轴";
			// 
			// cbbGoTo
			// 
			this.cbbGoTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbGoTo.FormattingEnabled = true;
			this.cbbGoTo.Items.AddRange(new object[] {
            "LED1",
            "LED2",
            "LED3",
            "LED4",
            "LED5",
            "LED6",
            "LED7",
            "LED8",
            "LED9",
            "LED10",
            "起始位置"});
			this.cbbGoTo.Location = new System.Drawing.Point(321, 52);
			this.cbbGoTo.Name = "cbbGoTo";
			this.cbbGoTo.Size = new System.Drawing.Size(121, 20);
			this.cbbGoTo.TabIndex = 5;
			this.cbbGoTo.SelectedIndexChanged += new System.EventHandler(this.cbbGoTo_SelectedIndexChanged);
			// 
			// lbGoTo
			// 
			this.lbGoTo.AutoSize = true;
			this.lbGoTo.Location = new System.Drawing.Point(250, 55);
			this.lbGoTo.Name = "lbGoTo";
			this.lbGoTo.Size = new System.Drawing.Size(41, 12);
			this.lbGoTo.TabIndex = 6;
			this.lbGoTo.Text = "运动到";
			// 
			// PmacTest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(508, 327);
			this.ControlBox = false;
			this.Controls.Add(this.lbGoTo);
			this.Controls.Add(this.cbbGoTo);
			this.Controls.Add(this.gbCurState);
			this.Controls.Add(this.gbFunctionSelect);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbbMotionSelect);
			this.Controls.Add(this.btnCLoseForm);
			this.Name = "PmacTest";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "设置";
			this.gbFunctionSelect.ResumeLayout(false);
			this.gbCurState.ResumeLayout(false);
			this.gbCurState.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCLoseForm;
		private System.Windows.Forms.ComboBox cbbMotionSelect;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox gbFunctionSelect;
		private System.Windows.Forms.Button btnJogNegative;
		private System.Windows.Forms.Button btnJogPositive;
		private System.Windows.Forms.Button btnGoZero;
		private System.Windows.Forms.Button btnStopMotion;
		private System.Windows.Forms.GroupBox gbCurState;
		private System.Windows.Forms.TextBox tbZPosition;
		private System.Windows.Forms.TextBox tbYPosition;
		private System.Windows.Forms.TextBox tbXPosition;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbbGoTo;
		private System.Windows.Forms.Label lbGoTo;
	}
}