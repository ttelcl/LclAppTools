// (c) 2021  ttelcl / ttelcl
module Usage

open CommonTools
open PrintUtils

let usage() =
  py "Symbolic link utility"
  pn ""
  py "fslink check {-l <link>}"
  pn "   Checks if the specified files or directories are symbolic links (or junctions)"
  pn "   and prints where they point to."
  pn ""
  py "fslink create [-nx] [-s <link>|-j <junction>] -t <target>"
  pn "   Creates a symbolic link or junction"
  p2 "-s <link>        The name of the symbolic link to create"
  py "                 ALERT: this likely requires administrator privileges!"
  p2 "-j <junction>    The name of the junction directory to create. (Windows only)"
  p2 "-t <target>      The target file (-s only) or directory (-s or -j)"
  p2 "-nx              If the link already exists and points to the given target"
  p2 "                 do not fail but succeed (only create if Not eXisting)"
  pn ""
  py "fslink resolve <directory>"
  pn "  Prints the full path the directory resolves to after following any"
  pn "  symbolic links or junctions in the path."
  pn "  (does not resolve SUBST drives on Windows)"
  pn ""
  py "General options:"
  p2 "-v               Verbose mode"



