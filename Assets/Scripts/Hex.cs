// -----------------------------------------------
// Filename: Hex.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System.Collections;
using UnityEngine;

public class Hex : MonoBehaviour {
   #region enums

   public enum HexDirections {
      UpperRight = 0,
      Right = 1,
      LowerRight = 2,
      LowerLeft = 3,
      Left = 4,
      UpperLeft = 5
   }

   #endregion enums

   #region properties

   public Hex[] Neighbours {
      get { return _neighbours; }
   }

   #endregion properties

   #region fields

   /// <summary>
   /// Neigbours are organised according to Hex.HexDirections
   /// </summary>
   private Hex[] _neighbours;

   #endregion fields

   #region methods

   #region unity methods

   private void OnEnable() {
      _neighbours = new Hex[6];
   }

   #endregion unity methods

   #region static methods

   public static HexDirections OppositeDirection(HexDirections direction) {
      return (HexDirections)Mathf.Abs((int)direction - 3);
   }

   public static HexDirections VerticalMirrorDirection(HexDirections direction) {
      switch (direction) {
         case HexDirections.UpperRight:
            return HexDirections.LowerRight;

         case HexDirections.LowerRight:
            return HexDirections.UpperRight;

         case HexDirections.LowerLeft:
            return HexDirections.UpperLeft;

         case HexDirections.UpperLeft:
            return HexDirections.LowerLeft;
      }

      return direction;
   }

   #endregion static methods

   public void SetMaterial(Material material) {
      GetComponent<Renderer>().material = material;
   }

   #endregion methods
}
