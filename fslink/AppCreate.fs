module AppCreate

open System
open System.IO
open System.Runtime.InteropServices
open System.ComponentModel
open System.Runtime.InteropServices

open CommonTools

open Lcl.FilesystemUtilities

type private CreateOptions = {
  LinkName: string
  JunctionMode: bool
  TargetName: string
  AllowExist: bool
}

let runCreate args =
  let rec parseMore o args =
    match args with
    | "-v" :: rest ->
      verbose <- true
      rest |> parseMore o
    | "-s" :: link :: rest ->
      rest |> parseMore {o with LinkName = link; JunctionMode = false}
    | "-j" :: junction :: rest ->
      if RuntimeInformation.IsOSPlatform(OSPlatform.Windows) |> not then
        failwith "'-j' requires Windows"
      rest |> parseMore {o with LinkName = junction; JunctionMode = true}
    | "-t" :: target :: rest ->
      rest |> parseMore {o with TargetName = target}
    | "-nx" :: rest ->
      rest |> parseMore {o with AllowExist = true}
    | [] ->
      if o.LinkName |> String.IsNullOrEmpty then
        failwith "No link name to create specified"
      if o.TargetName |> String.IsNullOrEmpty then
        failwith "No link target specified"
      o
    | x :: _ ->
      x |> failwithf "Unrecognized argument '%s'"
  let o = args |> parseMore {
    LinkName = null
    JunctionMode = false
    TargetName = null
    AllowExist = false
  }
  if o.TargetName |> File.Exists then
    if o.JunctionMode then
      failwith "Junction creation ('-j') requires a directory as target, not a file"
    // file link mode
    let target = new FileInfo(o.TargetName)
    let link = new FileInfo(o.LinkName |> Path.GetFullPath)
    if link.Exists then
      if o.AllowExist then
        failwith "Not yet implemented: '-nx' option. (link file exists)"
      else
        link.FullName |> failwithf "Link file already exists: %s"
    else
      File.CreateSymbolicLink(link.FullName, target.FullName) |> ignore
      printf "Created "
      color Color.Green
      link.FullName |> printf "%s"
      resetColor()
      printf " -> "
      color Color.DarkGreen
      target.FullName |> printf "%s"
      resetColor()
      printfn "."
    ()
  elif o.TargetName |> Directory.Exists then
    // directory link mode
    let target = new DirectoryInfo(o.TargetName)
    let link = new DirectoryInfo(o.LinkName |> Path.GetFullPath)
    if link.Exists then
      if o.AllowExist then
        failwith "Not yet implemented: '-nx' option. (link directory exists)"
      else
        link.FullName |> failwithf "Link directory already exists: %s"
    else
      if o.JunctionMode then
        let sl = target.CreateJunctionAs(link.FullName)
        printf "Created Junction "
        color Color.Cyan
        link.FullName |> printf "%s"
        resetColor()
        printf " -> "
        color Color.Blue
        target.FullName |> printf "%s"
        resetColor()
        printfn "."
      else
        let sl = target.CreateSymbolicLinkAs(link.FullName)
        printf "Created SymLink "
        color Color.Cyan
        link.FullName |> printf "%s"
        resetColor()
        printf " -> "
        color Color.Blue
        target.FullName |> printf "%s"
        resetColor()
        printfn "."

    ()
  else
    o.TargetName |> failwithf "Target directory / file does not exist: %s"
  0
