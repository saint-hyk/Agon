// -----------------------------------------------
// Filename: GamePiece.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using UnityEngine;

namespace HexBoard {
   public class GamePiece : MonoBehaviour {
      #region methods

      virtual public void Initialize() {
      }

      virtual public void SetMaterial(Material material) {
         GetComponent<Renderer>().material = material;
      }

      #endregion methods
   }
}
