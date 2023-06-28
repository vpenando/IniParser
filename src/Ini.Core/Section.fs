namespace Ini

open System

type Section =
    { Name : String
    ; Fields : List<Field>
    }
    override this.ToString() =
        let fields =
            this.Fields
            |> List.map (fun f -> f.ToString())
            |> String.concat "\n"
        $"[{this.Name}]\n{fields}"


module Section =
    let empty name =
        { Name = name; Fields = [] }


    let addField field section =
        { section with Fields = section.Fields @ List.singleton field }


    let findField name section =
        let predicate = fun (f: Field) -> f.Name = name
        let exists = List.exists predicate section.Fields
        if exists then
            List.findBack predicate section.Fields |> Some
        else None
