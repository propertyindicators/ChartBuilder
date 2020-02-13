using System.Collections.Generic;
using System.Drawing;

namespace ChartBuilder
{
	public static class ChartImageGraphics
	{
		public static Bitmap GetChartImage(ChartOptions chartOptions, MathFunction f)
		{
			List<List<PointF>> allFragments = CalculateChartGraph(chartOptions, f);
			return DrawImage(chartOptions, allFragments);
		}

		private static List<List<PointF>> CalculateChartGraph(ChartOptions chartOptions, MathFunction f)
		{
			List<List<PointF>> allFragments = new List<List<PointF>>();
			List<PointF> fragment = new List<PointF>();
			for (int i = 1; i <= chartOptions.SizeX; i++)
			{
				// Cook curve fragments (list of points for every fragment) 
				double x = GetRealX(chartOptions, i);
				double y = f(x);
				if (double.IsInfinity(y) || double.IsNaN(y)) // For example: when processing x asymptote or logarithm of negative value
				{
					if (fragment.Count > 0) { allFragments.Add(fragment); fragment = new List<PointF>(); }
				}
				else
				{
					float wy = GetRealY(chartOptions, (float)y);
					if ((float)y > chartOptions.MaxY || (float)y < chartOptions.MinY)
					{
						if (fragment.Count > 0)
						{
							fragment.Add(new PointF(i, wy));
							allFragments.Add(fragment);
							fragment = new List<PointF>();
						}
					}
					else
					{
						if (fragment.Count == 0)
						{
							fragment.Add(new PointF(i, wy));
						}
						else
						{
							fragment.Add(new PointF(i, wy));
						}
					}
				}
			}
			if (fragment.Count > 0)
			{
				allFragments.Add(fragment);
			}
			return allFragments;
		}

		private static Bitmap DrawImage(ChartOptions chartOptions, List<List<PointF>> allFragments)
		{
			Bitmap image = new Bitmap(chartOptions.SizeX + 1, chartOptions.SizeY + 1);
			Graphics g = Graphics.FromImage(image);
			using (g)
			{
				g.Clear(Color.White);
				Pen p = new Pen(Color.Blue, 2);
				p.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
				foreach (List<PointF> onef in allFragments)
				{
					if (onef.Count == 1) { onef.Add(onef[0]); } // If fragment contains only 1 point - add the same second point in order to draw line
					g.DrawCurve(p, onef.ToArray());
				}
			}
			return image;
		}

		private static float GetRealX(ChartOptions me, int x)
		{
			return me.MinX + (me.MaxX - me.MinX) * x / me.SizeX;
		}

		private static float GetRealY(ChartOptions me, float y)
		{
			return (me.MaxY - y) * me.SizeY / (me.MaxY - me.MinY);
		}

	}
}
