namespace ChartBuilder
{
	partial class MainForm
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.BuildChartButton = new System.Windows.Forms.Button();
			this.Xmin_box = new System.Windows.Forms.TextBox();
			this.X_label = new System.Windows.Forms.Label();
			this.Ymin_box = new System.Windows.Forms.TextBox();
			this.Y_label = new System.Windows.Forms.Label();
			this.Range_label = new System.Windows.Forms.Label();
			this.Ymax_box = new System.Windows.Forms.TextBox();
			this.Xmax_box = new System.Windows.Forms.TextBox();
			this.Min_label = new System.Windows.Forms.Label();
			this.Max_label = new System.Windows.Forms.Label();
			this.Func_label = new System.Windows.Forms.Label();
			this.Func_box = new System.Windows.Forms.TextBox();
			this.Func_label2 = new System.Windows.Forms.Label();
			this.Example_label = new System.Windows.Forms.Label();
			this.FuncCode_box = new System.Windows.Forms.RichTextBox();
			this.FuncCode_label = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// BuildChartButton
			// 
			this.BuildChartButton.Location = new System.Drawing.Point(54, 331);
			this.BuildChartButton.Name = "BuildChartButton";
			this.BuildChartButton.Size = new System.Drawing.Size(223, 37);
			this.BuildChartButton.TabIndex = 0;
			this.BuildChartButton.Text = "Построить график";
			this.BuildChartButton.UseVisualStyleBackColor = true;
			this.BuildChartButton.Click += new System.EventHandler(this.Build_Click);
			// 
			// Xmin_box
			// 
			this.Xmin_box.Location = new System.Drawing.Point(54, 47);
			this.Xmin_box.Name = "Xmin_box";
			this.Xmin_box.Size = new System.Drawing.Size(49, 20);
			this.Xmin_box.TabIndex = 1;
			// 
			// X_label
			// 
			this.X_label.AutoSize = true;
			this.X_label.Location = new System.Drawing.Point(28, 47);
			this.X_label.Name = "X_label";
			this.X_label.Size = new System.Drawing.Size(14, 13);
			this.X_label.TabIndex = 2;
			this.X_label.Text = "X";
			// 
			// Ymin_box
			// 
			this.Ymin_box.Location = new System.Drawing.Point(54, 79);
			this.Ymin_box.Name = "Ymin_box";
			this.Ymin_box.Size = new System.Drawing.Size(49, 20);
			this.Ymin_box.TabIndex = 3;
			// 
			// Y_label
			// 
			this.Y_label.AutoSize = true;
			this.Y_label.Location = new System.Drawing.Point(28, 82);
			this.Y_label.Name = "Y_label";
			this.Y_label.Size = new System.Drawing.Size(14, 13);
			this.Y_label.TabIndex = 4;
			this.Y_label.Text = "Y";
			// 
			// Range_label
			// 
			this.Range_label.AutoSize = true;
			this.Range_label.Location = new System.Drawing.Point(12, 9);
			this.Range_label.Name = "Range_label";
			this.Range_label.Size = new System.Drawing.Size(169, 13);
			this.Range_label.TabIndex = 5;
			this.Range_label.Text = "Диапазон построения графика:";
			// 
			// Ymax_box
			// 
			this.Ymax_box.Location = new System.Drawing.Point(124, 79);
			this.Ymax_box.Name = "Ymax_box";
			this.Ymax_box.Size = new System.Drawing.Size(49, 20);
			this.Ymax_box.TabIndex = 7;
			// 
			// Xmax_box
			// 
			this.Xmax_box.Location = new System.Drawing.Point(124, 47);
			this.Xmax_box.Name = "Xmax_box";
			this.Xmax_box.Size = new System.Drawing.Size(49, 20);
			this.Xmax_box.TabIndex = 6;
			// 
			// Min_label
			// 
			this.Min_label.AutoSize = true;
			this.Min_label.Location = new System.Drawing.Point(66, 31);
			this.Min_label.Name = "Min_label";
			this.Min_label.Size = new System.Drawing.Size(23, 13);
			this.Min_label.TabIndex = 8;
			this.Min_label.Text = "min";
			// 
			// Max_label
			// 
			this.Max_label.AutoSize = true;
			this.Max_label.Location = new System.Drawing.Point(135, 31);
			this.Max_label.Name = "Max_label";
			this.Max_label.Size = new System.Drawing.Size(26, 13);
			this.Max_label.TabIndex = 9;
			this.Max_label.Text = "max";
			// 
			// Func_label
			// 
			this.Func_label.AutoSize = true;
			this.Func_label.Location = new System.Drawing.Point(12, 133);
			this.Func_label.Name = "Func_label";
			this.Func_label.Size = new System.Drawing.Size(102, 13);
			this.Func_label.TabIndex = 10;
			this.Func_label.Text = "Функция графика:";
			// 
			// Func_box
			// 
			this.Func_box.Location = new System.Drawing.Point(31, 152);
			this.Func_box.Name = "Func_box";
			this.Func_box.Size = new System.Drawing.Size(350, 20);
			this.Func_box.TabIndex = 11;
			// 
			// Func_label2
			// 
			this.Func_label2.AutoSize = true;
			this.Func_label2.Location = new System.Drawing.Point(12, 152);
			this.Func_label2.Name = "Func_label2";
			this.Func_label2.Size = new System.Drawing.Size(18, 13);
			this.Func_label2.TabIndex = 12;
			this.Func_label2.Text = "y=";
			// 
			// Example_label
			// 
			this.Example_label.AutoSize = true;
			this.Example_label.Location = new System.Drawing.Point(12, 182);
			this.Example_label.Name = "Example_label";
			this.Example_label.Size = new System.Drawing.Size(222, 13);
			this.Example_label.TabIndex = 13;
			this.Example_label.Text = "Пример функции: x^2+sin(e[x-2])-cos(pi*x)^2";
			// 
			// FuncCode_box
			// 
			this.FuncCode_box.Location = new System.Drawing.Point(19, 230);
			this.FuncCode_box.Name = "FuncCode_box";
			this.FuncCode_box.Size = new System.Drawing.Size(377, 90);
			this.FuncCode_box.TabIndex = 14;
			this.FuncCode_box.Text = "";
			// 
			// FuncCode_label
			// 
			this.FuncCode_label.AutoSize = true;
			this.FuncCode_label.Location = new System.Drawing.Point(19, 211);
			this.FuncCode_label.Name = "FuncCode_label";
			this.FuncCode_label.Size = new System.Drawing.Size(52, 13);
			this.FuncCode_label.TabIndex = 15;
			this.FuncCode_label.Text = "Код С# :";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(417, 390);
			this.Controls.Add(this.FuncCode_label);
			this.Controls.Add(this.FuncCode_box);
			this.Controls.Add(this.Example_label);
			this.Controls.Add(this.Func_label2);
			this.Controls.Add(this.Func_box);
			this.Controls.Add(this.Func_label);
			this.Controls.Add(this.Max_label);
			this.Controls.Add(this.Min_label);
			this.Controls.Add(this.Ymax_box);
			this.Controls.Add(this.Xmax_box);
			this.Controls.Add(this.Range_label);
			this.Controls.Add(this.Y_label);
			this.Controls.Add(this.Ymin_box);
			this.Controls.Add(this.X_label);
			this.Controls.Add(this.Xmin_box);
			this.Controls.Add(this.BuildChartButton);
			this.Name = "MainForm";
			this.Text = "Построитель графиков матфункций";
			this.Load += new System.EventHandler(this.PreLoadForm);
			this.Click += new System.EventHandler(this.Build_Click);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button BuildChartButton;
		private System.Windows.Forms.TextBox Xmin_box;
		private System.Windows.Forms.Label X_label;
		private System.Windows.Forms.TextBox Ymin_box;
		private System.Windows.Forms.Label Y_label;
		private System.Windows.Forms.Label Range_label;
		private System.Windows.Forms.TextBox Ymax_box;
		private System.Windows.Forms.TextBox Xmax_box;
		private System.Windows.Forms.Label Min_label;
		private System.Windows.Forms.Label Max_label;
		private System.Windows.Forms.Label Func_label;
		private System.Windows.Forms.TextBox Func_box;
		private System.Windows.Forms.Label Func_label2;
		private System.Windows.Forms.Label Example_label;
		private System.Windows.Forms.RichTextBox FuncCode_box;
		private System.Windows.Forms.Label FuncCode_label;
	}
}

