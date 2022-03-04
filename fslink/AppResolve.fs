module AppResolve

open System
open System.IO

open CommonTools

open Lcl.FilesystemUtilities.FsModel

type private ResolveOptions = {
  Target: string
}

let runResolve args =
  let rec parsemore o args =
    match args with
    | "-v" :: rest ->
      verbose <- true
      rest |> parsemore o
    | target :: rest ->
      if target |> Directory.Exists |> not then
        target |> failwithf "Directory does not exist: %s"
      rest |> parsemore {o with Target = target}
    | [] ->
      if o.Target |> String.IsNullOrEmpty then
        failwith "No target directory specified"
      o
  let o = args |> parsemore {
    Target = null
  }
  let node = PathNode.TruePath(o.Target)
  printfn "%s" node.FullName
  0

