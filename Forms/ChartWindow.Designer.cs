namespace ChartBuilder
{
	partial class ChartWindow
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

		private System.Windows.Forms.Label FunctionLabel;
		private System.Windows.Forms.PictureBox Chart;
		private System.Windows.Forms.CheckBox GridOption;
		private System.Windows.Forms.CheckBox AxisOption;

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.FunctionLabel = new System.Windows.Forms.Label();
			this.Chart = new System.Windows.Forms.PictureBox();
			this.GridOption = new System.Windows.Forms.CheckBox();
			this.AxisOption = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.Chart)).BeginInit();
			this.SuspendLayout();

			// FunctionLabel
			this.FunctionLabel.AutoSize = true;
			this.FunctionLabel.Font = new System.Drawing.Font(
				"Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
			this.FunctionLabel.Location = new System.Drawing.Point(7, 7);
			this.FunctionLabel.Name = "FunctionLabel";
			this.FunctionLabel.Size = new System.Drawing.Size(56, 13);
			this.FunctionLabel.TabIndex = 2;
			this.FunctionLabel.Text = "FunctionLabel";

			// Chart
			this.Chart.Location = new System.Drawing.Point(6, 33);
			this.Chart.Name = "Chart";
			this.Chart.Size = new System.Drawing.Size(614, 424);
			this.Chart.TabIndex = 3;
			this.Chart.TabStop = false;

			// GridOption
			this.GridOption.AutoSize = true;
			this.GridOption.Location = new System.Drawing.Point(504, 6);
			this.GridOption.Name = "GridOption";
			this.GridOption.Size = new System.Drawing.Size(56, 17);
			this.GridOption.TabIndex = 6;
			this.GridOption.Text = "Сетка";
			this.GridOption.UseVisualStyleBackColor = true;
			this.GridOption.CheckedChanged += new System.EventHandler(this.GridOptionChanged);

			// AxisOption
			this.AxisOption.AutoSize = true;
			this.AxisOption.Location = new System.Drawing.Point(566, 6);
			this.AxisOption.Name = "AxisOption";
			this.AxisOption.Size = new System.Drawing.Size(46, 17);
			this.AxisOption.TabIndex = 7;
			this.AxisOption.Text = "Ось";
			this.AxisOption.UseVisualStyleBackColor = true;
			this.AxisOption.CheckedChanged += new System.EventHandler(this.AxisOptionChanged);

			// ChartWindow
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 460);
			this.Controls.Add(this.AxisOption);
			this.Controls.Add(this.GridOption);
			this.Controls.Add(this.Chart);
			this.Controls.Add(this.FunctionLabel);
			this.Name = "ChartWindow";
			this.Text = "График функции";
			((System.ComponentModel.ISupportInitialize)(this.Chart)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		#endregion
	}
}