module AppCreate

open System
open System.IO

open CommonTools

open Lcl.FilesystemUtilities
open System.Runtime.InteropServices
open System.ComponentModel

type private CreateOptions = {
  LinkName: string
  TargetName: string
  AllowExist: bool
}

let runCreate args =
  let rec parseMore o args =
    match args with
    | "-v" :: rest ->
      verbose <- true
      rest |> parseMore o
    | "-l" :: link :: rest ->
      rest |> parseMore {o with LinkName = link}
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
    TargetName = null
    AllowExist = false
  }
  if o.TargetName |> File.Exists then
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
      let sl = Directory.CreateSymbolicLink(link.FullName, target.FullName)
      let e = Marshal.GetLastPInvokeError()
      if e <> 0 then
        new Win32Exception(e) |> raise
      printf "Created "
      color Color.Cyan
      link.FullName |> printf "%s"
      resetColor()
      printf " -> "
      color Color.Blue
      target.FullName |> printf "%s"
      resetColor()
      printfn "."
      // printfn "(%O , %O, %d, %d)" sl (sl.Exists) e0 e1

    ()
  else
    o.TargetName |> failwithf "Target directory / file does not exist: %s"
  0
