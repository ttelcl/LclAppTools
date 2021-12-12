/*
 * (c) 2021  ttelcl / ttelcl
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Lcl.CommandLineUtilities
{
  /// <summary>
  /// Static factories and other helper methods for token filters
  /// </summary>
  public static class TokenFilter
  {
    /// <summary>
    /// Create a token filter for expanding file inclusions
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

    /// <summary>
    /// Apply the filter to the given input tokens
    /// </summary>
    /// <param name="filter">
    /// The filter to apply
    /// </param>
    /// <param name="tokens">
    /// The tokens to apply the filter to
    /// </param>
    /// <returns>
    /// The token sequence after filtering
    /// </returns>
    public static IEnumerable<CmdLineToken> Filter(
      this ITokenFilter filter,
      IEnumerable<CmdLineToken> tokens)
    {
      return tokens.SelectMany(token => filter.FilterToken(token));
    }

    /// <summary>
    /// Apply the filter to the given input tokens
    /// </summary>
    /// <param name="tokens">
    /// The tokens to apply the filter to
    /// </param>
    /// <param name="filter">
    /// The filter to apply
    /// </param>
    /// <returns>
    /// The token sequence after filtering
    /// </returns>
    public static IEnumerable<CmdLineToken> Filter(
      this IEnumerable<CmdLineToken> tokens,
      ITokenFilter filter)
    {
      return tokens.SelectMany(token => filter.FilterToken(token));
    }

  }

  /// <summary>
  /// A trivial token filter that just passes the argument
  /// token stream unmodified
  /// </summary>
  internal class PassTokenFilter: ITokenFilter
  {
    /// <summary>
    /// Implements ITokenFilter by returning the argument token
    /// as a singleton
    /// </summary>
    public IEnumerable<CmdLineToken> FilterToken(CmdLineToken token)
    {
      yield return token;
    }
  }

}

