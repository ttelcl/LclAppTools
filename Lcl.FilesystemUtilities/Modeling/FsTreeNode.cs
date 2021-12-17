/*
 * (c) 2021  ttelcl / ttelcl
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lcl.FilesystemUtilities.Modeling
{

  /// <summary>
  /// A node in a file system tree (a directory, file or symbolic link).
  /// This may refer to a node that does not yet exist
  /// </summary>
  public abstract class FsTreeNode
  {
    /// <summary>
    /// Create a new FsTreeNode
    /// </summary>
    protected FsTreeNode(
      string name,
      bool isDir,
      string? linkTarget = null,
      FsDirNode? parent = null)
    {
      Name = name;
      IsDir = isDir;
      LinkTarget = linkTarget;
      Parent = parent;
    }

    public static FsTreeNode FromFileSystemInfo(FileSystemInfo fsi)
    {
      if(fsi.Attributes.HasFlag(FileAttributes.ReparsePoint))
      {
        // TBD
      }
      throw new NotImplementedException();
    }

    /// <summary>
    /// The (short) name of the node
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Whether this node has directory or file semantics
    /// </summary>
    public bool IsDir { get; }

    /// <summary>
    /// The destination path to the target if this is a link.
    /// Null if this is not a link
    /// </summary>
    public string? LinkTarget { get; }

    /// <summary>
    /// True if this node is a link instead of an actual file or directory
    /// </summary>
    public bool IsLink { get => LinkTarget != null; }

    /// <summary>
    /// The parent node, if this is not a detached node. This can be set via
    /// the ChangeParent() method.
    /// </summary>
    public FsDirNode? Parent { get; private set; }

    /// <summary>
    /// True if this is a detached node (Parent == null)
    /// </summary>
    public bool IsDetached { get => Parent == null; }

    /// <summary>
    /// Detach this node from its current parent, then attach it to the new parent
    /// </summary>
    /// <param name="newParent">
    /// The new parent node, or null to only detach this node from its current parent
    /// </param>
    public void ChangeParent(FsDirNode? newParent)
    {
      if(Parent != null)
      {
        Parent.RemoveChild(this);
        Parent = null;
      }
      if(newParent != null)
      {
        newParent.AddChild(this);
        Parent = newParent;
      }
    }

  }


}
