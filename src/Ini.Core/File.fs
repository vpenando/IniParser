namespace Ini

open System

type File =
    { Sections : List<Section>
    }
    override this.ToString() =
        let sections =
            this.Sections
            |> List.sortBy (fun s -> s.Name)
            |> List.map (fun s -> s.ToString())
        in String.concat "\n\n" sections


type IniError =
    | SyntaxError of String


module File =
    let fromSections sections =
        { Sections = sections }


    let parseLines lines =
        let rec parseInner lines (sections: Section list) =
            match lines with
            | [] -> sections |> Ok

            | x::xs when x |> Inner.isSection ->
                let section = Inner.readSection x
                match section with
                | Some s ->
                    let newSections = sections @ [s]
                    in parseInner xs newSections
                | None -> x |> SyntaxError |> Error 
            
            | x::xs when x |> Inner.isField ->
                let field = Inner.readField x
                match field with
                | Some f ->
                    let newLastSection = List.last sections |> Section.addField f
                    let toTake = (List.length sections)-1
                    let newSections =
                        sections
                        |> List.take toTake
                        |> List.append [newLastSection]
                    in parseInner xs newSections
                | None -> x |> SyntaxError |> Error 
            
            | x::xs when Inner.isComment x ->
                // Skip this line if it is a comment
                parseInner xs sections
            
            | x::_ ->
                x |> SyntaxError |> Error 
        in parseInner lines [] |> Result.map fromSections


    let findSection name file =
        let predicate = fun (f: Section) -> f.Name = name
        let exists = List.exists predicate file.Sections
        if exists then
            List.findBack predicate file.Sections |> Some
        else None
