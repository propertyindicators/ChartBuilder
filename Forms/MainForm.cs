using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ChartBuilder
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void PreLoadForm(object sender, EventArgs e)
		{
			Xmin_box.Text = "-10";
			Xmax_box.Text = "10";
			Ymin_box.Text = "-10";
			Ymax_box.Text = "10";
			Func_box.Text = "0.5(x^2+sin(e[x-2])-cos(pi*x)^2)";
		}

		private void Build_Click(object sender, EventArgs e)
		{
			if (Func_box.Text != null)
			{
				Func_box.Text = Func_box.Text.Replace(" ", "");
			}
			if (string.IsNullOrEmpty(Func_box.Text))
			{
				MessageBox.Show("Не задана функция графика"); return;
			}

			// Code generation of math function
			MathFuncCodeGenerator generatedFuncCode = MathFuncCodeGenerator.Generate(Func_box.Text);
			if (string.IsNullOrEmpty(generatedFuncCode.Error)) {
				FuncCode_box.Text="Код функции : "+ generatedFuncCode.OutputText;
			}
			else
			{
				MessageBox.Show("Ошибка : " + generatedFuncCode.Error); return;
			}

			// Compile (with reflection) method with generated math function
			CompiledMathFunction compiledMathFunction = MathFuncCodeCompiler.CompileMathFuction(generatedFuncCode.OutputText);
			if (!string.IsNullOrWhiteSpace(compiledMathFunction.Error))
			{
				FuncCode_box.Text = $"{FuncCode_box.Text}\nВозникли ошибки компиляции: {compiledMathFunction.Error}";
				return;
			}
			ChartOptions chartOptions = 
				AnalyzeChartInputParametersAndCookChartOptions(Xmin_box.Text, Xmax_box.Text, Ymin_box.Text, Ymax_box.Text);
			if (chartOptions.Errors.Count>0)
			{
				MessageBox.Show("Обнаружны ошибки задания диапазона: " + string.Join("! ", chartOptions.Errors)); return;
			}
			// Create ChartWindow form
			ChartWindow f = new ChartWindow();
			f.InitializeChart(Func_box.Text, chartOptions, compiledMathFunction.Func);
			f.Show();
		}

		public static ChartOptions AnalyzeChartInputParametersAndCookChartOptions(
			string minX, string maxX, string minY, string maxY)
		{
			ChartOptions chartOptions = new ChartOptions();
			chartOptions.Errors = new List<string>();
			if (!float.TryParse(minX, out chartOptions.MinX))
			{
				chartOptions.Errors.Add("Нечисловое значение в поле дипазона \"min X\"");
			}
			if (!float.TryParse(maxX, out chartOptions.MaxX))
			{
				chartOptions.Errors.Add("Нечисловое значение в поле дипазона \"max X\"");
			}
			if (!float.TryParse(minY, out chartOptions.MinY))
			{
				chartOptions.Errors.Add("Нечисловое значение в поле дипазона \"min Y\"");
			}
			if (!float.TryParse(maxY, out chartOptions.MaxY))
			{
				chartOptions.Errors.Add("Нечисловое значение в поле дипазона \"max Y\"");
			}
			if (chartOptions.Errors.Count != 0) return chartOptions;
			if (chartOptions.MinX > 1000 || chartOptions.MinX < -1000)
			{
				chartOptions.Errors.Add("Значение в поле \"min X\" находится за пределами допустимого диапазона [-1000..1000]");
			}
			if (chartOptions.MaxX > 1000 || chartOptions.MaxX < -1000)
			{
				chartOptions.Errors.Add("Значение в поле \"max X\" находится за пределами допустимого диапазона [-1000..1000]");
			}
			if (chartOptions.MinY > 1000 || chartOptions.MinY < -1000)
			{
				chartOptions.Errors.Add("Значение в поле \"min Y\" находится за пределами допустимого диапазона [-1000..1000]");
			}
			if (chartOptions.MaxY > 1000 || chartOptions.MaxY < -1000)
			{
				chartOptions.Errors.Add("Значение в поле \"max Y\" находится за пределами допустимого диапазона [-1000..1000]");
			}
			if (chartOptions.MinX == chartOptions.MaxX)
			{
				chartOptions.Errors.Add("\"min X\" и \"max X\" не могут быть равными");
			}
			if (chartOptions.MinY == chartOptions.MaxY)
			{
				chartOptions.Errors.Add("\"min Y\" и \"max Y\" не могут быть равными");
			}
			if (chartOptions.Errors.Count != 0) return chartOptions;
			if (chartOptions.MinX > chartOptions.MaxX)
			{
				SwapFloats(ref chartOptions.MinX, ref chartOptions.MaxX);
			}
			if (chartOptions.MinY > chartOptions.MaxY)
			{
				SwapFloats(ref chartOptions.MinY, ref chartOptions.MaxY);
			}
			double lx = chartOptions.MaxX - chartOptions.MinX;
			double ly = chartOptions.MaxY - chartOptions.MinY;
			double dxy = lx / ly;
			if (dxy < 50.0 / 800 || dxy > 800 / 50.0)
			{
				chartOptions.Errors.Add("Соотношение диапазонов вывода не может быть больше 16");
				return chartOptions;
			}
			if (lx > ly)
			{
				chartOptions.SizeX = 800; chartOptions.SizeY = (int)Math.Round(800 * ly / lx);
			}
			else
			{
				chartOptions.SizeY = 800; chartOptions.SizeX = (int)Math.Round(800 * lx / ly);
			}
			return chartOptions;
		}

		private static void SwapFloats(ref float f1, ref float f2)
		{
			float t = f1;
			f1 = f2;
			f2 = t;
		}
	}
}
