/*
 * (c) 2021  ttelcl / ttelcl
 */

using System.Collections.Generic;
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
    IEnumerable<CmdLineToken> FilterToken(CmdLineToken token);
  }

}

