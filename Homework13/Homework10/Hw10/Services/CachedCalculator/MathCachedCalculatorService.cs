using Hw10.DbModels;
using Hw10.Dto;
using System.Runtime.Caching;

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
		var result = MemoryCache.Default.Get(expression) as CalculationMathExpressionResultDto;
		if (result != null)
			return result;
		
		result = await _simpleCalculator.CalculateMathExpressionAsync(expression);
		
		MemoryCache.Default.Set(expression, result,
			DateTimeOffset.Now.Add(new TimeSpan(0, 0, 30, 0)));

		return result;
	}
}