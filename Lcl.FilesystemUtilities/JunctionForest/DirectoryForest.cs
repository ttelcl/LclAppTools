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
  /// A collection root directories
  /// </summary>
  public class DirectoryForest
  {
    private Dictionary<string, IAnchor> _anchors;

    /// <summary>
    /// Create a new empty DirectoryForest
    /// </summary>
    /// <param name="caseSensitive">
    /// True if directory names in this model are case sensitive
    /// </param>
    public DirectoryForest(bool caseSensitive)
    {
      CaseSensitive = caseSensitive;
      NameComparer = CaseSensitive ? StringComparer.InvariantCulture : StringComparer.InvariantCultureIgnoreCase;
      _anchors = new Dictionary<string, IAnchor>(NameComparer);
    }

    /// <summary>
    /// True if directory names in this model are case sensitive
    /// </summary>
    public bool CaseSensitive { get; }

    /// <summary>
    /// The string comparer matching the CaseSensitive flag
    /// </summary>
    public StringComparer NameComparer { get; }

    /// <summary>
    /// Register a path as being a valid anchor
    /// </summary>
    public IAnchor RegisterAnchor(string anchorpath, string? id = null)
    {
      var cmp = CaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
      var anchor = CreateTrueAnchor(anchorpath, id);
      if(_anchors.ContainsKey(anchor.Name))
      {
        throw new InvalidOperationException(
          $"The anchor path '{anchor.Name}' was already in use");
      }
      foreach(var oldanchor in _anchors.Values)
      {
        if(oldanchor.Name.StartsWith(anchor.Name, cmp) || anchor.Name.StartsWith(oldanchor.Name, cmp))
        {
          throw new InvalidOperationException(
            $"Cannot add '{anchor.Name}' as anchor directory because it conflicts with '{oldanchor.Name}'");
        }
      }
      _anchors[anchor.Name] = anchor;
      return anchor;
    }

    /// <summary>
    /// Try to split a path in an anchor part and a tail relative to that anchor
    /// </summary>
    /// <param name="path">
    /// The path to split. This should be an absolute path
    /// </param>
    /// <param name="anchor">
    /// The anchor that was matched on success (or null on failure)
    /// </param>
    /// <param name="tail">
    /// The remaining part of the path after taking off the anchor (or null on failure)
    /// </param>
    /// <returns>
    /// True if an anchor was successfully matched, false on failure
    /// </returns>
    public bool TryMatchAnchor(string path, out IDirectory? anchor, out string? tail)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Create a new True Directory node (not an anchor and not a link)
    /// </summary>
    protected internal virtual TrueDirectory CreateTrueDirectory(
      string name,
      IDirectory parent)
    {
      if(!Object.ReferenceEquals(parent.Forest, this))
      {
        throw new ArgumentException(
          $"Unexpected DirectoryForest instance");
      }
      return new TrueDirectory(name, parent);
    }

    /// <summary>
    /// Create a new true anchor node (not a link)
    /// </summary>
    protected internal virtual TrueAnchor CreateTrueAnchor(
      string name,
      string? id = null)
    {
      return new TrueAnchor(this, name, id);
    }

  }
}
