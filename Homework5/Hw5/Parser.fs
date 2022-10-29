module Hw5.Parser

open System
open Hw5.Calculator
open Hw5.MaybeBuilder
open System.Globalization

let isArgLengthSupported (args:string[]): Result<'a,'b> =
    match args.Length with
    | 3 -> Ok args
    | _ -> Error Message.WrongArgLength
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with
    | "-" -> Ok (arg1, CalculatorOperation.Minus, arg2)
    | "+" -> Ok (arg1, CalculatorOperation.Plus, arg2)
    | "*" -> Ok (arg1, CalculatorOperation.Multiply, arg2)
    | "/" -> Ok (arg1, CalculatorOperation.Divide, arg2)
    | _ -> Error Message.WrongArgFormatOperation

let parseArgs (args: string[]): Result<('a * CalculatorOperation * 'b), Message> =
    let tup1 = Double.TryParse(args[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture)
    let tup2 = Double.TryParse(args[2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture)
    if (fst tup1 && fst tup2)
    then 
        isOperationSupported (snd tup1, args[1], snd tup2)
    else
        Error Message.WrongArgFormat
    

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match arg2 with
    | 0.0 -> Error Message.DivideByZero
    | _ -> Ok (arg1, operation, arg2)
    
let parseCalcArguments (args: string[]): Result<'a, 'b> =
    maybe
        {
        let! a = args |> isArgLengthSupported
        let! b = a |> parseArgs
        let! c = b |> isDividingByZero
        return c
        }