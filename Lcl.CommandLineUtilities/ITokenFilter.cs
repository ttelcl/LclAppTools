/*
 * (c) 2021  ttelcl / ttelcl
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lcl.CommandLineUtilities
{
  /// <summary>
  /// Description of ITokenFilter
  /// </summary>
  public interface ITokenFilter
  {
    /// <summary>
    /// Push the next token to be filtered into this filter and
    /// return the resulting filtered tokens
    /// </summary>
    IEnumerable<CommandLineParser.CmdLineToken> FilterToken(
      CommandLineParser.CmdLineToken token);
  }

  /// <summary>
  /// A trivial token filter that just passes the argument
  /// token stream unmodified
  /// </summary>
  public class PassTokenFilter: ITokenFilter
  {
    /// <summary>
    /// Implements ITokenFilter by returning the argument token
    /// as a singleton
    /// </summary>
    public IEnumerable<CommandLineParser.CmdLineToken> FilterToken(
      CommandLineParser.CmdLineToken token)
    {
      yield return token;
    }
  }

  /// <summary>
  /// Static factories for token filters
  /// </summary>
  public static class TokenFilter
  {
    /// <summary>
    /// Create a token filter for expanding 
    /// </summary>
    /// <param name="folder">
    /// The folder to use as reference for resolving relative paths. Or null
    /// to use the current directory.
    /// </param>
    /// <param name="recurseArgument">
    /// The argument that triggers recursive inclusion. The argument following
    /// this must be the name of the file to be included. If this is null,
    /// this method returns a dummy filter that does not support includion.
    /// </param>
    /// <param name="maxRecurse">
    /// Maximum recursive inclusion depth.
    /// </param>
    /// <returns>
    /// A new token filter.
    /// </returns>
    public static ITokenFilter InclusionFilter(
      string folder = null,
      string recurseArgument = "-@",
      int maxRecurse = 8)
    {
      if(String.IsNullOrEmpty(recurseArgument))
      {
        return DummyFilter();
      }
      folder =
        String.IsNullOrEmpty(folder)
        ? Environment.CurrentDirectory
        : Path.GetFullPath(folder);
      return new CommandFileRecursiveIncludeFilter(
        folder, recurseArgument, maxRecurse);
    }

    /// <summary>
    /// Returns a new PassTokenFilter, which just passes the input
    /// </summary>
    public static ITokenFilter DummyFilter()
    {
      return new PassTokenFilter();
    }

  }

}

