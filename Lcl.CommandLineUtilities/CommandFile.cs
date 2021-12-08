/*
 * (c) 2021  HP / ttelcl
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lcl.CommandLineUtilities
{
  /// <summary>
  /// Static helpers for reading "command line" style content from
  /// files instead of the actual CLI
  /// </summary>
  public static class CommandFile
  {
    /// <summary>
    /// Read the content of a command file. The words in the file are returned
    /// as separate strings (supporting quotes to escape space-containing arguments
    /// in the same way as CSV files, and comments starting with '#')
    /// </summary>
    /// <param name="lines">
    /// The content lines of the command file to parse
    /// </param>
    /// <param name="folder">
    /// The path to the folder to use for resolving relative include files,
    /// or null to use the current working directory
    /// </param>
    /// <param name="recurseArgument">
    /// The argument that indicates the next argument is the name of 
    /// a command file to expand recursively. Or null to disable nested
    /// command file expansion
    /// </param>
    /// <param name="newlineMarker">
    /// An argument that is inserted for each end of line, or null not to use
    /// such a marker. This can help in adding semantics to lines in the command
    /// file.
    /// </param>
    /// <param name="maxRecurse">
    /// The maximum nesting level (default 8)
    /// </param>
    /// <returns>
    /// The words that were read from the input
    /// </returns>
    public static IEnumerable<string> ReadCommandFile(
      IEnumerable<string> lines,
      string folder = null,
      string recurseArgument = "-@",
      string newlineMarker = null,
      int maxRecurse = 8)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Read the content of a command file. The words in the file are returned
    /// as separate strings (supporting quotes to escape space-containing arguments
    /// in the same way as CSV files, and comments starting with '#')
    /// </summary>
    /// <param name="fileName">
    /// The name of the file whose content to load
    /// </param>
    /// <param name="recurseArgument">
    /// The argument that indicates the next argument is the name of 
    /// a command file to expand recursively. Or null to disable nested
    /// command file expansion
    /// </param>
    /// <param name="newlineMarker">
    /// An argument that is inserted for each end of line, or null not to use
    /// such a marker. This can help in adding semantics to lines in the command
    /// file.
    /// </param>
    /// <param name="maxRecurse">
    /// The maximum nesting level (default 8)
    /// </param>
    /// <returns>
    /// The words that were read from the input
    /// </returns>
    public static IEnumerable<string> ReadCommandFile(
      string fileName,
      string recurseArgument = "-@",
      string newlineMarker = null,
      int maxRecurse = 8)
    {
      fileName = Path.GetFullPath(fileName);
      var folder = Path.GetDirectoryName(fileName);
      return ReadCommandFile(
        File.ReadLines(fileName),
        folder,
        recurseArgument,
        newlineMarker,
        maxRecurse);
    }
  }
}
