using Hw10.DbModels;
using Hw10.Dto;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly ApplicationContext _dbContext;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(ApplicationContext dbContext, IMathCalculatorService simpleCalculator)
	{
		_dbContext = dbContext;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		var solvingExpressions = _dbContext.SolvingExpressions;
		var solvingExpression = solvingExpressions.FirstOrDefault(s => s.Expression == expression);

        if (solvingExpression != null)
		{
			return new CalculationMathExpressionResultDto(solvingExpression.Result);
		}

		var result = await _simpleCalculator.CalculateMathExpressionAsync(expression);
		if (result.IsSuccess)
		{
            await solvingExpressions.AddAsync(new SolvingExpression { Expression = expression, Result = result.Result });
            await _dbContext.SaveChangesAsync();
        }
		return result;
    }
}