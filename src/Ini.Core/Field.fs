namespace Ini

open System

type Field =
    { Name : String
    ; Value : String
    }
    override this.ToString() =
        $"{this.Name} = {this.Value}"


module Field =
    let newField name value =
        { Name = name; Value = value }
