// (c) 2021  ttelcl / ttelcl
module Usage

open CommonTools
open PrintUtils

let usage() =
  py "Symbolic link utility"
  pn ""
  py "fslink check {-l <link>}"
  pn "   Checks if the specified files or directories are symbolic links and"
  pn "   prints where they point to"
  pn ""
  py "fslink create [-nx] -l <link> -t <target>"
  pn "   Creates a symbolic link"
  p2 "-l <link>        The name of the link to create"
  p2 "-t <target>      The target file or directory"
  p2 "-nx              If the link already exists and points to the given target"
  p2 "                 do not fail but succeed (only create if Not eXisting)"
  pn ""
  py "General options:"
  p2 "-v               Verbose mode"



