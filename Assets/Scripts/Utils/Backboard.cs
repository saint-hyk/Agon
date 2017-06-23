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

      #region fields

      [SerializeField]
      private float _thickness = 0.5f;

      private GameObject _backboard;
      private BoxCollider _collider;
      private ClickPasser _clickPasser;
      private Camera _camera;

      #endregion fields

      #region methods

      #region unity methods

      private void Start() {
         _backboard = new GameObject("Backboard");
         _backboard.transform.parent = transform;

         _collider = _backboard.AddComponent<BoxCollider>();

         _clickPasser = _backboard.AddComponent<ClickPasser>();
         _clickPasser.Clicked += OnBackboardClicked;

         _camera = GetComponent<Camera>();
      }

      private void Update() {
         _backboard.transform.localPosition = new Vector3(0.0f, 0.0f, _camera.farClipPlane + (_thickness / 2.0f));

         Vector3 bottomLeft = _camera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, _camera.farClipPlane));
         Vector3 topRight = _camera.ScreenToWorldPoint(new Vector3(_camera.pixelWidth, _camera.pixelHeight, _camera.farClipPlane));
         _collider.size = new Vector3(topRight.x - bottomLeft.x, _thickness, bottomLeft.z - topRight.z);
      }

      #endregion unity methods

      private void OnBackboardClicked(object sender, EventArgs e) {
         Debug.Log("Backboard");
      }

      #endregion methods
   }
}
