open Hw5.Calculator
open Hw5.Parser
open Hw5

let Main val1 operation val2 = 
    let args = [|val1; operation; val2|]
    let parserResult = parseCalcArguments args
    match parserResult with
    | Ok resultOk ->
        let a, b, c = resultOk
        printfn "%A" (calculate a b c)
    | Error error -> printfn "%A" error