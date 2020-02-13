using ChartBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestClass]
	public class MainFormTest
	{
		[TestMethod]
		public void MainFormAnalyzeChartInputParametersAndCookChartOptionsTest()
		{
			ChartOptions chartOptions =
				MainForm.AnalyzeChartInputParametersAndCookChartOptions("-10.0", "10.0", "-10.0", "10.0");
			Assert.IsTrue(chartOptions.Errors.Count == 0);
			Assert.AreEqual(800, chartOptions.SizeX);
			Assert.AreEqual(800, chartOptions.SizeY);

			chartOptions =
				MainForm.AnalyzeChartInputParametersAndCookChartOptions("-10", "10", "-5", "5");
			Assert.IsTrue(chartOptions.Errors.Count == 0);
			Assert.AreEqual(800, chartOptions.SizeX);
			Assert.AreEqual(400, chartOptions.SizeY);

			chartOptions =
				MainForm.AnalyzeChartInputParametersAndCookChartOptions("5", "-5", "10", "-10");
			Assert.IsTrue(chartOptions.Errors.Count == 0);
			Assert.AreEqual(400, chartOptions.SizeX);
			Assert.AreEqual(800, chartOptions.SizeY);

			chartOptions =
				MainForm.AnalyzeChartInputParametersAndCookChartOptions("5", "5", "10", "10");
			Assert.AreEqual(2, chartOptions.Errors.Count);

			chartOptions =
				MainForm.AnalyzeChartInputParametersAndCookChartOptions("-2000", "2000", "-2000", "2000");
			Assert.AreEqual(4, chartOptions.Errors.Count);

			chartOptions =
				MainForm.AnalyzeChartInputParametersAndCookChartOptions("-10", "10", "-1000", "1000");
			Assert.AreEqual(1, chartOptions.Errors.Count);

			chartOptions =
				MainForm.AnalyzeChartInputParametersAndCookChartOptions("1000", "-1000", "10", "-10");
			Assert.AreEqual(1, chartOptions.Errors.Count);
		}
	}
}
