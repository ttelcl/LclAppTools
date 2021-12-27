/*
 * (c) 2021  ttelcl / ttelcl
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// a new DirectoryInfo object. Throws a Win32Exception in case something goes wrong
    /// in the underlying system call.
    /// </summary>
    /// <exception cref="Win32Exception">
    /// Thrown if the underlying system call fails, most likely because the user does
    /// not have the required permission. On a default Windows setup this call
    /// requires admin privileges.
    /// </exception>
    public static DirectoryInfo CreateSymbolicLinkAs(this DirectoryInfo target, string linkPath)
    {
      var di = (DirectoryInfo)Directory.CreateSymbolicLink(linkPath, target.FullName);
      // Why, oh why, isn't the next bit included in the CreateSymbolicLink call???
      var e = Marshal.GetLastPInvokeError();
      if(e != 0)
      {
        throw new Win32Exception(e);
      }
      return di;
    }

    /// <summary>
    /// Create a new junction to this target directory, and return the new link as 
    /// a new DirectoryInfo object. Junctions are a Windows NTFS specific feature
    /// </summary>
    public static DirectoryInfo CreateJunctionAs(this DirectoryInfo target, string junctionPath)
    {
      if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        throw new PlatformNotSupportedException("Junctions are specific to Windows");
      }

      JunctionPoint.Create(Path.GetFullPath(junctionPath), target.FullName, false);
      target.Refresh();
      return target;
    }

    /// <summary>
    /// Create a new symbolic link to this target file, and return the new link as 
    /// a new FileInfo object
    /// </summary>
    public static FileInfo CreateSymbolicLinkAs(this FileInfo target, string linkPath)
    {
      var fi = (FileInfo)File.CreateSymbolicLink(linkPath, target.FullName);
      // Why, oh why, isn't the next bit included in the CreateSymbolicLink call???
      var e = Marshal.GetLastPInvokeError();
      if(e != 0)
      {
        throw new Win32Exception(e);
      }
      return fi;
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
