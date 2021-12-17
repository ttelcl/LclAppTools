/*
 * (c) 2021  ttelcl / ttelcl
 */

namespace Lcl.FilesystemUtilities.Modeling
{
  /// <summary>
  /// And FsTreeNode representing a actual directory rather than a file
  /// or link
  /// </summary>
  public class FsDirNode: FsInnerNode
  {
    private readonly List<FsTreeNode> _children;

    /// <summary>
    /// Create an FsDirNode
    /// </summary>
    /// <param name="name">
    /// The directory name (without path)
    /// </param>
    /// <param name="parent">
    /// The parent directory (or null for a detached directory)
    /// </param>
    /// <param name="children">
    /// If not null: a list of child nodes
    /// </param>
    public FsDirNode(
      string name,
      IEnumerable<FsTreeNode>? children = null,
      FsDirNode? parent = null)
      : base(name, null, parent)
    {
      _children = new List<FsTreeNode>();
      if(children != null)
      {
        _children.AddRange(children);
      }
      Children = _children.AsReadOnly();
    }

    /// <summary>
    /// A read-only view on the child nodes
    /// </summary>
    public override IReadOnlyList<FsTreeNode> Children { get; }

    /// <summary>
    /// Notification that a child wants to add itself to this directory
    /// </summary>
    internal void AddChild(FsTreeNode child)
    {
      _children.Add(child);
    }

    /// <summary>
    /// Notification that a child wants to remove itself to this directory
    /// </summary>
    internal void RemoveChild(FsTreeNode child)
    {
      if(!_children.Remove(child))
      {
        throw new InvalidOperationException(
          $"Attempt to remove a child from a directory node where it wasn't present");
      }
    }

  }


}
