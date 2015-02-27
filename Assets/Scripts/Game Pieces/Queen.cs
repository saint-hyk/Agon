// -----------------------------------------------
// Filename: Queen.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using Geometry;
using HexBoard;

namespace Agon {
   public class Queen : GamePiece {
      override public void Initialize() {
         GetComponent<UniformPolygonDrawer>().Initialize();
      }
   }
}
