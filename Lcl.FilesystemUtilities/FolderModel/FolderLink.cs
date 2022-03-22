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
  /// A FolderItem that is a link to another folder (identified
  /// by target name)
  /// </summary>
  public class FolderLink: FolderItem
  {
    /// <summary>
    /// Create a new FolderLink that resides in a FolderContainer
    /// </summary>
    internal FolderLink(FolderContainer parent, string name, string target)
      : base(parent.Host, parent, name)
    {
      Target = target;
    }

    /// <summary>
    /// Create a root FolderLink (for example to represent a SUBST drive). Nameless
    /// </summary>
    internal FolderLink(FolderSystem host, string target)
      : base(host, null, "")
    {
      Target = target;
    }

    /// <summary>
    /// The name of the target the link points to
    /// </summary>
    public string Target { get; }

  }
}
