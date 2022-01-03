using System;
using System.Linq;

using Xunit;
using Xunit.Abstractions;

using Lcl.FilesystemUtilities.FsModel;

namespace UnitTests.FileSystemUtilities
{
  public class FsModelTests
  {
    private readonly ITestOutputHelper _output;

    public FsModelTests(ITestOutputHelper outputHelper)
    {
      _output = outputHelper;
    }

    [Fact]
    public void CanMakeAnchor()
    {
      var forest = new FsForest();
      
      var anchor = forest.GetAnchor(@"C:\Users\fluffy\");
      Assert.Equal("C:/Users/fluffy", anchor.Label);
    }

    [Fact]
    public void GetAnchorIsIdempotent()
    {
      var forest = new FsForest();

      var anchor = forest.GetAnchor(@"C:\Users\fluffy\");
      Assert.Equal("C:/Users/fluffy", anchor.Label);

      var anchor2 = forest.GetAnchor(@"C:\Users\fluffy\");
      Assert.Same(anchor, anchor2);
    }

    [Fact]
    public void AnchorNotMatchTest()
    {
      var forest = new FsForest();
      var _ = forest.GetAnchor(@"C:\Users\fluffy\");

      var match = forest.TryMatchAnchor("C:/Users", out var matchAnchor, out var matchTail);
      Assert.False(match);
      Assert.Null(matchAnchor);
      Assert.Null(matchTail);
    }

    [Fact]
    public void AnchorSplitMatchTest()
    {
      var forest = new FsForest();
      var anchor = forest.GetAnchor(@"C:\Users\fluffy\");

      // Test a normal anchor+tail match
      var match = forest.TryMatchAnchor("C:/Users/fluffy/Documents", out var matchAnchor, out var matchTail);
      Assert.True(match);
      Assert.NotNull(matchAnchor);
      Assert.NotNull(matchTail);
      Assert.Same(anchor, matchAnchor);
      Assert.Equal("Documents", matchTail);
    }

    [Fact]
    public void AnchorExactMatchTest()
    {
      var forest = new FsForest();
      var anchor = forest.GetAnchor(@"C:\Users\fluffy\");

      // Test exact match
      var match = forest.TryMatchAnchor("C:/Users/fluffy", out var matchAnchor, out var matchTail);
      Assert.True(match);
      Assert.NotNull(matchAnchor);
      Assert.NotNull(matchTail);
      Assert.Same(anchor, matchAnchor);
      Assert.Equal("", matchTail);
    }

    [Fact]
    public void AnchorCaseInsensitiveMatchTest()
    {
      var forest = new FsForest();
      var anchor = forest.GetAnchor(@"C:\Users\fluffy\");

      // Test case-insensivity
      var match = forest.TryMatchAnchor("c:/users/FLUFFY/documents", out var matchAnchor, out var matchTail);
      Assert.True(match);
      Assert.NotNull(matchAnchor);
      Assert.NotNull(matchTail);
      Assert.Same(anchor, matchAnchor);
      Assert.Equal("documents", matchTail);
    }

    [Fact]
    public void AnchorTrailingSlashMatchTest()
    {
      var forest = new FsForest();
      var anchor = forest.GetAnchor(@"C:\Users\fluffy\");

      // Test a normal anchor+tail match
      var match = forest.TryMatchAnchor("C:/Users/fluffy/Documents/", out var matchAnchor, out var matchTail);
      Assert.True(match);
      Assert.NotNull(matchAnchor);
      Assert.NotNull(matchTail);
      Assert.Same(anchor, matchAnchor);
      // Alert! the final '/' in 'C:/Users/fluffy/Documents/' is removed before matching, so it
      // will be missing from the tail!
      Assert.Equal("Documents", matchTail);
    }

  }
}

