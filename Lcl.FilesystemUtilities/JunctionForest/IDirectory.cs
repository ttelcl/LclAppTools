/*
 * (c) 2022  ttelcl / ttelcl
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lcl.FilesystemUtilities.JunctionForest
{
  /// <summary>
  /// A directory-like object, which may be an actual directory or
  /// a junction (reference to another directory)
  /// </summary>
  public interface IDirectory
  {
    /// <summary>
    /// The parent directory in case this is not an anchor.
    /// This is expected to be the true parent directory.
    /// </summary>
    IDirectory? Parent { get; }

    /// <summary>
    /// The anchor directory this directory is part of (the directory
    /// itself in case of anchor nodes)
    /// </summary>
    IDirectory Anchor { get; }

    /// <summary>
    /// The name of this directory, including a trailing '/'
    /// </summary>
    /// <remarks>
    /// <para>
    /// Including a trailing '/' may come as a surprise at first, but helps
    /// working around ambiguous situations for the names of root directories.
    /// For example, on a Unix system, the name of the root would otherwise be
    /// an empty string. And on Windows it would be just the drive indicator
    /// (e.g. "C:"), which by default is interpreted as "the current directory
    /// on that drive" instead of the intended "the root of the drive"
    /// </para>
    /// </remarks>
    string Name { get; }

    /// <summary>
    /// The forest this directory resides in
    /// </summary>
    DirectoryForest Forest { get; }

    /// <summary>
    /// The target directory in case this is a junction (or link)
    /// </summary>
    IDirectory? LinkTarget { get; }

    /// <summary>
    /// A mapping from child name to child directory. Note that
    /// in the case this is a junction, the child's parent may be
    /// distinct from this!
    /// </summary>
    IReadOnlyDictionary<string, IDirectory> Children { get; }

    /// <summary>
    /// Create a new child node that has this node as parent.
    /// Fails if a child by that name already exists.
    /// </summary>
    IDirectory InsertTrueChild(string name);

    /// <summary>
    /// Create a new child node that links to another existing directory
    /// </summary>
    /// <param name="name">
    /// The name of the child in this directory
    /// </param>
    /// <param name="target">
    /// The existing directory node (in the same forest)
    /// </param>
    /// <returns>
    /// The new link node
    /// </returns>
    IDirectory InsertLinkChild(string name, IDirectory target);
  }
}


