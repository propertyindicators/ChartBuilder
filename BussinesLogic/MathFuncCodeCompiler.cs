using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace ChartBuilder
{
	/// <summary>
	/// Interface of chart compiled math function
	/// </summary>
	/// <param name="x">"x" input</param>
	/// <returns>"y" output</returns>
	public delegate double MathFunction(double x);

	/// <summary>
	/// Result of compilation predefined math function
	/// </summary>
	public class CompiledMathFunction
	{
		public string Error;
		public MathFunction Func;
	}

	public static class MathFuncCodeCompiler
	{
		private const string MathFuctionMethodPattern =
@"using System;
namespace ChartBuilder
{
	public static class CompileMathFunction
	{
		public static MathFunction Get() 
		{ 
			return (MathFunction)GetValue; 
		}

		public static double GetValue(double x)
		{
		   return [dyncode];
		}
	}
}";
		// Assembly needed references
		private static readonly string[] RefsForMathFuctionMethod =
			new string[] {"System.dll", typeof(MathFunction).Assembly.Location};

		/// <summary>
		/// Compiles (using reflection) static method that implement input C# coded math function
		/// </summary>
		/// <param name="mathFuncCode">Math function to compile (C# encoded string)</param>
		/// <returns>CompiledMathFunction -- contains ready to invoke math method reference</returns>
		public static CompiledMathFunction CompileMathFuction(string mathFuncCode)
		{
			string t = MathFuctionMethodPattern.Replace("[dyncode]", mathFuncCode);
			//Console.WriteLine(t);
			var tprov = CodeDomProvider.CreateProvider("CSharp");
			CompilerParameters pars = new CompilerParameters()
			{
				GenerateExecutable = false,
				GenerateInMemory = true,
			};
			pars.ReferencedAssemblies.AddRange(RefsForMathFuctionMethod);
			var resultCompile = tprov.CompileAssemblyFromSource(pars, t);
			if (resultCompile.Errors.Count == 0)
				return new CompiledMathFunction()
				{
					Func = (MathFunction)resultCompile
						.CompiledAssembly
						.GetType("ChartBuilder.CompileMathFunction")
						.GetMethod("Get")
						.Invoke(null, null)
				};
			else
			{
				List<string> errors = new List<string>();
				foreach (CompilerError er in resultCompile.Errors)
				{
					errors.Add(er.ErrorText);
				}
				string ts = string.Join(",", errors);
				return new CompiledMathFunction()
				{
					Error = $"Compilation error: {string.Join(", ", errors)}"
				};
			}
		}
	}
}
