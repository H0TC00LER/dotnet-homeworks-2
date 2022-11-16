using System.Linq.Expressions;
using Hw9.ErrorMessages;
using System.Text.RegularExpressions;
using static Hw9.Parser.Checker;
using Hw9.Services.MathCalculator;

namespace Hw9.Parser
{
    public class Parser
    {
        static private string[] SplitExpression(string expression)
        {
            expression = Regex.Replace(expression, @"[^0123456789.()]", new MatchEvaluator(match => $" {match.Value} "));
            expression = Regex.Replace(expression, @"\( - [1-9]", new MatchEvaluator(match => $"( -{match.Value[4]}"));

            return expression
                .Replace("(", " ( ")
                .Replace(")", " ) ")
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);
        }

        static private readonly string[] operations = new string[] { "/", "*", "-", "+" };

        static public Queue<string> Parse(string expression)
        {
            CheckEmptyString(expression);

            expression = expression.Replace(" ", "");

            CheckStartingOperation(expression);
            CheckEndingOperation(expression);
            CheckOperationBeforeParenthesis(expression);
            CheckOperatorAfterParenthesis(expression);
            CheckTwoOperationsInRow(expression);

            var outputQueue = new Queue<string>();
            var operatorStack = new Stack<string>();

            var splittedExpression = SplitExpression(expression);

            foreach(var elem in splittedExpression)
            {
                if (Double.TryParse(elem, out var number))
                {
                    outputQueue.Enqueue(elem);
                }
                else if (operations.Contains(elem))
                {
                    if (operatorStack.Any())
                    {
                        while (operatorStack.Any() &&
                            operatorStack.Peek() != "(" &&
                            GetOperationPriority(operatorStack.Peek()) >= GetOperationPriority(elem))
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                    }
                    operatorStack.Push(elem);
                }
                else if (elem == "(")
                {
                    operatorStack.Push(elem);
                }
                else if (elem == ")")
                {
                    while (operatorStack.Any() && operatorStack.Peek() != "(")
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }

                    if (!operatorStack.Any())
                        throw new Exception(MathErrorMessager.IncorrectBracketsNumber);

                    if (operatorStack.Peek() == "(")
                    {
                        operatorStack.Pop();
                    }
                }
                else
                {
                    if (elem.Length == 1)
                        throw new Exception(MathErrorMessager.UnknownCharacterMessage(elem[0]));
                    else
                        throw new Exception(MathErrorMessager.NotNumberMessage(elem));
                }
            }

            while(operatorStack.Any())
            {
                if (operatorStack.Peek() == ")" || operatorStack.Peek() == "(")
                    throw new Exception(MathErrorMessager.IncorrectBracketsNumber);
                if (operatorStack.Peek() != "(")
                    outputQueue.Enqueue(operatorStack.Pop());
            }

            return outputQueue;
        }

        static private int GetOperationPriority(string operation)
        {
            switch (operation)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                default:
                    return -1000;
            }
        }
    }
}