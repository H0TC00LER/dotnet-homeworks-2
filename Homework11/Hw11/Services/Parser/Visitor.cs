using Hw11.ErrorMessages;
using System.Linq.Expressions;

namespace Hw11.Services.Parser
{
    public class Visitor
    {
        public async Task<double> Visit(ConstantExpression node) => await Task.Run(() => Convert.ToDouble(node.Value));

        public async Task<double> Visit(BinaryExpression node)
        {
            await Task.Delay(1000);

            dynamic leftNode = node.Left;
            dynamic rightNode = node.Right;

            var tasks = await Task.WhenAll(new Task<double>[] { Visit(leftNode), this.Visit(rightNode) });
            var leftNodeResult = tasks[0];
            var rightNodeResult = tasks[1];

            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    return leftNodeResult + rightNodeResult;
                case ExpressionType.Subtract:
                    return leftNodeResult - rightNodeResult;
                case ExpressionType.Multiply:
                    return leftNodeResult * rightNodeResult;
                case ExpressionType.Divide:
                    if (rightNodeResult == 0)
                        throw new DivideByZeroException(MathErrorMessager.DivisionByZero);
                    return leftNodeResult / rightNodeResult;
                default:
                    throw new Exception();
            }
        }
    }
}
