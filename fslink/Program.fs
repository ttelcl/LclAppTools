// (c) 2021  ttelcl / ttelcl

open System
open System.IO

open CommonTools
open ExceptionTool
open Usage

let run arglist =
  match arglist with
  | "check" :: rest ->
    rest |> AppCheck.runCheck
  | []
  | "-h" :: _
  | "help" :: _ ->
    usage()
    0
  | x :: _ ->
    color Color.Red
    printf "Unrecognized command "
    color Color.DarkYellow
    x |> printf "%s"
    resetColor()
    printfn "."
    1
  

[<EntryPoint>]
let main args =
  try
    args |> Array.toList |> run
  with
    | ex ->
      ex |> fancyExceptionPrint verbose
      resetColor ()
      1



