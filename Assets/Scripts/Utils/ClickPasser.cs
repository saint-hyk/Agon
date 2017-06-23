// -----------------------------------------------
// Filename: ClickPasser.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System;
using UnityEngine;

namespace Utils {
   [RequireComponent(typeof(Collider))]
   public class ClickPasser : MonoBehaviour {

      #region events

      public event EventHandler Clicked;

      #endregion events

      #region methods

      #region unity methods

      private void OnMouseDown() {
         EventHandler handler = Clicked;

         if (handler != null) {
            handler(this, null);
         }
      }

      #endregion unity methods

      #endregion methods
   }
}
