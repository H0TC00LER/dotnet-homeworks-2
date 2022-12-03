module Hw6.App

open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Microsoft.AspNetCore.Http
open Calculator.MaybeBuilder
open Calculator.Calculator
open System
open System.Globalization

let parsedQuery (ctx: HttpContext) = 
    let val1 = ctx.TryGetQueryStringValue "value1"
    let operation = ctx.TryGetQueryStringValue "operation"
    let val2 = ctx.TryGetQueryStringValue "value2"
    Ok (val1.Value, operation.Value, val2.Value)

let parseArg1 (arg1: string, operation, arg2: string) = 
    let tup = Double.TryParse(arg1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture)
    if (fst tup) then
        Ok (snd tup, operation, arg2)
    else
        Error $"Could not parse value '{arg1}'"

let parseArg2 (arg1: float, operation, arg2: string) = 
    let tup = Double.TryParse(arg2, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture)
    if (not (fst tup)) then
        Error $"Could not parse value '{arg2}'"
    else if (snd tup = 0 && operation = "Divide") then
        Error "DivideByZero"
    else
        Ok (arg1, operation, snd tup)

let parseOperation (arg1, operation, arg2) = 
    match operation with
    | "Minus" -> Ok (arg1, CalculatorOperation.Minus, arg2)
    | "Plus" -> Ok (arg1, CalculatorOperation.Plus, arg2)
    | "Multiply" -> Ok (arg1, CalculatorOperation.Multiply, arg2)
    | "Divide" -> Ok (arg1, CalculatorOperation.Divide, arg2)
    | _ -> Error $"Could not parse value '{operation}'"
    
let parseCalcArgs (args: HttpContext) = 
    maybe
        {
        let! a = args |> parsedQuery
        let! b = a |> parseArg1
        let! c = b |> parseArg2
        let! d = c |> parseOperation
        return d
        }

let calculatorHandler: HttpHandler =
    fun next ctx ->
        let parsedResults = parseCalcArgs ctx
        let result: Result<string, string> = 
            match parsedResults with
            | Ok resultOk ->
                let a, b, c = resultOk
                Ok ((calculate a b c).ToString())
            | Error error ->
                if (error = "DivideByZero") then
                    Ok "DivideByZero"
                else
                    Error (error.ToString())

        match result with
        | Ok ok -> (setStatusCode 200 >=> text (ok.ToString())) next ctx
        | Error error -> (setStatusCode 400 >=> text error) next ctx

let webApp =
    choose [
        GET >=> choose [
             route "/calculate" >=> calculatorHandler
        ]
        setStatusCode 404 >=> text "Not Found" 
    ]
    
type Startup() =
    member _.ConfigureServices (services : IServiceCollection) =
        services.AddGiraffe() |> ignore
    
        services.AddMemoryCache()
                .AddMiniProfiler() |> ignore

    member _.Configure (app : IApplicationBuilder) (_ : IHostEnvironment) (_ : ILoggerFactory) =
        app.UseMiniProfiler()
           .UseGiraffe webApp
        
[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun whBuilder -> whBuilder.UseStartup<Startup>() |> ignore)
        .Build()
        .Run()
    0