/*
 * (c) 2021  ttelcl / ttelcl
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lcl.FilesystemUtilities
{
  /// <summary>
  /// Static helper class to have symbolic link functionality all together
  /// </summary>
  public static class SymbolicLink
  {
    /// <summary>
    /// Returns true if the argument exists and is a symbolic link to a directory or file
    /// </summary>
    public static bool IsLink(this FileSystemInfo fsi)
    {
      return fsi.Exists && fsi.Attributes.HasFlag(FileAttributes.ReparsePoint);
    }

    /// <summary>
    /// Create a new symbolic link to this target directory, and return the new link as 
    /// a new DirectoryInfo object
    /// </summary>
    public static DirectoryInfo CreateSymbolicLinkAs(this DirectoryInfo target, string linkPath)
    {
      return (DirectoryInfo)Directory.CreateSymbolicLink(linkPath, target.FullName);
    }

    /// <summary>
    /// Create a new symbolic link to this target file, and return the new link as 
    /// a new FileInfo object
    /// </summary>
    public static FileInfo CreateSymbolicLinkAs(this FileInfo target, string linkPath)
    {
      return (FileInfo)File.CreateSymbolicLink(linkPath, target.FullName);
    }

    /// <summary>
    /// Return the target of a symbolic file or directory link, or null if the
    /// argument is not a symbolic link.
    /// </summary>
    /// <param name="fsi">
    /// The symbolic link DirectoryInfo or FileInfo to get the target of
    /// </param>
    public static string? Target(FileSystemInfo fsi)
    {
      return fsi.LinkTarget;
    }

  }
}
