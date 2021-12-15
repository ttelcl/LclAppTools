/*
 * (c) 2021  ttelcl / ttelcl
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lcl.FilesystemUtilities.Modeling
{
  /// <summary>
  /// A node in a file system tree (a directory, file or symbolic link)
  /// </summary>
  public class FsTreeNode
  {
    /// <summary>
    /// Create a new FsTreeNode
    /// </summary>
    public FsTreeNode()
    {
      throw new NotImplementedException();
    }

    public static FsTreeNode FromFileSystemInfo(FileSystemInfo fsi)
    {
      if(fsi.Attributes.HasFlag(FileAttributes.ReparsePoint))
      {
        // TBD
      }
      throw new NotImplementedException();
    }

  }
}
