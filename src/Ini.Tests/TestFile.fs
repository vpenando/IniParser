module Ini.Tests.File

open Xunit
open Ini


module private Input =
    let lines = [ "[Test]"; "field1=value1"; "field2=value2"; "; comment" ]
    let linesWithSyntaxError = [ "[Test]"; "field1=value1"; "field2=value2"; "syntaxError" ]


// Ok(Some(x)) => Some(x)
let private unwrapSection = function
    | Ok opt -> opt
    | Error _ -> None


[<Fact>]
let ``parseLines() with valid lines is Ok`` () =
    File.parseLines Input.lines
        |> Result.isOk
        |> Assert.True
    

[<Fact>]
let ``parseLines() with syntax error is SyntaxError`` () =
    let predicate res = res = (Error <| SyntaxError "syntaxError")
    in File.parseLines Input.linesWithSyntaxError
        |> predicate
        |> Assert.True


[<Fact>]
let ``findSection() with existing section`` () =
    File.parseLines Input.lines
        |> Result.map (File.findSection "Test")
        |> unwrapSection
        |> Option.isSome
        |> Assert.True


[<Fact>]
let ``findSection() with unexisting section`` () =
    File.parseLines Input.lines
        |> Result.map (File.findSection "Unexisting")
        |> unwrapSection
        |> Option.isNone
        |> Assert.True
