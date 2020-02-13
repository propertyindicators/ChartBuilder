using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ChartBuilder
{
	public enum ExpressionType
	{
		X,
		Operator,
		DigitValue,
		Parentheses,
		Brackets, // implements math.abs()
		Function,
		Const
	}

	public class MathExpression
	{
		internal ExpressionType Type;
		internal int Length;
		internal int Position; // Char index in string
		internal string Original;
		internal string Replaced;
		internal string Error;
	}

	public class MathFuncCodeGenerator
	{
		// Input
		private readonly string InputText;

		// Output
		public string OutputText;
		public string Error;

		private List<MathExpression> ExpressionList = new List<MathExpression>();

		/// <summary>
		/// Static constructor that creates MathFuncCodeGenerator object and generate C# code of input math expression.
		/// </summary>
		/// <param name="functionText">Text of math expression</param>
		public static MathFuncCodeGenerator Generate(string functionText)
		{
			MathFuncCodeGenerator result = new MathFuncCodeGenerator(functionText);
			result.Parse();
			return result;
		}

		private MathFuncCodeGenerator(string functionText)
		{
			InputText = functionText;
		}

		/// <summary>
		/// Implements functionality for parsing math function string using recursive approach.
		/// </summary>
		private void Parse()
		{
			int startCharIndex = 0;
			// Extract potential math expressions
			while (startCharIndex < InputText.Length)
			{
				MathExpression extractedExpression = GetExpression(InputText, startCharIndex);
				if (!string.IsNullOrEmpty(extractedExpression.Error))
				{
					Error = extractedExpression.Error;
					return;
				}
				ExpressionList.Add(extractedExpression);
				startCharIndex += extractedExpression.Length;
			}

			// Validate consistency of expression queue (operators do not follow in a row, there are not missed values near operators) 
			for (int i = 0; i < ExpressionList.Count; i++)
			{
				string t = CheckOperatorConsistency(i);
				if (!string.IsNullOrEmpty(t))
				{
					Error = t;
					return;
				}
			}

			// Find pow operator "^" and replace with Math.Pow()
			IterateExpressionListWithAction(1, 2, DetectPowOperatorAndCookReplacement, (te, i) =>
				{
					ExpressionList.RemoveRange(i - 1, 3);
					ExpressionList.Insert(i - 1, te);
					return 0;
				});

			// Find missed multiply operators "*" and insert "*" expressions
			IterateExpressionListWithAction(0, 2, DetectMissedMultiplyOperatorAndCookInsertment, (te, i) =>
				{
					ExpressionList.Insert(i + 1, te);
					return 2;
				});

			//Join parsed and processed expressions replaced with C# code
			List<string> outslist = new List<string>();
			foreach (MathExpression me in ExpressionList)
			{
				outslist.Add(me.Replaced);
			}
			OutputText = string.Join("", outslist);
		}

		private void IterateExpressionListWithAction(
			int startShift,
			int endSfift,
			Func<int, MathExpression> operatorCheckFunc,
			Func<MathExpression, int, int> exprListUpdatincAction)
		{
			Debug.Assert(startShift < ExpressionList.Count || startShift >= 0,
				$"Unexpected \"startShift\" = {startShift} input in IterateExpressionListWithAction()");
			Debug.Assert(endSfift < ExpressionList.Count || startShift >= 0,
				$"Unexpected \"endSfift\" = {endSfift} input in IterateExpressionListWithAction()");
			int i = startShift;
			while (i <= ExpressionList.Count - endSfift)
			{
				MathExpression te = operatorCheckFunc(i);
				if (te == null) i++;
				else
				{
					i += exprListUpdatincAction(te, i);
				}
			}
		}

		private MathExpression DetectPowOperatorAndCookReplacement(int n)
		{
			if (ExpressionList[n].Type != ExpressionType.Operator) return null;
			else if (ExpressionList[n].Original != "^") return null;
			string arg1 = ExpressionList[n - 1].Type == ExpressionType.Parentheses ?
				ExpressionList[n - 1].Replaced.Substring(1, ExpressionList[n - 1].Replaced.Length - 2)
				: ExpressionList[n - 1].Replaced; // remove unneeded parentheses in arguments if exist
			string arg2 = ExpressionList[n + 1].Type == ExpressionType.Parentheses ?
				ExpressionList[n + 1].Replaced.Substring(1, ExpressionList[n + 1].Replaced.Length - 2)
				: ExpressionList[n + 1].Replaced;
			return new MathExpression()
			{
				Type = ExpressionType.Function,
				Position = ExpressionList[n - 1].Position,
				Length = ExpressionList[n - 1].Length + ExpressionList[n + 1].Length + 1,
				Original = $"{ExpressionList[n - 1].Original}^{ExpressionList[n + 1].Original}",
				Replaced = $"Math.Pow({arg1},{arg2})"
			};
		}

		private static readonly Dictionary<ExpressionType, Func<string, int, MathExpression>> DetectorsMap =
			new Dictionary<ExpressionType, Func<string, int, MathExpression>>()
		{
				{ExpressionType.X,  MathExpressionDetectors.X},
				{ExpressionType.Operator,  MathExpressionDetectors.Operator},
				{ExpressionType.DigitValue,  MathExpressionDetectors.DigitValue},
				{ExpressionType.Parentheses,  MathExpressionDetectors.Parentheses},
				{ExpressionType.Brackets,  MathExpressionDetectors.Brackets},
				{ExpressionType.Function,  MathExpressionDetectors.Function},
				{ExpressionType.Const,  MathExpressionDetectors.Const},
		};

		private MathExpression GetExpression(string mathFunc, int start)
		{
			Debug.Assert(!string.IsNullOrEmpty(mathFunc),
				"\"mathExpression\" input parameter could not be null or empty");
			Debug.Assert(start >= 0 && start < mathFunc.Length,
				"\"start\" input parameter should be >= 0 and < mathExpression.Length");
			foreach (Func<string, int, MathExpression> f in DetectorsMap.Values)
			{
				MathExpression te = f(mathFunc, start);
				if (te != null) return te;
			}
			// If all detectors did not detect anything - return error about unknown construction
			return new MathExpression()
			{
				Error = $"Обнаружен неизвестный/непонятный символ \"{mathFunc[start]}\" в выражении \"{mathFunc}\" на позиции {start}"
			};
		}

		private string CheckOperatorConsistency(int n)
		{
			Debug.Assert(n < ExpressionList.Count || n >= 0,
				$"Unexpected input index \"n\" in CheckOperatorConsistency(): n={n}, ExpressionList.Count = {ExpressionList.Count}");
			if (ExpressionList[n].Type != ExpressionType.Operator) return ""; // Process operators only
			if (n == ExpressionList.Count - 1)
				return $"Обнаружен недопустимый оператор вычисления {ExpressionList[n].Original} в конце выражения: {InputText}";
			else
			{
				if (n == 0 & (ExpressionList[0].Original == "*" || ExpressionList[0].Original == "/" || ExpressionList[0].Original == "^"))
					return $"Обнаружено недопустимое использование опертора вычисления {ExpressionList[0].Original} в начале выражения: {InputText}";
				if (ExpressionList[n + 1].Type == ExpressionType.Operator)
					return $@"Обнаружена недопустимая последовательность двух операторов  ""{ExpressionList[n].Original}{ExpressionList[n + 1].Original}"" 
в позиции {ExpressionList[n].Position + 1} в мат.выражении: {InputText}";
			}
			return "";
		}

		private MathExpression DetectMissedMultiplyOperatorAndCookInsertment(int n)
		{
			if (n>=ExpressionList.Count - 1) return null;
			if (ExpressionList[n].Type == ExpressionType.Operator) return null; // Process operands only
			if (ExpressionList[n + 1].Type == ExpressionType.Operator) return null;
			return new MathExpression()
			{
				Type = ExpressionType.Operator,
				Position = ExpressionList[n + 1].Position,
				Length = 1,
				Original = "",
				Replaced = "*"
			};
		}
	}
}
