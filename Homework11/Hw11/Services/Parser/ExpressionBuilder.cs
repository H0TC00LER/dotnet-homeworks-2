using Hw11.ErrorMessages;
using System.Linq.Expressions;

namespace Hw11.Services.Parser
{
    public class ExpressionBuilder
    {
        static public Expression BuildExpressionTree(Queue<string> queue)
        {
            var treeStack = new Stack<Expression>();
            while (queue.Any())
            {
                var currentNode = queue.Dequeue();
                if (Double.TryParse(currentNode, out double number))
                {
                    treeStack.Push(Expression.Constant(number));
                }
                else
                {
                    var arg2 = treeStack.Pop();
                    var arg1 = treeStack.Pop();
                    treeStack.Push(BuildMathExpression(arg1, arg2, currentNode));
                }
            }
            return treeStack.Pop();
        }

        static public Expression BuildMathExpression(Expression arg1, Expression arg2, string operation)
        {
            switch (operation)
            {
                case "+":
                    return Expression.Add(arg1, arg2);
                case "-":
                    return Expression.Subtract(arg1, arg2);
                case "*":
                    return Expression.Multiply(arg1, arg2);
                case "/":
                    if (arg2.Type == typeof(ConstantExpression) && Convert.ToDouble((arg2 as ConstantExpression).Value) == 0)
                        throw new Exception(MathErrorMessager.DivisionByZero);
                    return Expression.Divide(arg1, arg2);
                default:
                    throw new Exception();
            }
        }
    }
}
