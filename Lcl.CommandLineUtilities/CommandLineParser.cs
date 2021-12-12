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
  internal enum CmdParseMode
  {
    /// <summary>
    /// WhiteSpace in between arguments (or the virtual space before the first
    /// character). The next character determines the parse mode for the next token.
    /// </summary>
    Idle = 0,

    /// <summary>
    /// No more tokens on the current line
    /// </summary>
    Done = 1
  }

  /// <summary>
  /// Implements the command file line parser state machine
  /// </summary>
  internal class CommandLineParser
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
        _mode = CmdParseMode.Idle;
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
        return new CmdLineToken(CmdLineTokenType.Eoln, null, LineNumber);
      }
      if(_mode != CmdParseMode.Idle)
      {
        throw new InvalidOperationException(
          $"Unexpected parser state {_mode}");
      }
      SkipWhitespace();
      if(_pointer >= _input.Length)
      {
        _mode = CmdParseMode.Done;
        return new CmdLineToken(CmdLineTokenType.Eoln, null, LineNumber);
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
            return new CmdLineToken(CmdLineTokenType.Argument, sb.ToString(), LineNumber);
          }
          else if(_input[_pointer] != '\"')
          {
            if(Char.IsWhiteSpace(_input[_pointer]))
            {
              _mode = CmdParseMode.Idle;
              return new CmdLineToken(CmdLineTokenType.Argument, sb.ToString(), LineNumber);
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
        _mode = CmdParseMode.Idle;
        _pointer = _input.Length;
        return new CmdLineToken(CmdLineTokenType.Comment, comment, LineNumber);
      }
      else
      {
        var p = _pointer;
        while(p < _input.Length && !Char.IsWhiteSpace(_input[p]))
        {
          p++;
        }
        var arg = _input.Substring(_pointer, p-_pointer);
        _mode = CmdParseMode.Idle;
        _pointer = p;
        return new CmdLineToken(CmdLineTokenType.Argument, arg, LineNumber);
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
  }

}
