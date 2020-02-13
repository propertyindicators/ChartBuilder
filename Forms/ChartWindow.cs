using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChartBuilder
{
	public partial class ChartWindow : Form
	{
		public ChartWindowGraphics Images;

		public ChartWindow()
		{
			InitializeComponent();
		}

		public void InitializeChart(string mathFuncText, ChartOptions chartParametrs, MathFunction mathFunc)
		{
			FunctionLabel.Text = "y=" + mathFuncText;
			// Adjusting of window and chart area
			Size = new Size(chartParametrs.SizeX + 23, chartParametrs.SizeY + 66);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Chart.Size = new Size(chartParametrs.SizeX + 1, chartParametrs.SizeY + 1);
			Chart.Location = new Point(3, 23);
			Chart.Visible = true;
			// Initialization and drawing of chart images
			Images = new ChartWindowGraphics(chartParametrs);
			Images.CookBitmaps();
			Images.AddChartImage(ChartImageGraphics.GetChartImage(chartParametrs, mathFunc));
			// Initialization and adjusting of axis/grid options' checks
			GridOption.Location = new Point(chartParametrs.SizeX - 100, 3);
			AxisOption.Location = new Point(chartParametrs.SizeX - 40, 3);
			AxisOption.Checked = true;
			GridOption.Checked = false;
		}

		private void AxisOptionChanged(object sender, EventArgs e)
		{
			ChangeChartBackground();
		}

		private void GridOptionChanged(object sender, EventArgs e)
		{
			ChangeChartBackground();
		}

		/// <summary>
		/// Outputs to the main PictureBox the variant of chart image dependently of options user selected
		/// </summary>
		private void ChangeChartBackground()
		{
			if (GridOption.Checked && AxisOption.Checked) Chart.Image = Images.AxisAndGrid;
			else if (GridOption.Checked ) Chart.Image = Images.Grid;
			else if (AxisOption.Checked) Chart.Image = Images.Axis;
			else Chart.Image = Images.ChartImage;
		}
	}
}
