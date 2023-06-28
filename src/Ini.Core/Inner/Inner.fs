module private Inner

open System

let private removeComment (line: String) =
    let trimmed = line.Trim()
    let hasComment = String.exists ((=) ';') trimmed
    if hasComment then 
        let commentIndex = trimmed.IndexOf ";"
        let trimmedWithoutComment = trimmed.Substring(0, commentIndex)
        in trimmedWithoutComment.TrimEnd()
    else line


let rec getSectionName (section: String) =
    let trimmed = section.Trim() |> removeComment
    let rec readNameWithoutBrackets (s: String) = function
        | '['::xs -> readNameWithoutBrackets s xs
        | ']'::_ -> s
        | x::xs -> readNameWithoutBrackets $"{s}{x}" xs
        | _ -> failwith "should not happen"
    if trimmed.StartsWith('[') && trimmed.EndsWith(']') && trimmed.Length > 2 then
        let characterList = trimmed |> Seq.toList
        let name = readNameWithoutBrackets "" characterList
        in Some name
    else None


let isComment (line: String) =
    line.TrimStart().StartsWith(";")


let rec isSection (line: String) =
    let trimmed = line.Trim() |> removeComment
    trimmed.StartsWith "[" && trimmed.EndsWith "]" && trimmed.Length > 2
        && (1 = (trimmed |> String.filter ((=) '[') |> String.length))
        && (1 = (trimmed |> String.filter ((=) ']') |> String.length))


let isField (line: String) =
    let field = line |> removeComment
    let equalitySymbols = String.filter ((=) '=') field
    in String.length equalitySymbols > 0
        && field.IndexOf('=') <> 0


let readSection =
    getSectionName >> Option.map Ini.Section.empty


let readField (line: String) =
    if isField line then
        let indexOfAssignmentOp = line.IndexOf('=')
        let name = line.Substring(0, indexOfAssignmentOp).Trim()
        let value = line.Substring(indexOfAssignmentOp+1).Trim()
        let field = Ini.Field.newField name value
        in Some field
    else None
