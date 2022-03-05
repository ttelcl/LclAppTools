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
  /// An anchor directory that itself is not a link
  /// </summary>
  public class TrueAnchor: TrueDirectoryBase, IDirectory, IAnchor
  {
    /// <summary>
    /// Create a new TrueAnchor
    /// </summary>
    /// <param name="owner">
    /// The forest this anchor will belong to
    /// </param>
    /// <param name="name">
    /// The path of the anchor. Any backslashes will be replaced by forward slashes,
    /// and a trailing slash will be appended if not already present.
    /// </param>
    /// <param name="id">
    /// The ID for the anchor. If null, an ID will be generated from the name.
    /// </param>
    public TrueAnchor(
      DirectoryForest owner,
      string name,
      string? id = null)
      : base(owner, AnchorName(name), null)
    {
      if(String.IsNullOrEmpty(id))
      {
        Id = $"<{Name[..^1]}>";
      }
      else
      {
        Id = id;
      }
    }

    /// <summary>
    /// The anchor ID
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Returns this node itself
    /// </summary>
    public override IDirectory Anchor { get => this; }

    /// <summary>
    /// Basic checks on anchor name; appends '/' if necessary
    /// and replaces '\' by '/'. Unlike TrueDirectory, this check
    /// is much more forgiving
    /// </summary>
    public static string AnchorName(string name)
    {
      name = name.Replace("\\", "/");
      if(!name.EndsWith("/"))
      {
        name = name + "/";
      }
      return name;
    }
  }
}
