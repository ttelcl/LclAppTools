/*
 * (c) 2021  ttelcl / ttelcl
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
  /// Filters a sequence of CommandLineParser.CmdLineTokens to
  /// expand recursive includes
  /// </summary>
  public class CommandFileRecursiveIncludeFilter: ITokenFilter
  {
    private RecursionFilterState _state;

    /// <summary>
    /// Create a new CommandFileRecursiveIncludeFilter
    /// </summary>
    public CommandFileRecursiveIncludeFilter(
      string folder,
      string recurseArgument = "-@",
      int maxRecurse = 8)
    {
      RecurseKey = recurseArgument;
      MaxRecurse = maxRecurse;
      ReferenceFolder = Path.GetFullPath(folder);
      _state = RecursionFilterState.Pass;
    }

    /// <summary>
    /// The key argument that triggers recursion; the following
    /// argument is the name of the file to recursively include
    /// </summary>
    public string RecurseKey { get; }

    /// <summary>
    /// The maximum recursion level.
    /// </summary>
    public int MaxRecurse { get; }

    /// <summary>
    /// The folder against which to resolve relative file paths
    /// </summary>
    public string ReferenceFolder { get; }

    /// <summary>
    /// Implements ITokenFilter, passing most arguments directly,
    /// but trapping and expanding inclusions
    /// </summary>
    public IEnumerable<CommandLineParser.CmdLineToken> FilterToken(
      CommandLineParser.CmdLineToken token)
    {
      switch(_state)
      {
        case RecursionFilterState.Pass:
          if(token.TokenType == CmdLineTokenType.Argument
            && token.Value == RecurseKey)
          {
            if(MaxRecurse <= 0)
            {
              throw new InvalidOperationException(
                "Include recursion limit exceeded");
            }
            _state = RecursionFilterState.WaitForFile;
            // do not yield anything in this case
          }
          else
          {
            yield return token;
          }
          break;
        case RecursionFilterState.WaitForFile:
          if(token.TokenType != CmdLineTokenType.Argument)
          {
            throw new InvalidOperationException(
              "Expecting a plain argument after the argument include marker");
          }
          var fileName = token.Value;
          fileName = Path.Combine(ReferenceFolder, fileName);
          if(!File.Exists(fileName))
          {
            throw new FileNotFoundException(
              "Included command file not found",
              fileName);
          }
          var childFolder = Path.GetDirectoryName(fileName);
          var childFilter = new CommandFileRecursiveIncludeFilter(
            childFolder, RecurseKey, MaxRecurse-1);
          foreach(var token2 in CommandFile.TokenizeCommandFile(fileName, childFilter))
          {
            yield return token2;
          }
          _state = RecursionFilterState.Pass;
          break;
        default:
          throw new InvalidOperationException(
            $"Internal error: unknown recursion filter state '{_state}'");
      }
    }

  }


  internal enum RecursionFilterState
  {
    Pass = 0,

    WaitForFile = 1,
  }
}
