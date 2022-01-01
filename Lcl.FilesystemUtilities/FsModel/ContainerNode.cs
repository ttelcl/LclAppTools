/*
 * (c) 2022  ttelcl / ttelcl
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lcl.FilesystemUtilities.FsModel
{
  /// <summary>
  /// An FsNode that can contain other FsNodes, behaving like a directory.
  /// </summary>
  public abstract class ContainerNode: FsNode
  {
    private readonly Dictionary<string, FsNode> _children;

    /// <summary>
    /// Create a new ContainerNode
    /// </summary>
    public ContainerNode(ContainerNode? parent, string? name)
      : base(parent, name)
    {
      _children = new Dictionary<string, FsNode>(StringComparer.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Locates a trail of directories for the give '/' separated path,
    /// creating missing ones and returning the final one
    /// </summary>
    /// <param name="path">
    /// A '/' separated path containing one or more directory segments to create
    /// or locate
    /// </param>
    /// <returns>
    /// The final directory node in the trail
    /// </returns>
    public DirectoryNode MakeDirectory(string path)
    {
      if(String.IsNullOrEmpty(path))
      {
        throw new ArgumentException(
          "The path to locate must not be empty");
      }
      if(path.IndexOf('\\') >= 0)
      {
        throw new ArgumentException(
          "Expecting a '/' separated path; the character '\\' is not allowed in the path");
      }
      var parts = path.Split('/', 2);
      var dn = FindDirectoryChild(parts[0]);
      if(dn == null)
      {
        dn = AddDirectory(parts[0]);
      }
      if(parts.Length == 1 || String.IsNullOrEmpty(parts[1]))
      {
        return dn;
      }
      else
      {
        return dn.MakeDirectory(parts[1]);
      }
    }

    /// <summary>
    /// Return the child node with the specified name and verify that it is a 
    /// directory node. Returns null if the child is not found. Throws an
    /// exception if the node exists but is not a directory
    /// </summary>
    public DirectoryNode? FindDirectoryChild(string name)
    {
      if(_children.TryGetValue(name, out var node))
      {
        if(node is DirectoryNode dn)
        {
          return dn;
        }
        else
        {
          throw new InvalidOperationException(
            $"The node named '{name}' is not a Directory");
        }
      }
      else
      {
        return null;
      }
    }
    
    /// <summary>
    /// Create a new DirectoryNode as child of this ContainerNode
    /// </summary>
    internal DirectoryNode AddDirectory(string name)
    {
      return new DirectoryNode(this, name);
    }

    /// <summary>
    /// Callback from FsNode constructor
    /// </summary>
    internal void AddChild(FsNode child)
    {
      if(child.UncheckedParent != this)
      {
        throw new InvalidOperationException(
          "Attempt to insert a child node in the wrong container");
      }
      if(child.IsAnonymous)
      {
        throw new InvalidOperationException(
          "Cannot insert anonymous nodes in a parent");
      }
      if(_children.ContainsKey(child.Name))
      {
        if(Object.ReferenceEquals(_children[child.Name], this))
        {
          // ignore, it was present already
        }
        else
        {
          throw new InvalidOperationException(
            $"Duplicate child name: '{child.Name}'");
        }
      }
      else
      {
        _children[child.Name] = child;
      }
    }



  }
}
