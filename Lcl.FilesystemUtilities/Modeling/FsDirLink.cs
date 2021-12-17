/*
 * (c) 2021  ttelcl / ttelcl
 */

namespace Lcl.FilesystemUtilities.Modeling
{
  /// <summary>
  /// A link to a directory
  /// </summary>
  public class FsDirLink: FsInnerNode
  {
    /// <summary>
    /// Create a new FsDirLink
    /// </summary>
    /// <param name="name">
    /// The link's name (without path)
    /// </param>
    /// <param name="linkTarget">
    /// The target path
    /// </param>
    /// <param name="parent">
    /// The parent directory, or null for a detached node
    /// </param>
    public FsDirLink(string name, string linkTarget, FsDirNode? parent = null)
      : base(name, linkTarget, parent)
    {
    }

    /// <summary>
    /// The list of children (an empty list if not resolved)
    /// </summary>
    public override IReadOnlyList<FsTreeNode> Children { get => GetChildren(); }

    /// <summary>
    /// An empty list of child nodes, returned by Children for unresolved links
    /// </summary>
    public static IReadOnlyList<FsTreeNode> EmptyChildList = Array.Empty<FsTreeNode>();

    /// <summary>
    /// The resolved target node, or null if not resolved
    /// </summary>
    public FsDirNode? ResolvedTarget { get; private set; }
    
    private IReadOnlyList<FsTreeNode> GetChildren()
    {
      return ResolvedTarget == null ? EmptyChildList : ResolvedTarget.Children;
    }
  }


}
