/*
 * (c) 2021  ttelcl / ttelcl
 */

namespace Lcl.CommandLineUtilities
{

  /// <summary>
  /// The types of tokens detected in commandlines
  /// </summary>
  public enum CmdLineTokenType
  {
    /// <summary>
    /// An argument, either unquoted or quoted-and-decoded
    /// </summary>
    Argument = 1,

    /// <summary>
    /// A comment
    /// </summary>
    Comment = 2,

    /// <summary>
    /// The end of of the input line was found
    /// </summary>
    Eoln = 3,
  }

  /// <summary>
  /// Wraps a commandline token plus its type
  /// </summary>
  public struct CmdLineToken
  {
    /// <summary>
    /// Create a new CmdLineToken
    /// </summary>
    public CmdLineToken(CmdLineTokenType type, string value)
    {
      TokenType = type;
      Value = value;
    }

    /// <summary>
    /// The kind of token that was found
    /// </summary>
    public CmdLineTokenType TokenType { get; }

    /// <summary>
    /// The string value of the token (possibly null, depending on the type)
    /// </summary>
    public string Value { get; }
  }

}
