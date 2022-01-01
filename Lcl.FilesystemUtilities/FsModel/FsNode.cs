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
  /// Base class for a node in a virtual filesystem model tree.
  /// </summary>
  public abstract class FsNode
  {
    private AnchorNode? _anchorCached = null;
    private static readonly char[] __forbiddenCharacters = { '\\', '/', ':', '|', '<', '>', '*', '?', '\'', '\"' };

    /// <summary>
    /// Create a new FsNode, and insert it in its parent
    /// </summary>
    /// <param name="parentOrNull">
    /// The parent node, possibly null (for the root of a fragment)
    /// </param>
    /// <param name="nameOrNull">
    /// The name of this node in its parent, possibly null (in case there
    /// is no parent)
    /// </param>
    protected FsNode(
      ContainerNode? parentOrNull,
      string? nameOrNull)
    {
      UncheckedParent = parentOrNull;
      UncheckedName = nameOrNull;
      if(UncheckedName != null && !IsValidName(UncheckedName))
      {
        // Alert! null and the empty string for once are NOT equivalent!
        throw new ArgumentException(
          $"The name '{UncheckedName}' is not an acceptable node name");
      }
      if(IsAnonymous && !IsRoot)
      {
        throw new InvalidOperationException(
          "Only root nodes can be anonymous");
      }
      if(IsRoot)
      {
        // ignore this node's name if there is one!
        Path = String.Empty;
      }
      else
      {
        Path = Path + "/" + Name;
      }
      if(!IsRoot)
      {
        Parent.AddChild(this);
      }
    }

    /// <summary>
    /// The parent node, if there is one
    /// </summary>
    public ContainerNode? UncheckedParent { get; }

    /// <summary>
    /// The parent node, throwing an InvalidOperationException if there is no parent
    /// </summary>
    public ContainerNode Parent { 
      get {
        if(UncheckedParent == null)
        {
          throw new InvalidOperationException(
            "Attempt to retrieve parent of a root node");
        }
        else
        {
          return UncheckedParent;
        }
      } 
    }

    /// <summary>
    /// True if this node is a root and therefore does not have a parent
    /// and possibly no name.
    /// </summary>
    public bool IsRoot { get => UncheckedParent == null; }

    /// <summary>
    /// The name of this node, possibly null if there is no parent
    /// </summary>
    public string? UncheckedName { get; }

    /// <summary>
    /// The name of this node, throwing an InvalidOperationException if
    /// this node is anonymous
    /// </summary>
    public string Name { 
      get {
        if(String.IsNullOrEmpty(UncheckedName))
        {
          throw new InvalidOperationException(
            "Attempt to retrieve the name of an anonymous node");
        }
        else
        {
          return UncheckedName;
        }
      }
    }

    /// <summary>
    /// True if this node has no name
    /// </summary>
    public bool IsAnonymous { get => String.IsNullOrEmpty(UncheckedName); }

    /// <summary>
    /// The /-separated relative path from the anchor node to this node (an empty string for
    /// anchor nodes)
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Get the anchor (root) node for the tree fragment this node is in
    /// </summary>
    public AnchorNode Anchor {
      get {
        if(_anchorCached == null)
        {
          _anchorCached = GetAnchor();
        }
        return _anchorCached;
      }
    }

    /// <summary>
    /// Get the forest this node ultimately belongs to
    /// </summary>
    public FsForest Forest { get => Anchor.Forest; }

    /// <summary>
    /// Test if the given name is a valid node name. To be valid, a node name
    /// must not be null nor empty and not contain any of the characters &lt;&gt;:|\/'"*?
    /// </summary>
    public static bool IsValidName(string? name)
    {
      if(String.IsNullOrEmpty(name))
      {
        return false;
      }
      if(name.IndexOfAny(__forbiddenCharacters) >= 0)
      {
        return false;
      }
      return true;
    }

    /// <summary>
    /// Implements the retrieval of the Anchor property. This will be called once and cached.
    /// This default implementation returns the Anchor of the parent node, and throws
    /// an InvalidOperationException if there is no parent. Sunclasses that support a null parent
    /// must override this method
    /// </summary>
    protected virtual AnchorNode GetAnchor()
    {
      if(IsRoot)
      {
        throw new InvalidOperationException(
          "Internal Error: FsNode subclasses that support being parentless must override 'GetAnchor()'");
      }
      return Parent.GetAnchor();
    }
  }
}
