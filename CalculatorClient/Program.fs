module CalculatorClient
open System
open System.Net.Http

let matchOperation operation
    = match operation with
    | "+" -> "Plus"
    | "-" -> "Minus"
    | "*" -> "Multiply"
    | "/" -> "Divide"
    | _ -> operation

let calculateAsync (client: HttpClient) = 
    async{
        while true do
            let args = Console.ReadLine().Split()
            let operation = matchOperation args[1]
            let url = $"https://localhost:59659/calculate?value1={args[0]}&operation={operation}&value2={args[2]}"
            let! response = client.GetAsync(url) |> Async.AwaitTask
            let! result = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            printfn "%A" result
    }

[<EntryPoint>]
let main _ = 
    use client = new HttpClient()
    Async.RunSynchronously(calculateAsync client)
    0

    

