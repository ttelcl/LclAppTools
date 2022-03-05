/*
 * (c) 2022  ttelcl / ttelcl
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lcl.FilesystemUtilities.JunctionForest
{
  /// <summary>
  /// Additional information on anchor directories
  /// </summary>
  public interface IAnchor: IDirectory
  {
    /// <summary>
    /// A logical identifier for this anchor
    /// </summary>
    string Id { get; }
  }
}

