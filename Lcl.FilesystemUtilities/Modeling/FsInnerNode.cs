/*
 * (c) 2021  ttelcl / ttelcl
 */

namespace Lcl.FilesystemUtilities.Modeling
{
  /// <summary>
  /// An FsTreeNode that can have child nodes: either an actual directory or
  /// a link to a directory
  /// </summary>
  public abstract class FsInnerNode: FsTreeNode
  {
    /// <summary>
    /// Create a new FsInnerNode
    /// </summary>
    /// <param name="name">
    /// The name of the node (directory), without path
    /// </param>
    /// <param name="linkTarget">
    /// If not null: the target directory name
    /// </param>
    /// <param name="parent">
    /// The parent directory, or null for a detached node
    /// </param>
    protected FsInnerNode(string name, string? linkTarget = null, FsDirNode? parent = null) 
      : base(name, true, linkTarget, parent)
    {
    }

    /// <summary>
    /// A read-only view on the child nodes
    /// </summary>
    public abstract IReadOnlyList<FsTreeNode> Children { get; }
  }


}
