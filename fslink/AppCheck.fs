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
      let di = new DirectoryInfo(link)
      let target = di.LinkTarget
      if target = null then
        color Color.Blue
        link |> printf "%s "
        color Color.DarkGray
        printf " [Not a link]"
        resetColor()
      else
        color Color.Cyan
        link |> printf "%s "
        color Color.Yellow
        printf " -> "
        color Color.Blue
        target |> printf "%s"
        resetColor()
    elif link |> File.Exists then
      printf "File "
      let di = new FileInfo(link)
      let target = di.LinkTarget
      if target = null then
        color Color.DarkGreen
        link |> printf "%s "
        color Color.DarkGray
        printf " [Not a link]"
        resetColor()
      else
        color Color.Green
        link |> printf "%s "
        color Color.Yellow
        printf " -> "
        color Color.DarkGreen
        target |> printf "%s"
        resetColor()
    else
      printf "---- "
      color Color.Red
      link |> printf "%s "
      color Color.DarkYellow
      printf " [Does not exist]"
      resetColor()
    printfn "."
  0
