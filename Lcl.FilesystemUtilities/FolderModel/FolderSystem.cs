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
  /// A group of related folder trees, sharing naming rules, and allowing links
  /// between trees.
  /// </summary>
  public class FolderSystem
  {
    private readonly Dictionary<string, FolderItem> _anchors;

    /// <summary>
    /// Create a new FolderSystem
    /// </summary>
    public FolderSystem(bool caseSensitive)
    {
      CaseSensitive = caseSensitive;
      NameComparer = CaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
      _anchors = new Dictionary<string, FolderItem>(NameComparer);
      Anchors = _anchors;
    }

    /// <summary>
    /// Whether or not name comparisons are case sensitive
    /// </summary>
    public bool CaseSensitive { get; }

    /// <summary>
    /// The name comparer (as determined by CaseSensitive)
    /// </summary>
    public StringComparer NameComparer { get; }

    /// <summary>
    /// The anchors: the root folders and / or links acting as roots
    /// </summary>
    public IReadOnlyDictionary<string, FolderItem> Anchors { get; }

    /// <summary>
    /// Add a new anchor folder.
    /// Anchor folders are nameless, but have a label that identifies them.
    /// </summary>
    /// <param name="label">
    /// The label for the anchor
    /// </param>
    /// <returns>
    /// The new (nameless) anchor folder
    /// </returns>
    public FolderContainer AddAnchorFolder(string label)
    {
      if(_anchors.ContainsKey(label))
      {
        throw new InvalidOperationException(
          $"Duplicate anchor label: {label}");
      }
      var anchor = new FolderContainer(this);
      _anchors[label] = anchor;
      return anchor;
    }

    /// <summary>
    /// Add a new anchor link. 
    /// Anchor links are nameless, but have a label that identifies them.
    /// </summary>
    /// <param name="label">
    /// The label for the anchor
    /// </param>
    /// <param name="target">
    /// The target for the link
    /// </param>
    /// <returns>
    /// The new anchor link
    /// </returns>
    public FolderLink AddAnchorLink(string label, string target)
    {
      if(_anchors.ContainsKey(label))
      {
        throw new InvalidOperationException(
          $"Duplicate anchor label: {label}");
      }
      var anchor = new FolderLink(this, target);
      _anchors[label] = anchor;
      return anchor;
    }

  }
}
