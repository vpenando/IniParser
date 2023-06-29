let allLines = 
    [ "[Section1] ; this is a comment"
    ; "name1 = value1"
    ; "name2 = value2"
    ; "; another comment here"

    ; "[Section2]"
    ; "name3 = value3"
    ]


let printContent = function
    | Ok content ->
        printfn "%s" (content.ToString())
        
    | Error e ->
        printfn "Error: %A" e


allLines
    |> Ini.File.parseLines
    |> printContent
