using Hw11.ErrorMessages;
using Hw11.Exceptions;
using System.Text.RegularExpressions;

namespace Hw11.Services.Parser
{
    public class Checker
    {
        static private readonly string[] operations = new string[] { "/", "*", "-", "+" };

        public static void CheckEmptyString(string expression)
        {
            if (expression == null || expression.Length == 0)
                throw new Exception(MathErrorMessager.EmptyString);
        }

        public static void CheckStartingOperation(string expression)
        {
            if (operations.Contains(expression[0].ToString()))
                throw new InvalidSyntaxException(MathErrorMessager.StartingWithOperation);
        }

        public static void CheckEndingOperation(string expression)
        {
            if (operations.Contains(expression[expression.Length - 1].ToString()))
                throw new InvalidSyntaxException(MathErrorMessager.EndingWithOperation);
        }

        public static void CheckOperationBeforeParenthesis(string expression)
        {
            var regex = new Regex(@"[-+*/]\)");
            var match = regex.Match(expression);
            if (match.Success)
                throw new InvalidSyntaxException(MathErrorMessager.OperationBeforeParenthesisMessage(match.Value[0].ToString()));
        }

        public static void CheckOperatorAfterParenthesis(string expression)
        {
            var regex = new Regex(@"\([+*/]");
            var match = regex.Match(expression);
            if (match.Success)
                throw new InvalidSyntaxException(MathErrorMessager.InvalidOperatorAfterParenthesisMessage(match.Value[1].ToString()));
        }

        public static void CheckTwoOperationsInRow(string expression)
        {
            var regex = new Regex(@"[-+*/][-+*/]");
            var match = regex.Match(expression);
            if (match.Success)
                throw new InvalidSyntaxException
                    (MathErrorMessager.TwoOperationInRowMessage(match.Value[0].ToString(), match.Value[1].ToString()));
        }
    }
}
