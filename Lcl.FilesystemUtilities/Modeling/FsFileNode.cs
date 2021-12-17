/*
 * (c) 2021  ttelcl / ttelcl
 */

namespace Lcl.FilesystemUtilities.Modeling
{
  /// <summary>
  /// An FsTreeNode representing an File rather than a directory or a link
  /// </summary>
  public class FsFileNode: FsTreeNode
  {
    /// <summary>
    /// Create an FsFileNode
    /// </summary>
    /// <param name="name">
    /// The name of the file (without path)
    /// </param>
    /// <param name="parent">
    /// The parent directory (or null for a detached file)
    /// </param>
    public FsFileNode(string name, FsDirNode? parent = null) 
      : base(name, false, null, parent)
    {
    }
  }


}
