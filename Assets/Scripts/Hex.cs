// -----------------------------------------------
// Filename: Hex.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using Geometry;
using System;
using UnityEngine;

namespace HexBoard {
   [RequireComponent(typeof(UniformPolygonDrawer))]
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

      #region events

      public event EventHandler HexClicked;

      #endregion events

      #region properties

      public Hex[] Neighbours {
         get { return _neighbours; }
      }

      public GamePiece CurrentPiece {
         get { return _currentPiece; }
         set { _currentPiece = value; }
      }

      public bool Selected {
         set {
            //TODO: prettyshader.visible = value
         }
      }

      #endregion properties

      #region fields

      /// <summary>
      /// Neigbours are organised according to Hex.HexDirections
      /// </summary>
      private Hex[] _neighbours;

      private UniformPolygonDrawer _geometry;

      [SerializeField]
      private GamePiece _currentPiece;

      #endregion fields

      #region methods

      #region unity methods

      private void Awake() {
         _geometry = GetComponent<UniformPolygonDrawer>();
      }

      private void OnEnable() {
         _neighbours = new Hex[6];

         _geometry.Initialize();

         CreateCollider();
      }

      private void OnMouseDown() {
         EventHandler handler = HexClicked;

         if (handler != null) {
            handler(this, null);
         }
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
         _geometry.SetMaterial(material);
      }

      public void ClearPiece() {
         if (_currentPiece) {
            GameObject.Destroy(_currentPiece);
         }
      }

      private void CreateCollider() {
         gameObject.AddComponent<MeshCollider>();
      }

      #endregion methods
   }
}
