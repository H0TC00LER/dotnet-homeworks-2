using Hw9.Dto;
using Hw9.ErrorMessages;
using Hw9.Parser;
using System.Buffers;
using System.Linq.Expressions;

namespace Hw9.Services.MathCalculator;

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
        catch(Exception ex)
        {
            return new CalculationMathExpressionResultDto(ex.Message);
        }
    }
}