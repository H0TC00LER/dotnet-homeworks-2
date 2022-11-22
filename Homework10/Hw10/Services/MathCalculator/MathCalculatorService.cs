using Hw10.Dto;
using Hw10.Services.Parser;

namespace Hw10.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        try
        {
            var parsedExpression = Parser.Parser.Parse(expression);
            var expressionTree = ExpressionBuilder.BuildExpressionTree(parsedExpression);
            var visitor = new Visitor();
            var result = await visitor.Visit(expressionTree);
            return new CalculationMathExpressionResultDto(result);
        }
        catch (Exception ex)
        {
            return new CalculationMathExpressionResultDto(ex.Message);
        }
    }
}