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
  /// An anonymous and parentless ContainerNode, acting as the root for
  /// a filesysytem tree fragment
  /// </summary>
  public class AnchorNode: ContainerNode
  {
    /// <summary>
    /// Create a new AnchorNode
    /// </summary>
    /// <param name="owner">
    /// The FsForest instance this anchor belongs to
    /// </param>
    /// <param name="label">
    /// The absolute path corresponding to this anchor directory if it
    /// represents an actual directory. Or an abstract name that enables
    /// refering to it. Backslashes will be translated to forward slashes,
    /// if there is a trailing slash it is removed.
    /// </param>
    internal AnchorNode(
      FsForest owner,
      string label)
      : base(null, null)
    {
      Owner = owner;
      Label = NormalizeLabel(label);
      LabelTag = $"<{Label}>";
    }

    /// <summary>
    /// Normalize an anchor label: replace any '\' by '/' and remove a trailing
    /// '/' if it is present
    /// </summary>
    public static string NormalizeLabel(string label)
    {
      label = label.Replace('\\', '/');
      if(label.EndsWith("/"))
      {
        label = label[0..^1];
      }
      return label;
    }

    /// <summary>
    /// The owning FsForest instance
    /// </summary>
    public FsForest Owner { get; }

    /// <summary>
    /// The label for this anchor: if it represents an actual directory then
    /// it is its absolute path, translated to use forward slashes and without
    /// trailing slash.
    /// </summary>
    public string Label { get; }

    /// <summary>
    /// The serialization tag used to identify this Anchor node: the label enclosed
    /// in angle brackets (&lt; ... &gt;)
    /// </summary>
    public string LabelTag { get; }

    /// <summary>
    /// Override the default anchor retrieval
    /// </summary>
    protected override AnchorNode GetAnchor()
    {
      return this;
    }
  }
}
