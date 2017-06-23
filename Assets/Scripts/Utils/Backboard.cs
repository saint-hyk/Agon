// -----------------------------------------------
// Filename: Backboard.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System;
using UnityEngine;

namespace Utils {
   [RequireComponent(typeof(Camera))]
   public class Backboard : MonoBehaviour {
      #region events

      public event EventHandler BackboardClicked;

      #endregion events

      #region fields

      [SerializeField]
      private float _thickness = 0.5f;

      private BoxCollider _collider;
      private Camera _camera;

      #endregion fields

      #region methods

      #region unity methods

      private void Start() {
         _camera = GetComponent<Camera>();

         _collider = gameObject.AddComponent<BoxCollider>();
      }

      private void Update() {
         _collider.center = new Vector3(0.0f, 0.0f, _camera.farClipPlane + (_thickness / 2.0f));

         Vector3 worldBottomLeft = _camera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, _camera.farClipPlane));
         Vector3 worldTopRight   = _camera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, _camera.farClipPlane));

         Vector3 bottomLeft = transform.worldToLocalMatrix.MultiplyPoint(worldBottomLeft);
         Vector3 topRight   = transform.worldToLocalMatrix.MultiplyPoint(worldTopRight);

         _collider.size = new Vector3(Mathf.Abs(topRight.x - bottomLeft.x),
                                      Mathf.Abs(topRight.y - bottomLeft.y),
                                      _thickness);
      }

      #endregion unity methods

      private void OnMouseDown() {
         EventHandler handler = BackboardClicked;

         if (handler != null) {
            handler(this, null);
         }
      }

      #endregion methods
   }
}
