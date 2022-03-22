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
  /// Common ancestor for items in our simplified directory tree model:
  /// anchor folders, true folders and junctions
  /// </summary>
  public abstract class FolderItem
  {
    /// <summary>
    /// Create a new FolderItem and insert it in its parent if it has one
    /// </summary>
    protected FolderItem(FolderSystem host, FolderContainer? parent, string name)
    {
      Host = host;
      Parent = parent;
      Name = name;
      if(Parent != null)
      {
        if(String.IsNullOrEmpty(name))
        {
          throw new ArgumentException(
            $"Expecting a non-blank name for child folders");
        }
        Parent.RegisterChild(this);
      }
      else
      {
        if(!String.IsNullOrEmpty(name))
        {
          throw new ArgumentException(
            $"Expecting a blank name for anchor folders");
        }
      }
    }

    /// <summary>
    /// The parent folder, or null if there is none
    /// </summary>
    public FolderContainer? Parent { get; }

    /// <summary>
    /// The name of the item in its parent. For root (anchor) folders this is 
    /// an empty string
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The FolderSystem instance this is part of
    /// </summary>
    public FolderSystem Host { get; }

  }
}
