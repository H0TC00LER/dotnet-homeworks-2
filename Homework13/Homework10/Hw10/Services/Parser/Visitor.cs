using Hw10.ErrorMessages;
using System.Linq.Expressions;

namespace Hw10.Services.Parser
{
    public class Visitor : ExpressionVisitor
    {
        public async Task<double> Visit(Expression node)
        {
            var nodeAsBinaryExpression = node as BinaryExpression;

            if (nodeAsBinaryExpression == null)
            {
                var nodeAsConstantExpression = node as ConstantExpression;
                if (nodeAsConstantExpression != null)
                    return Convert.ToDouble(nodeAsConstantExpression.Value);
            }

            await Task.Delay(1000);

            var tasks = await Task.WhenAll(new Task<double>[] { Visit(nodeAsBinaryExpression.Left), this.Visit(nodeAsBinaryExpression.Right) });
            var leftNode = tasks[0];
            var rightNode = tasks[1];

            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    return leftNode + rightNode;
                case ExpressionType.Subtract:
                    return leftNode - rightNode;
                case ExpressionType.Multiply:
                    return leftNode * rightNode;
                case ExpressionType.Divide:
                    if (rightNode == 0)
                        throw new Exception(MathErrorMessager.DivisionByZero);
                    return leftNode / rightNode;
                default:
                    throw new Exception();
            }
        }
    }
}
