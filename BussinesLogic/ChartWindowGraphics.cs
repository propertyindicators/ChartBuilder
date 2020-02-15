using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ChartBuilder
{
	public class ChartOptions
	{
		public int SizeX;
		public int SizeY;
		public float MinX;
		public float MaxX;
		public float MinY;
		public float MaxY;
		public List<string> Errors = new List<string>();
	}

	public struct MathFunc
	{
		public string Name;
		public int ParametersNumber;
	}

	public class ChartWindowGraphics
	{
		// Input
		private readonly ChartOptions Options;

		public ChartWindowGraphics(ChartOptions options)
		{
			Options = options;
		}

		// Output
		public Bitmap Grid;
		public Bitmap Axis;
		public Bitmap AxisAndGrid;
		public Bitmap ChartImage;

		// View adjustment
		private const int DashLengthHalf = 3;
		private const int SignOffsetAboveLeft = 20;
		private const int StepDeviderMax = 10;
		private const int AxisArrowLength = 10;
		private const int AxisArrowWidth = 5;
		private const float AccurancyCorrection = (float)0.999999;
		private static readonly Pen GridPen = new Pen(Color.Green, 1) { DashStyle = DashStyle.Dot, Alignment = PenAlignment.Center };
		private static readonly Pen AxisPen = new Pen(Color.Black, 2) { Alignment = PenAlignment.Center };
		private static readonly Font DashesSignFont = new Font("Arial", 10, FontStyle.Bold);
		private const int AproximateCharHigh = 7;
		private const int AproximateCharWidth = 5;
		private static readonly Brush DashesSignBrush = new SolidBrush(Color.Black);
		private static readonly Color ChartBackgroundColor = Color.White;

		private struct ChartLine
		{
			public PointF Start;
			public PointF End;
		}

		private struct ChartString
		{
			public PointF Location;
			public string Text;
		}

		private struct Offsets
		{
			public int DashStartX;
			public int DashEndX;
			public int SignOffsetX;
			public int DashStartY;
			public int DashEndY;
			public int SignOffsetY;

			public static Offsets Cook(ChartOptions options, float xAxisLocation, float yAxisLocation)
			{
				// X axis elements
				Offsets result = new Offsets();
				if (options.MinY >= 0)
				{
					result.DashStartX = options.SizeY - DashLengthHalf;
					result.DashEndX = options.SizeY;
					result.SignOffsetX = options.SizeY - SignOffsetAboveLeft;
				}
				else if (options.MaxY <= 0)
				{
					result.DashStartX = 1;
					result.DashEndX = DashLengthHalf + 1;
					result.SignOffsetX = DashLengthHalf + 1;
				}
				else
				{
					result.DashStartX = (int)Math.Round(xAxisLocation - DashLengthHalf);
					result.DashEndX = (int)Math.Round(xAxisLocation + DashLengthHalf);
					if (Math.Abs(options.MinY) > options.MaxY)
					{
						result.SignOffsetX = (int)Math.Round(xAxisLocation) + DashLengthHalf + 1;
					}
					else
					{
						result.SignOffsetX = (int)Math.Round(xAxisLocation) - SignOffsetAboveLeft;
					}
				}
				// Y axis elements
				if (options.MinX >= 0)
				{
					result.DashStartY = 1;
					result.DashEndY = DashLengthHalf + 1;
					result.SignOffsetY = DashLengthHalf + 1;
				}
				else if (options.MaxX <= 0) {
					result.DashStartY = options.SizeX - DashLengthHalf;
					result.DashStartY = options.SizeX;
					result.SignOffsetY = options.SizeX - SignOffsetAboveLeft; }
				else
				{
					result.DashStartY = (int)Math.Round(yAxisLocation - DashLengthHalf);
					result.DashEndY = (int)Math.Round(yAxisLocation + DashLengthHalf);
					if (Math.Abs(options.MinX) > options.MaxX)
					{
						result.SignOffsetY = (int)Math.Round(yAxisLocation) - SignOffsetAboveLeft;
					}
					else
					{
						result.SignOffsetY = (int)Math.Round(yAxisLocation) + DashLengthHalf + 1;
					}
				}
				return result;
			}
		}

		public void CookBitmaps()
		{
			// Cook axises' elements and help data
			List<ChartLine> AxisesWithDashes = CookAxisLines(Options, out float xAxisLocation, out float yAxisLocation);
			int step = DetectStep(Options); // dashes and grid lines step in pixels
			List<int> dashesX = CookDashList(Options.MinX, Options.MaxX, step);
			List<int> dashesY = CookDashList(Options.MinY, Options.MaxY, step);
			Offsets offsets = Offsets.Cook(Options, xAxisLocation, yAxisLocation);
			AxisesWithDashes.AddRange(CookDashLines(Options, offsets, xAxisLocation, yAxisLocation, dashesX, dashesY));
			List<ChartString> DashesSigns = CookDashSigns(Options, offsets, dashesX, dashesY);
			// Cook grid lines
			List<ChartLine> GridLines = CookGridLines(Options, dashesX, dashesY);
			// Draw grid
			Grid = DrawGrid(GridLines, Options);
			// Draw axis on clean bitmap
			Axis = new Bitmap(Options.SizeX + 1, Options.SizeY + 1);
			DrawAxis(Axis, AxisesWithDashes, DashesSigns);
			// Draw axis on Grid bitmap (over grid)
			AxisAndGrid = new Bitmap(Grid);
			DrawAxis(AxisAndGrid, AxisesWithDashes, DashesSigns, false);
		}

		private static Bitmap DrawGrid(List<ChartLine> gridLines, ChartOptions options)
		{
			Bitmap result = new Bitmap(options.SizeX + 1, options.SizeY + 1);
			DrawLines(result, gridLines, GridPen);
			return result;
		}

		private static void DrawAxis(Bitmap initBitmap, List<ChartLine> axisLines, List<ChartString> dashesSigns, bool cleanup = true)
		{
			DrawLines(initBitmap, axisLines, AxisPen, cleanup);
			DrawStrings(initBitmap, dashesSigns, DashesSignFont, DashesSignBrush, false);
		}

		private static void DrawLines(Bitmap initBitmap, List<ChartLine> lines, Pen p, bool cleanup = true)
		{
			using (Graphics g = Graphics.FromImage(initBitmap))
			{
				if (cleanup)
				{
					g.Clear(ChartBackgroundColor);
				}
				lines.ForEach((ChartLine l) => g.DrawLine(p, l.Start, l.End));
			}
		}

		private static void DrawStrings(Bitmap initBitmap, List<ChartString> messages, Font font, Brush brush, bool cleanup = true)
		{
			using (Graphics g = Graphics.FromImage(initBitmap))
			{
				if (cleanup)
				{
					g.Clear(ChartBackgroundColor);
				}
				messages.ForEach((ChartString m) => g.DrawString(m.Text, font, brush, m.Location));
			}
		}

		private static List<int> CookDashList(float min, float max, int step)
		{
			List<int> result = new List<int>();
			int start = (min > 0) ? ((int)(min + AccurancyCorrection) / step) * step : (int)(min / step) * step;
			int finish = (max < 0) ? ((int)(max - AccurancyCorrection) / step) * step : (int)(max / step) * step;
			for (int i = start; i <= finish; i += step)
			{
				result.Add(i);
			}
			return result;
		}

		private static int DetectStep(ChartOptions options)
		{
			int step;
			if ((options.MaxX - options.MinX) > (options.MaxY - options.MinY))
			{
				step = Convert.ToInt32(Math.Round((options.MaxX - options.MinX) / StepDeviderMax));
			}
			else
			{
				step = Convert.ToInt32(Math.Round((options.MaxY - options.MinY) / StepDeviderMax));
			}
			if (step == 0)
			{
				step = 1;
			}
			return step;
		}

		private static List<ChartLine> CookAxisLines(ChartOptions options, out float xAxisLocation, out float yAxisLocation)
		{
			List<ChartLine>  result = new List<ChartLine>();
			xAxisLocation = -1;
			yAxisLocation = -1;
			// X Axis and arrow
			if (options.MinY <= 0 && options.MaxY >= 0)
			{
				xAxisLocation = options.SizeY - options.SizeY * (-options.MinY) / (-options.MinY + options.MaxY);
				if (xAxisLocation < 0.5)
				{
					xAxisLocation = 1;
				}
				result.Add(new ChartLine { Start = new PointF(0, xAxisLocation), End = new PointF(options.SizeX, xAxisLocation) });
				if (options.MinY < 0)
				{
					result.Add(new ChartLine { Start = new PointF(options.SizeX - AxisArrowLength, xAxisLocation + AxisArrowWidth), End = new PointF(options.SizeX, xAxisLocation) });
				}
				if (options.MaxY > 0)
				{
					result.Add(new ChartLine { Start = new PointF(options.SizeX - AxisArrowLength, xAxisLocation - AxisArrowWidth), End = new PointF(options.SizeX, xAxisLocation) });
				}
			}
			// Y Axis and arrow
			if (options.MinX <= 0 && options.MaxX >= 0)
			{
				yAxisLocation = options.SizeX * (-options.MinX) / (-options.MinX + options.MaxX);
				if (yAxisLocation < 0.5)
				{
					yAxisLocation = 1;
				}
				result.Add(new ChartLine { Start = new PointF(yAxisLocation, 0), End = new PointF(yAxisLocation, options.SizeY) });
				if (options.MinX < 0)
				{
					result.Add(new ChartLine { Start = new PointF(yAxisLocation - AxisArrowWidth, AxisArrowLength + 1), End = new PointF(yAxisLocation, 1) });
				}
				if (options.MaxX > 0)
				{
					result.Add(new ChartLine { Start = new PointF(yAxisLocation + AxisArrowWidth, AxisArrowLength +1 ), End = new PointF(yAxisLocation, 1) });
				}
			}
			return result;
		}

		private static List<ChartLine> CookDashLines(ChartOptions options, Offsets offsets, float xAxisLocation, float yAxisLocation, List<int> dashesX, List<int> dashesY)
		{
			List<ChartLine> result = new List<ChartLine>();
			// X axis dashes
			foreach (int i in dashesX)
			{
				float tx = options.SizeX * (i - options.MinX) / (options.MaxX - options.MinX);
				if (xAxisLocation == -1 || tx <= options.SizeX - AxisArrowLength)
				{
					result.Add(new ChartLine { Start = new PointF(tx, offsets.DashStartX), End = new PointF(tx, offsets.DashEndX) });
				}
			}
			// Y axis dashes
			foreach (int i in dashesY)
			{
				float ty = options.SizeY - options.SizeY * (i - options.MinY) / (options.MaxY - options.MinY);
				if (yAxisLocation == -1 || ty >= AxisArrowLength)
				{
					result.Add(new ChartLine { Start = new PointF(offsets.DashStartY, ty), End = new PointF(offsets.DashEndY, ty) });
				}
			}
			return result;
		}

		private static List<ChartString> CookDashSigns(ChartOptions options, Offsets offsets, List<int> dashesX, List<int> dashesY)
		{
			List<ChartString> result = new List<ChartString>();
			// X dashes' signs
			foreach (int i in dashesX)
			{
				float tx = options.SizeX * (i - options.MinX) / (options.MaxX - options.MinX);
				if (i != 0 && tx - i.ToString().Length * AproximateCharWidth >= 0
					&& tx + i.ToString().Length * AproximateCharWidth <= options.SizeX)
				{
					result.Add(new ChartString { Location = new PointF(tx - i.ToString().Length * AproximateCharWidth, offsets.SignOffsetX), Text = i.ToString() });
				}
			}
			// Y dashes signs
			foreach (int i in dashesY)
			{
				float ty = options.SizeY - options.SizeY * (i - options.MinY) / (options.MaxY - options.MinY);
				if (i != 0 && ty - i.ToString().Length * AproximateCharHigh >= 0
					&& ty + i.ToString().Length * AproximateCharHigh <= options.SizeY)
				{
					result.Add(new ChartString { Location = new PointF(offsets.SignOffsetY, ty - AproximateCharHigh), Text = i.ToString() });
				}
			}
			return result;
		}

		private static List<ChartLine> CookGridLines(ChartOptions options, List<int> dashesX, List<int> dashesY)
		{
			List<ChartLine> result = new List<ChartLine>();
			// Y parallels
			foreach (int i in dashesX)
			{
				float tx = options.SizeX * (i - options.MinX) / (options.MaxX - options.MinX);
				result.Add(new ChartLine { Start = new PointF(tx, 0), End = new PointF(tx, options.SizeY + 1) });
			}
			// X parallels
			foreach (int i in dashesY)
			{
				float ty = options.SizeY - options.SizeY * (i - options.MinY) / (options.MaxY - options.MinY);
				result.Add(new ChartLine { Start = new PointF(0, ty), End = new PointF(options.SizeX + 1, ty) });
			}
			return result;
		}

		public void AddChartImage(Bitmap chartImage)
		{
			Debug.Assert(chartImage.Size == Grid.Size && chartImage.Size == Axis.Size && chartImage.Size == AxisAndGrid.Size,
				"Sizes of chart image and grid image(s) are not consistence");
			ChartImage = chartImage;
			UniteImages(Grid, chartImage);
			UniteImages(Axis, chartImage);
			UniteImages(AxisAndGrid, chartImage);
		}

		private static void UniteImages(Bitmap target, Bitmap source)
		{
			Color refcolor = Color.LightBlue;
			for (int x = 0; x < target.Size.Width; x++)
			{
				for (int y = 0; y < target.Size.Height; y++)
				{
					Color tc = target.GetPixel(x, y);
					Color sc = source.GetPixel(x, y);
					if (!(sc.A == ChartBackgroundColor.A && sc.R == ChartBackgroundColor.R && sc.G == ChartBackgroundColor.G && sc.B == ChartBackgroundColor.B))
					{
						if (tc.A == ChartBackgroundColor.A && tc.R == ChartBackgroundColor.R && tc.G == ChartBackgroundColor.G && tc.B == ChartBackgroundColor.B)
						{
							target.SetPixel(x, y, sc);
						}
						else
						{
							target.SetPixel(x, y, refcolor);
						}
					}
				}
			}
		}
	}
}
