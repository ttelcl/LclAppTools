/*
 * (c) 2021  ttelcl / ttelcl
 */

namespace Lcl.FilesystemUtilities.Modeling
{
  /// <summary>
  /// An FsTreeNode representing a link to a file
  /// </summary>
  public class FsFileLink: FsTreeNode
  {
    /// <summary>
    /// Create an FsFileNode
    /// </summary>
    /// <param name="name">
    /// The name of the file (without path)
    /// </param>
    /// <param name="linkTarget">
    /// The target file this link points to
    /// </param>
    /// <param name="parent">
    /// The parent directory (or null for a detached file link)
    /// </param>
    public FsFileLink(string name, string linkTarget, FsDirNode? parent = null)
      : base(name, false, linkTarget, parent)
    {
    }

    /// <summary>
    /// The node this link resolves to, or null if not yet resolved
    /// </summary>
    public FsFileNode? ResolvedLink { get; private set; }
  }


}
