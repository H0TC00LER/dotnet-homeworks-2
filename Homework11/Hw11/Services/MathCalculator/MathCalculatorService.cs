using Hw11.Dto;
using Hw11.Services.Parser;

namespace Hw11.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        var parsedExpression = Parser.Parser.Parse(expression);
        dynamic expressionTree = ExpressionBuilder.BuildExpressionTree(parsedExpression);
        var visitor = new Visitor();
        var result = await visitor.Visit(expressionTree);
        return result;
        //return new CalculationMathExpressionResultDto(ex.Message);
    }
}