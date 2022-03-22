/*
 * (c) 2022  ttelcl / ttelcl
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lcl.FilesystemUtilities.FolderModel
{
  /// <summary>
  /// A FolderItem that may contain other FolderItems, indexable by name
  /// </summary>
  public class FolderContainer: FolderItem
  {
    private readonly Dictionary<string, FolderItem> _children;

    /// <summary>
    /// Create a new non-root FolderContainer
    /// </summary>
    internal FolderContainer(FolderContainer parent, string name)
      : base(parent.Host, parent, name)
    {
      _children = new Dictionary<string, FolderItem>(Host.NameComparer);
      Children = _children;
    }

    /// <summary>
    /// Create a new root FolderContainer (nameless)
    /// </summary>
    /// <param name="host">
    /// The host FolderSystem this root is part of
    /// </param>
    internal FolderContainer(FolderSystem host)
      : base(host, null, "")
    {
      _children = new Dictionary<string, FolderItem>(Host.NameComparer);
      Children = _children;
    }

    /// <summary>
    /// The child items (read-only)
    /// </summary>
    public IReadOnlyDictionary<string, FolderItem> Children { get; }

    /// <summary>
    /// Add a new child representing an actual directory (not a link)
    /// </summary>
    /// <param name="name">
    /// The child name
    /// </param>
    /// <returns>
    /// The newly created child
    /// </returns>
    public FolderContainer AddChildFolder(string name)
    {
      var child = new FolderContainer(this, name); // this will also invoke this.RegisterChild()
      return child;
    }

    /// <summary>
    /// Add a new child representing a link to another FolderItem
    /// </summary>
    /// <param name="name">
    /// The child name
    /// </param>
    /// <param name="target">
    /// The target name
    /// </param>
    /// <returns>
    /// The newly created child
    /// </returns>
    public FolderLink AddChildLink(string name, string target)
    {
      var child = new FolderLink(this, name, target); // this will also invoke this.RegisterChild()
      return child;
    }

    /// <summary>
    /// Callback from the child creation process to insert it into this container
    /// </summary>
    /// <param name="child">
    /// The child item to insert
    /// </param>
    internal void RegisterChild(FolderItem child)
    {
      if(child.Parent != this)
      {
        throw new ArgumentException(
          $"Incorrect parent for the given child");
      }
      if(_children.ContainsKey(child.Name))
      {
        throw new InvalidOperationException(
          $"Duplicate child name: {child.Name}");
      }
      _children.Add(child.Name, child);
    }

  }
}
