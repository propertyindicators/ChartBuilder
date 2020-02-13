using System.Collections.Generic;

namespace ChartBuilder
{
	public static class Functions
	{
		public static Dictionary<string, MathFunc> Get = new Dictionary<string, MathFunc>
		{
			{"log", new MathFunc {Name="Math.Log", ParametersNumber = 2}},
			{"ln", new MathFunc {Name="Math.Log", ParametersNumber = 1}},
			{"lg", new MathFunc {Name="Math.Log10", ParametersNumber = 1}},
			{"exp", new MathFunc {Name="Math.Exp", ParametersNumber = 2}},
			{"abs", new MathFunc {Name="Math.Abs", ParametersNumber = 1}},
			{"sin", new MathFunc {Name="Math.Sin", ParametersNumber = 1}},
			{"cos", new MathFunc {Name="Math.Cos", ParametersNumber = 1}},
			{"asin", new MathFunc {Name="Math.Asin", ParametersNumber = 1}},
			{"acos", new MathFunc {Name="Math.Acos", ParametersNumber = 1}},
			{"tan", new MathFunc {Name="Math.Tan", ParametersNumber = 1}},
			{"atan", new MathFunc {Name="Math.Atan", ParametersNumber = 1}},
			{"ctg", new MathFunc {Name="1/Math.Tan", ParametersNumber = 1}},
			{"tg", new MathFunc {Name="Math.Tan", ParametersNumber = 1}},
			{"cotan", new MathFunc {Name="1/Math.Tan", ParametersNumber = 1}},
			{"min", new MathFunc {Name="Math.Min", ParametersNumber = 2}},
			{"max", new MathFunc {Name="Math.Max", ParametersNumber = 2}},
			{"trc", new MathFunc {Name="Math.Truncate", ParametersNumber = 1}},
			{"truncate", new MathFunc {Name="Math.Truncate", ParametersNumber =1}},
		};
	}

	public static class Consts
	{
		public static Dictionary<string, string> Get = new Dictionary<string, string>
		{
			{"e",  "Math.E"} ,
			{"pi", "Math.PI"},
			{"ex", "Math.E*x"},
			{"xe", "x*Math.E" }
		};
	}

}
