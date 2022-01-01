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
  /// A concrete ContainerNode that is not a root and has a name
  /// </summary>
  public class DirectoryNode: ContainerNode
  {
    /// <summary>
    /// Create a new DirectoryNode (normally invoked via ContainerNode.AddDirectory())
    /// </summary>
    /// <param name="parent">
    /// The parent node for this directory node (not nullable!)
    /// </param>
    /// <param name="name">
    /// The name of the directory (not nullable)
    /// </param>
    internal DirectoryNode(ContainerNode parent, string name)
      : base(parent, name)
    {
    }

  }
}
