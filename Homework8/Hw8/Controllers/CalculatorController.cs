using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;
using static System.Double;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<double> Calculate([FromServices] ICalculator calculator,
        string val1,
        string operation,
        string val2)
    {
        if (TryParse(val1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double parsedVal1) &&
            TryParse(val2, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double parsedVal2))
        {
            switch (operation)
            {
                case "Plus":
                    return calculator.Plus(parsedVal1, parsedVal2);
                case "Minus":
                    return calculator.Minus(parsedVal1, parsedVal2);
                case "Multiply":
                    return calculator.Multiply(parsedVal1, parsedVal2);
                case "Divide":
                    if (parsedVal2 == 0)
                        return Content(Messages.DivisionByZeroMessage);
                    return calculator.Divide(parsedVal1, parsedVal2);
                default:
                    return Content(Messages.InvalidOperationMessage);
            }
        }

        return Content(Messages.InvalidNumberMessage);
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}