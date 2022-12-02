﻿module Calculator.MaybeBuilder

type MaybeBuilder() =
    member builder.Bind(a, f): Result<'e,'d> =
        match a with
        | Ok a -> f a
        | Error err -> Error err
    member builder.Return x: Result<'a,'b> =
        Ok x
let maybe = MaybeBuilder()