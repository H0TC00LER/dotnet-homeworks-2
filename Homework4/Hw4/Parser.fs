module Hw4.Parser

open System
open Hw4.Calculator


type CalcOptions = {
    arg1: float
    arg2: float
    operation: CalculatorOperation
}

let isArgLengthSupported (args : string[]) =
    args.Length = 3

let parseOperation (arg : string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> ArgumentException() |> raise
    
let parseCalcArguments(args : string[]) =
    let mutable arg1 = 1.0
    let mutable arg2 = 1.0
 
    if 
        (isArgLengthSupported args && Double.TryParse(args[0], &arg1) && Double.TryParse(args[2], &arg2))
    then
        {arg1 = arg1; arg2 = arg2; operation = parseOperation args[1]}
    else
        ArgumentException() |> raise

