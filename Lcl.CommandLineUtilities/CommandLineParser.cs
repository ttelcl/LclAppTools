/*
 * (c) 2021  ttelcl / ttelcl
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lcl.CommandLineUtilities
{
  /// <summary>
  /// Parser states
  /// </summary>
  public enum CmdParseMode
  {
    /// <summary>
    /// Normal commandline content (not escaped, not comment)
    /// </summary>
    Normal = 0,

    /// <summary>
    /// Inside a quoted string, but not handling an escaped inner quote
    /// </summary>
    Quoted = 1,

    /// <summary>
    /// Inside a quoted string, or just after, could be handling an escaped inner quote
    /// </summary>
    QuoteEnd = 2,

    /// <summary>
    /// Inside a comment
    /// </summary>
    Comment = 3,

    /// <summary>
    /// WhiteSpace in between arguments (or the virtual space before the first
    /// character). The next character determines the parse mode for the next token.
    /// </summary>
    WhiteSpace = 4,

    /// <summary>
    /// No more tokens on the current line
    /// </summary>
    Done = 5
  }

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
  /// Description of CommandLineParser
  /// </summary>
  public class CommandLineParser
  {
    private string _input;
    private int _pointer;
    private int _lineNumber;
    private CmdParseMode _mode;

    /// <summary>
    /// Create a new CommandLineParser
    /// </summary>
    public CommandLineParser()
    {
      _input = null;
      _lineNumber = 0;
      _mode = CmdParseMode.Done;
    }

    /// <summary>
    /// Bind to the next input line
    /// </summary>
    public void StartNextLine(string input)
    {
      _input = input;
      _lineNumber++;
      _pointer = 0;
      if(_input == null || _pointer>=_input.Length)
      {
        _mode = CmdParseMode.Done;
      }
      else
      {
        _mode = CmdParseMode.WhiteSpace;
      }
    }

    /// <summary>
    /// The current line number (or 0 before any input)
    /// </summary>
    public int LineNumber => _lineNumber;

    /// <summary>
    /// True after the current line has been completely parsed
    /// </summary>
    public bool Done => _mode == CmdParseMode.Done || _input == null;

    /// <summary>
    /// Parse the next token
    /// </summary>
    public CmdLineToken NextToken()
    {
      if(Done)
      {
        return new CmdLineToken(CmdLineTokenType.Eoln, null);
      }
      if(_mode != CmdParseMode.WhiteSpace)
      {
        throw new InvalidOperationException(
          $"Unexpected parser state {_mode}");
      }
      SkipWhitespace();
      if(_pointer >= _input.Length)
      {
        _mode = CmdParseMode.Done;
        return new CmdLineToken(CmdLineTokenType.Eoln, null);
      }
      // At this point _pointer points to the start of one of:
      // - a normal argument
      // - a quoted argument
      // - a comment
      if(_input[_pointer] == '\"')
      {
        var sb = new StringBuilder();
        _pointer++;
        while(true)
        { 
          while(_pointer < _input.Length && _input[_pointer] != '\"')
          {
            sb.Append(_input[_pointer++]);
          }
          if(_pointer >= _input.Length)
          {
            throw new InvalidOperationException(
              $"Invalid command file: unterminated quoted argument on line {_lineNumber}");
          }
          _pointer++;
          if(_pointer >= _input.Length)
          {
            _mode = CmdParseMode.Done;
            return new CmdLineToken(CmdLineTokenType.Argument, sb.ToString());
          }
          else if(_input[_pointer] != '\"')
          {
            if(Char.IsWhiteSpace(_input[_pointer]))
            {
              _mode = CmdParseMode.WhiteSpace;
              return new CmdLineToken(CmdLineTokenType.Argument, sb.ToString());
            }
            else
            {
              throw new InvalidOperationException(
                $"Invalid command file: found undoubled '\"' inside a quoted string on line {_lineNumber}");
            }
          }
          else
          {
            sb.Append(_input[_pointer++]); // push the '"' into the buffer and continue
          }
        }
        throw new InvalidOperationException("This code should have been unreachable");
      }
      else if(_input[_pointer] == '#')
      {
        var comment = _input.Substring(_pointer);
        _mode = CmdParseMode.WhiteSpace;
        _pointer = _input.Length;
        return new CmdLineToken(CmdLineTokenType.Comment, comment);
      }
      else
      {
        var p = _pointer;
        while(p < _input.Length && !Char.IsWhiteSpace(_input[p]))
        {
          p++;
        }
        var arg = _input.Substring(_pointer, p-_pointer);
        _mode = CmdParseMode.WhiteSpace;
        _pointer = p;
        return new CmdLineToken(CmdLineTokenType.Argument, arg);
      }
    }

    /// <summary>
    /// Parse all remaining tokens on the current line
    /// </summary>
    public IEnumerable<CmdLineToken> ParseAll()
    {
      while(!Done)
      {
        var token = NextToken();
        yield return token;
      }
    }

    private void SkipWhitespace()
    {
      while(_pointer < _input.Length && Char.IsWhiteSpace(_input[_pointer]))
      {
        _pointer++;
      }
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
}
