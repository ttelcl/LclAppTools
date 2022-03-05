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
  /// Represents a directory in an immutable style.
  /// </summary>
  public class PathNode
  {
    /// <summary>
    /// Create a new PathNode
    /// </summary>
    public PathNode(
      PathNode? parent,
      string name)
    {
      Parent = parent;
      if(name.IndexOfAny(new char[] { '/', '\\'}) >= 0)
      {
        throw new ArgumentException("The name must not contain any forward or backward slash");
      }
      if(parent == null)
      {
        if(name != "")  // else: acceptable Unix style root
        {
          if(name.Length != 2 || name[1] != ':' || name.ToUpper()[0]<'A' || name.ToUpper()[0]>'Z')
          {
            throw new ArgumentException(
              "The name of a root node must be either an empty string or a single letter followed by ':'");
          }
          name = name.ToUpper();  // normalize the drive letter to upper case
        }
      }
      else if(name == "")
      {
        throw new ArgumentException(
          "An empty string is only allowed as name for a root node");
      }
      else if(name.Contains(':'))
      {
        throw new ArgumentException(
          "Only a root node name may contain a ':'");
      }
      else if(name == "." || name == "..")
      {
        throw new ArgumentException(
          "The names '.' and '..' are not allowed as node names");
      }
      Name = name + "/";
      FullName = parent == null ? Name : parent.FullName + Name;
    }

    /// <summary>
    /// The parent node, or null for the root node (Unix) or a drive
    /// node (Windows)
    /// </summary>
    public PathNode? Parent { get; }

    /// <summary>
    /// True if there is no parent node
    /// </summary>
    public bool IsRoot { get => Parent == null; }

    /// <summary>
    /// The name of the node, including the trailing '/'.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The full path name (using '/' as separator)
    /// </summary>
    public string FullName { get; }

    /// <summary>
    /// Create a chain of new PathNodes from the path. The path does not need
    /// to actually exist.
    /// </summary>
    /// <param name="path">
    /// The path to break into nodes
    /// </param>
    /// <returns>
    /// The final PathNode of the created chain (representing the full path)
    /// </returns>
    public static PathNode FromPath(string path)
    {
      path = Path.GetFullPath(path).Replace('\\', '/');
      if(path.EndsWith("/"))
      {
        path = path[..^1];
      }
      return FromPathInternal(path);
    }

    /// <summary>
    /// Build a best-effort for the true path for the given path,
    /// with symbolic links and junctions resolved.
    /// </summary>
    /// <param name="path">
    /// The existing path to resolve.
    /// </param>
    /// <returns>
    /// The final PathNode of the created chain (representing the full path)
    /// </returns>
    public static PathNode TruePath(string path)
    {
      return TruePathInternal(Path.GetFullPath(path), 20);
    }

    private static PathNode TruePathInternal(string fullpath, int maxlinks)
    {
      // Past bug alert: make sure not to call GetFullPath() or di.FullPath
      // in any situation where the argument may be a drive letter without
      // trailing slash (because that would resolve to the current directory
      // on that drive instead of its root directory)
      if(maxlinks <= 0)
      {
        throw new InvalidOperationException(
          "Recursion limit exceeded");
      }
      fullpath = fullpath.Replace('\\', '/');
      var di = new DirectoryInfo(fullpath);
      if(!di.Exists)
      {
        throw new ArgumentException(
          "Expecting the argument to be an existing directory");
      }
      if(di.LinkTarget != null)
      {
        return TruePathInternal(Path.GetFullPath(di.LinkTarget), maxlinks - 1);
      }
      if(fullpath.EndsWith("/"))
      {
        fullpath = fullpath[..^1];
      }
      var idx = fullpath.LastIndexOf('/');
      if(idx < 0)
      {
        // root node
        return new PathNode(null, fullpath);
      }
      else
      {
        var parentpath = fullpath[0..idx];
        var child = fullpath[(idx+1)..];
        var parent = TruePathInternal(parentpath, maxlinks-1);
        return new PathNode(parent, child);
      }
    }

    private static PathNode TruePathInternal(string parentpath, string childname)
    {
      throw new NotImplementedException();
    }

    private static PathNode FromPathInternal(string fullpath)
    {
      var idx = fullpath.LastIndexOf('/');
      if(idx < 0)
      {
        // root node
        return new PathNode(null, fullpath);
      }
      else
      {
        var parentpath = fullpath[0..idx];
        var child = fullpath[(idx+1)..];
        var parent = FromPathInternal(parentpath);
        return new PathNode(parent, child);
      }
    }

  }
}
