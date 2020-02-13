using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ChartBuilder
{
	public static class MathExpressionDetectors
	{
		public static MathExpression X(string mathFunc, int start)
		{
			if (mathFunc[start] == 'x' || mathFunc[start] == 'X')
			{
				if (start == mathFunc.Length - 1 || !char.IsLetter(mathFunc[start + 1]))
					return new MathExpression()
					{
						Type = ExpressionType.X,
						Length = 1,
						Position = start,
						Original = mathFunc[start].ToString(),
						Replaced = "x",
						Error = ""
					};
			}
			return null;
		}

		public static MathExpression Operator(string mathFunc, int start)
		{
			if (mathFunc[start] == '+' || mathFunc[start] == '-' || mathFunc[start] == '*' || mathFunc[start] == '/' || mathFunc[start] == '^')
				return new MathExpression()
				{
					Type = ExpressionType.Operator,
					Length = 1,
					Position = start,
					Original = mathFunc[start].ToString(),
					Replaced = new string(new char[] { mathFunc[start] }),
					Error = ""
				};
			return null;
		}


		private static Regex FindDigitValueRegex = new Regex("^[0-9]+\\.[0-9]+|^[0-9]+", RegexOptions.Compiled);

		public static MathExpression DigitValue(string mathFunc, int start)
		{
			if (char.IsDigit(mathFunc[start]))
			{
				var m = FindDigitValueRegex.Match(mathFunc.Substring(start, mathFunc.Length - start));
				if (m.Success)
				{
					if (!double.TryParse(m.Value, out double parsedValue))
					{
						return new MathExpression()
						{
							Error = $"Неправильно форматированое число (возможно - слишком большое или слишком малое) в выражении \"{mathFunc}\" на позиции {start}"
						};
					}
					return new MathExpression()
					{
						Type = ExpressionType.DigitValue,
						Length = m.Value.Length,
						Position = start,
						Original = m.Value,
						Replaced = m.Value
					};
				}
			}
			return null;
		}

		public static MathExpression Parentheses(string mathFunc, int start)
		{
			return NestedExpression(mathFunc, start, '(', ')', "круглые", "({0})");
		}

		public static MathExpression Brackets(string mathFunc, int start)
		{
			return NestedExpression(mathFunc, start, '[', ']', "квадратные", "Math.Abs({0})");
		}

		// "wrapper" parameter should contains "{0}" that will be replaced with generated expression code 
		private static MathExpression NestedExpression(string mathFunc, int start, char openChar, char closeChar, string errSnippet, string wrapper)
		{
			if (mathFunc[start] == openChar)
			{
				int pos = 1;
				int openedCount = 1;
				while (openedCount > 0 && start + pos < mathFunc.Length)
				{
					if (mathFunc[start + pos] == openChar) openedCount++;
					else if (mathFunc[start + pos] == closeChar) openedCount--;
					pos++;
				}
				if (openedCount > 0)
					return new MathExpression()
					{
						Error = $"Незакрытые {errSnippet} скобки в выражении \"{mathFunc}\" на позиции {start}"
					};
				else
				{
					if (pos == 2)
						return new MathExpression()
						{
							Error = $"Пустые {errSnippet} скобки в выражении \"{mathFunc}\" на позиции {start}"
						};
					// When we found some expression in consistency (open and close) parentheses/brackets - create recursively and try to parse math expression in parentheses/brackets
					MathFuncCodeGenerator nestedExpression = MathFuncCodeGenerator.Generate(mathFunc.Substring(start + 1, pos - 2));
					// If parsing of expression in parentheses/brackets is failed - return error feedback
					if (!string.IsNullOrEmpty(nestedExpression.Error)) return new MathExpression() { Error = nestedExpression.Error };
					// If expression in parentheses/brackets is parsed with success - return generated code recursively (this allows parsing of any nesting levels)
					return new MathExpression()
					{
						Type = ExpressionType.Brackets,
						Position = start,
						Length = pos,
						Original = mathFunc.Substring(start, pos),
						Replaced = string.Format(wrapper, nestedExpression.OutputText)
					};
				}
			}
			return null;
		}

		public static Regex FunctionRegex = new Regex("^([a-z]+)[(]", RegexOptions.Compiled);
		public static Regex ConstRegex = new Regex("^([a-z]+)", RegexOptions.Compiled);

		public static MathExpression Function(string mathFunc, int start)
		{
			string t = mathFunc.Substring(start, mathFunc.Length - start);
			Match m = FunctionRegex.Match(t);
			if (m.Success && Functions.Get.ContainsKey(m.Groups[1].Value))
			{
				int cpos = start + m.Value.Length;
				int countParentheses = 1;
				int countBrackets = 0;
				StringBuilder temppar = new StringBuilder();
				List<string> parameters = new List<string>();
				bool isFuncParenthesesClosed = true;
				while (isFuncParenthesesClosed & cpos < mathFunc.Length)
				{
					if (mathFunc[cpos] == '(') { countParentheses++; }
					if (mathFunc[cpos] == ')') { countParentheses--; }
					if (mathFunc[cpos] == '[') { countBrackets++; }
					if (mathFunc[cpos] == ']') { countBrackets--; }
					if (countBrackets < 0)
						return new MathExpression()
						{
							Error = $"Обнаружена лишняя закрывающая квадратная скобка в параметрах функции {m.Groups[1].Value} в выражении: {mathFunc} на позиции {start}"
						};
					if (countBrackets == 0
						&& ((mathFunc[cpos] == ',' && countParentheses == 1) || (mathFunc[cpos] == ')'
						&& countParentheses == 0))) // Condition is true when parameter' separator (coma out of parentheses/brackets) is detected
					{
						parameters.Add(temppar.ToString());
						temppar = new StringBuilder();
					}
					else
					{
						temppar.Append(mathFunc[cpos]);
					}
					if (countParentheses == 0 && countBrackets == 0)
					{
						isFuncParenthesesClosed = false;
					}
					cpos++;
				}
				if (isFuncParenthesesClosed)
				{
					if (countBrackets == 0)
						return new MathExpression()
						{
							Error = $"Обнаружена незарытая круглая скобка в параметрах функции {m.Groups[1].Value} в выражении: {mathFunc} на позиции {start}"
						};
					else
						return new MathExpression()
						{
							Error = $@"Обнаружена незарытая круглая скобка в параметрах функции {m.Groups[1].Value} в выражении: {mathFunc} на позиции {start}!
Возможно некорректное использование квадратных скобок [] (countBrackets={countBrackets})"
						};
				}

				if (parameters.Count == 1 && parameters[0].Length == 0)
					return new MathExpression()
					{
						Error = $"Непонятный вызов ошибки пустой функции (константы) {m.Groups[1].Value}() в выражении: {mathFunc} на позиции {start}"
					};
				if (parameters.Count != Functions.Get[m.Groups[1].Value].ParametersNumber)
				{
					List<string> tp = new List<string>();
					for (int i = 0; i < parameters.Count; i++)
					{
						tp.Add($"p{i + 1}=\"{parameters[i]}\"");
					}
					return new MathExpression()
					{
						Error = $@"Обнаружено неверное количество указаных параметров в функции {m.Groups[1].Value} в выражении {mathFunc} 
на позиции{start}. Функция предполагает параметров - {Functions.Get[m.Groups[1].Value].ParametersNumber}, обнаружено {parameters.Count}. 
Обнаружены следующие параметры: {string.Join(",", tp)}"
					};
				}
				// Parse recursively every found parameter
				List<string> parsedParameters = new List<string>();
				foreach (string p in parameters)
				{
					MathFuncCodeGenerator sp = MathFuncCodeGenerator.Generate(p);
					if (!string.IsNullOrEmpty(sp.Error)) return new MathExpression() { Error = sp.Error }; // Error feedback
					else { parsedParameters.Add(sp.OutputText); }
				}
				return new MathExpression()
				{
					Type = ExpressionType.Function,
					Position = start,
					Length = cpos - start,
					Original = mathFunc.Substring(start, cpos - start),
					Replaced = $"{Functions.Get[m.Groups[1].Value].Name}({string.Join(",", parsedParameters)})"
				};
			}
			return null;
		}

		// Run this detector after Function() detector in order to have consistency in errors' processioning
		public static MathExpression Const(string mathFunc, int start)
		{
			string t = mathFunc.Substring(start, mathFunc.Length - start);
			Match mc = ConstRegex.Match(t);
			if (mc.Success)
			{
				if (Consts.Get.ContainsKey(mc.Value))
					return new MathExpression()
					{
						Type = ExpressionType.Const,
						Position = start,
						Length = mc.Value.Length,
						Original = mc.Value,
						Replaced = Consts.Get[mc.Value]
					};
				else
					return new MathExpression()
					{
						Error = $"Неизвестная функция или константа \"{mc.Value}\" в выражении \"{mathFunc}\" на позиции {start}"
					};
			}
			return null;
		}
	}
}
