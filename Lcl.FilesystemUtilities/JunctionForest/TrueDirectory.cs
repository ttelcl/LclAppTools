/*
 * (c) 2022  ttelcl / ttelcl
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lcl.FilesystemUtilities.JunctionForest
{
  /// <summary>
  /// An IDirectory representing an actual directory (and not a junction)
  /// </summary>
  public abstract class TrueDirectoryBase: IDirectory
  {
    private readonly Dictionary<string, IDirectory> _children;

    /// <summary>
    /// Create a new TrueDirectory
    /// </summary>
    internal TrueDirectoryBase(
      DirectoryForest owner,
      string name,
      IDirectory? parent)
    {
      Forest = owner;
      Name = name;
      Parent = parent;
      _children = new Dictionary<string, IDirectory>(Forest.NameComparer);
      Children = _children; 
    }

    /// <summary>
    /// The parent node, potentially null
    /// </summary>
    public IDirectory? Parent { get; }

    /// <summary>
    /// The anchor node (to be implemented by subclasses)
    /// </summary>
    public abstract IDirectory Anchor { get; }

    /// <summary>
    /// The directory's short name, including a trailing '/'
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The model this directory is part of
    /// </summary>
    public DirectoryForest Forest { get; }

    /// <summary>
    /// The link target. For this implementation this is always null.
    /// </summary>
    public IDirectory? LinkTarget { get => null; }

    /// <summary>
    /// A read-only view on the child directories
    /// </summary>
    public IReadOnlyDictionary<string, IDirectory> Children { get; }

    /// <summary>
    /// Add a child directory that is a junction (link) to another
    /// directory inside the same forest
    /// </summary>
    public IDirectory InsertLinkChild(string name, IDirectory target)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Add a child directory that is a TrueDirectory
    /// </summary>
    public IDirectory InsertTrueChild(string name)
    {
      if(_children.ContainsKey(name))
      {
        throw new InvalidOperationException(
          $"The directory already conatons a child named '{name}'");
      }
      var child = Forest.CreateTrueDirectory(name, this);
      _children[name] = child;
      return child;
    }
  }

  /// <summary>
  /// A True directory that is not anchor
  /// </summary>
  public class TrueDirectory : TrueDirectoryBase
  {
    /// <summary>
    /// Create a true directory node (that is neither a link nor an anchor)
    /// </summary>
    /// <param name="name">
    /// The name of the node
    /// </param>
    /// <param name="parent">
    /// The parent of the directory. For this class this can not be null!
    /// </param>
    public TrueDirectory(
      string name,
      IDirectory parent)
      : base(parent.Forest, TrueDirectoryName(name), parent)
    {
      if(parent == null)
      {
        throw new ArgumentNullException(nameof(parent));
      }
      Anchor = parent.Anchor;
    }

    /// <summary>
    /// The anchor node
    /// </summary>
    public override IDirectory Anchor { get; }

    /// <summary>
    /// Check and patch the name so it ends with a '/' if it didn't
    /// </summary>
    public static string TrueDirectoryName(string name)
    {
      if(name.EndsWith("/")) // strip off a trailing '/' since that makes checking easier
      {
        name = name[..^1];
      }
      if(name == "")
      {
        throw new InvalidOperationException("A directory name cannot be empty");
      }
      if(name == "." || name == "..")
      {
        throw new InvalidOperationException("A directory name cannot be '.' or '..'");
      }
      if(name.IndexOfAny(new[] { '/', '\\', ':', '?', '*', '<', '>'}) >= 0)
      {
        throw new InvalidOperationException("A directory name cannot contain any of the characters '/\\:?*<>'");
      }
      return name + "/";
    }
  }

}
