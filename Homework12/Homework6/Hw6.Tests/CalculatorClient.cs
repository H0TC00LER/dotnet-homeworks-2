using Hw6;
using System;
using System.Threading.Tasks;

namespace Hw6Tests
{
    public class CalculatorClient
    {
        private readonly CustomWebApplicationFactory<App.Startup> _factory;
        public CalculatorClient(CustomWebApplicationFactory<App.Startup> factory)
        {
            _factory = factory;
        }
        private async Task Calculate(string value1, string value2, string operation)
        {
            var url = $"/calculate?value1={value1}&operation={operation}&value2={value2}";

            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine(result);
        }

        private string MatchOperations(string operation)
        {
            switch (operation)
            {
                case "+": return "Plus";
                case "-": return "Minus";
                case "*": return "Multiply";
                case "/": return "Divide";
                default: return operation;
            }
        }

        public async void HandleInput()
        {
            while (true)
            {
                var input = Console.ReadLine().Split();
                if (input.Length > 3)
                    Console.WriteLine("Wrong argument length");
                var operation = MatchOperations(input[1]);
                await Calculate(input[0], input[2], operation);
            }
        }
    }
}
