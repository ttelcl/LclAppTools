module AppCheck

open System
open System.IO

open CommonTools

open Lcl.FilesystemUtilities

type private CheckOptions = {
  Links: string list
}

let runCheck args =
  let rec parseMore o args =
    match args with
    | "-v" :: rest ->
      verbose <- true
      rest |> parseMore o
    | [] ->
      if o.Links |> List.isEmpty then
        failwith "No links to check provided"
      {o with Links = o.Links |> List.rev}
    | "-l" :: link :: rest ->
      rest |> parseMore {o with Links = link :: o.Links}
    | x :: _ ->
      x |> failwithf "unrecognized argument '%s'"
  let o = args |> parseMore { 
    Links = []
  }
  for link in o.Links do
    if link |> Directory.Exists then
      printf "Dir  "
      link |> printf "%s "
      let di = new DirectoryInfo(link)
      let target = di.LinkTarget
      if target = null then
        printf " [Not a link]"
      else
        printf " -> "
        target |> printf "%s"
    elif link |> File.Exists then
      printf "File "
      link |> printf "%s "
      let di = new FileInfo(link)
      let target = di.LinkTarget
      if target = null then
        printf " [Not a link]"
      else
        printf " -> "
        target |> printf "%s"
    else
      printf "---- "
      link |> printf "%s "
      printf " [Does not exist]"
  0
