// -----------------------------------------------
// Filename: DepthTexture.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using UnityEngine;

namespace Utils {
   [RequireComponent(typeof(Camera))]
   public class DepthTexture : MonoBehaviour {
      #region fields

      public DepthTextureMode _mode;

      #endregion fields

      #region methods

      #region unity methods

      private void Start() {
         GetComponent<Camera>().depthTextureMode |= _mode;
      }

      #endregion unity methods

      #endregion methods
   }
}
