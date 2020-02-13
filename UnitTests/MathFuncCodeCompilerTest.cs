using ChartBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestClass]
	public class MathFuncCodeCompilerTest
	{
		[TestMethod]
		public void MathFuncCodeCompilerCompileMathFuctionTest()
		{
			Assert.AreEqual(1, MathFuncCodeCompiler.CompileMathFuction("x").Func(1));
			Assert.AreEqual(0, MathFuncCodeCompiler.CompileMathFuction("x*x/2-2").Func(2));
			Assert.AreEqual(1, MathFuncCodeCompiler.CompileMathFuction("Math.Pow(Math.E,x)").Func(0));
		}
	}
}
