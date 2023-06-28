module Ini.Tests.Section

open Xunit
open Ini


module private Input =
    let field = Field.newField "fieldName" "someValue"


[<Fact>]
let ``findField() with existing field`` () =
    Section.empty "Test"
        |> Section.addField Input.field
        |> Section.findField "fieldName" |> Option.isSome
        |> Assert.True


[<Fact>]
let ``findField() with unexisting field`` () =
    Section.empty "Test"
        |> Section.addField Input.field
        |> Section.findField "invalidFieldName" |> Option.isNone
        |> Assert.True
