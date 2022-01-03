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
  /// A collection of FsNode tree fragments (AnchorNode instances)
  /// </summary>
  public class FsForest
  {
    private readonly Dictionary<string, AnchorNode> _anchors;

    /// <summary>
    /// Create a new empty FsForest
    /// </summary>
    public FsForest()
    {
      _anchors = new Dictionary<string, AnchorNode>(StringComparer.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Get a read-only view on the anchors in this collection
    /// </summary>
    public IReadOnlyCollection<AnchorNode> Anchors { get => _anchors.Values; }

    /// <summary>
    /// Retrieve an existing anchor or create a new one
    /// </summary>
    /// <param name="label">
    /// The identifying label for the anchor. Before searching for this label, this
    /// string will be normalized
    /// </param>
    /// <returns>
    /// The existing or new anchor
    /// </returns>
    public AnchorNode GetAnchor(string label)
    {
      label = AnchorNode.NormalizeLabel(label);
      if(!_anchors.TryGetValue(label, out var anchor))
      {
        anchor = new AnchorNode(this, label);
        _anchors[anchor.Label] = anchor;
      }
      return anchor;
    }

    /// <summary>
    /// Attempts to find an anchor that matches the given path
    /// </summary>
    /// <param name="path">
    /// The path to match
    /// </param>
    /// <param name="anchor">
    /// On success: the anchor that was found
    /// </param>
    /// <param name="tailPath">
    /// On success: the rest of the path after the matched anchor
    /// </param>
    /// <returns>
    /// True on success, false on failure
    /// </returns>
    public bool TryMatchAnchor(string path, out AnchorNode? anchor, out string? tailPath)
    {
      path = AnchorNode.NormalizeLabel(path);
      var i = path.Length;
      // First see if there is an exact match, only start splitting if there isn't
      if(_anchors.TryGetValue(path, out anchor))
      {
        tailPath = String.Empty;
        return true;
      }
      while(i > 0)
      {
        i = path.LastIndexOf('/', i-1);
        if(i < 0)
        {
          break;
        }
        var tailPath2 = path.Substring(i+1);
        var anchorPath = path.Substring(0, i);
        if(_anchors.TryGetValue(anchorPath, out anchor))
        {
          tailPath = tailPath2;
          return true;
        }
      }
      anchor = null;
      tailPath = null;
      return false;
    }
  }
}
