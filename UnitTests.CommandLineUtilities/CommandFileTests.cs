using System;
using System.Linq;

using Lcl.CommandLineUtilities;

using Xunit;
using Xunit.Abstractions;

namespace UnitTests.CommandLineUtilities
{
  public class CommandFileTests
  {
    private readonly ITestOutputHelper _output;

    public CommandFileTests(ITestOutputHelper outputHelper)
    {
      _output = outputHelper;
    }

    [Fact]
    public void InlineArgsTest()
    {
      var lines = new[] {
        "-x foo bar -y \"yip\" #COMMENT",
        "-z zzz"
      };

      var tokens =
        CommandFile.TokenizeCommandFile(lines)
        .ToList();
      _output.WriteLine("Tokens:");
      foreach(var token in tokens)
      {
        switch(token.TokenType)
        {
          case CmdLineTokenType.Argument:
            _output.WriteLine($"ARG: {token.Value}");
            break;
          case CmdLineTokenType.Comment:
            _output.WriteLine($"CMT: {token.Value}");
            break;
          case CmdLineTokenType.Eoln:
            _output.WriteLine("EOLN");
            break;
        }
      }
      Assert.Equal(10, tokens.Count);

      var args =
        CommandFile.ReadCommandFile(lines, newlineMarker: null)
        .ToList();
      Assert.Equal(7, args.Count);

      var args2 =
        CommandFile.ReadCommandFile(lines, newlineMarker: "---")
        .ToList();
      _output.WriteLine("Arguments (with EOLN markers):");
      foreach(var arg in args2)
      {
        _output.WriteLine($"{arg}");
      }
      Assert.Equal(9, args2.Count);
    }
  
    [Fact]
    public void FromFileTest1()
    {
      var args =
        CommandFile.ReadCommandFile("Commands1.txt")
        .ToList();
      _output.WriteLine("Arguments:");
      foreach(var arg in args)
      {
        _output.WriteLine($"{arg}");
      }
    }

    [Fact]
    public void IncludeFileTest1()
    {
      var args =
        CommandFile.ReadCommandFile("CommandInclusion1.txt")
        .ToList();
      _output.WriteLine("Arguments:");
      foreach(var arg in args)
      {
        _output.WriteLine($"{arg}");
      }
    }


    [Fact]
    public void IncludeFileTest2()
    {
      var args =
        CommandFile.ReadCommandFile("CommandInclusion2.txt", maxRecurse: 2)
        .ToList();
      _output.WriteLine("Arguments:");
      foreach(var arg in args)
      {
        _output.WriteLine($"{arg}");
      }

      Assert.Throws<InvalidOperationException>(() => {
        args =
          CommandFile.ReadCommandFile("CommandInclusion2.txt", maxRecurse: 1)
          .ToList();
      });
    }

  }
}
