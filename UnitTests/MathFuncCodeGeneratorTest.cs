using System;
using ChartBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestClass]
	public class MathFuncCodeGeneratorTest
	{
		[TestMethod]
		public void MathFuncCodeGeneratorGenerateTest()
		{
			Assert.AreEqual("x", MathFuncCodeGenerator.Generate("x").OutputText);
			Assert.AreEqual("Math.E*x", MathFuncCodeGenerator.Generate("ex").OutputText);
			Assert.AreEqual("Math.E*(x-2)", MathFuncCodeGenerator.Generate("e(x-2)").OutputText);
			Assert.AreEqual("Math.E*(Math.Sin(x)-2)", MathFuncCodeGenerator.Generate("e(sin(x)-2)").OutputText);
			Assert.AreEqual("Math.E*Math.Pow(Math.Abs((x-2)*2-2),2)", MathFuncCodeGenerator.Generate("e[(x-2)2-2]^2").OutputText);
			Assert.AreEqual("Math.E*Math.Pow(Math.Abs((x-2)*2-2),(1/2))", MathFuncCodeGenerator.Generate("e[(x-2)2-2]^(1/2)").OutputText);
			Assert.AreEqual("Math.E*Math.Pow(Math.Abs((x-2)*2-2),0.33333)", MathFuncCodeGenerator.Generate("e[(x-2)2-2]^0.33333").OutputText);
			Assert.AreEqual("0.5*(Math.Pow(x,2)+Math.Sin(Math.E*Math.Abs(x-2))-Math.Pow(Math.Cos(Math.PI*x),2))",
				MathFuncCodeGenerator.Generate("0.5(x^2+sin(e[x-2])-cos(pi*x)^2)").OutputText);
			Assert.IsNull(MathFuncCodeGenerator.Generate("e[(x-2)2-2]^0.33333*Nan").OutputText);
		}
	}
}
