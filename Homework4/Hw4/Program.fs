open Hw4.Calculator
open Hw4.Parser

let Main val1 operation val2 = 
    let args = [|val1; operation; val2|]
    let options = parseCalcArguments args
    let result = calculate options.arg1 options.operation options.arg2
    printfn "Result is %f" result