// -----------------------------------------------
// Filename: UniformPolygonDrawer.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Geometry {
   public class UniformPolygonDrawer : MonoBehaviour {
      #region fields

      [SerializeField]
      private int _sides;

      [SerializeField]
      private float _size;

      [SerializeField]
      private float _centreExtrusion;

      [SerializeField]
      private float _height;

      #endregion fields

      #region methods

      #region unity methods

      private void OnEnable() {
         Mesh mesh = new Mesh();

         MeshFilter filter = GetComponent<MeshFilter>();
         if (filter == null) {
            filter = gameObject.AddComponent<MeshFilter>();
         }
         filter.mesh = mesh;

         CreateUniformPolygon(ref mesh);
      }

      #endregion unity methods

      private void CreateUniformPolygon(ref Mesh mesh) {
         List<Vector3> vertices = new List<Vector3>();
         List<int> triangles = new List<int>();
         List<Vector2> uvs = new List<Vector2>();
         List<Vector3> normals = new List<Vector3>();

         float step = Mathf.PI * 2 / _sides;

         float u = Mathf.Sin(0);
         float v = Mathf.Cos(0);

         for (int i = 0; i < _sides; ++i) {
            // middle of polygon
            vertices.Add(new Vector3(0, _height + _centreExtrusion, 0));
            uvs.Add(new Vector2(0.5f, 0.5f));

            // first point on border
            AddBorderPoints(vertices, uvs, u, v);

            // first half of side
            AddSidePoints(vertices, uvs, u, v);

            // second point on border
            u = Mathf.Sin((i + 1) * step);
            v = Mathf.Cos((i + 1) * step);
            AddBorderPoints(vertices, uvs, u, v);

            // second half of side
            AddSidePoints(vertices, uvs, u, v);

            triangles.AddInOrderWithOffset(vertices.Count, -7, -6, -3, -5, -4, -2, -1, -2, -4);

            Vector3 topNormal = Vector3.Normalize(Vector3.Cross(vertices[vertices.Count - 6] - vertices[vertices.Count - 7],
                                                                vertices[vertices.Count - 3] - vertices[vertices.Count - 7]));
            Vector3 sideNormal = Vector3.Normalize(Vector3.Cross(vertices[vertices.Count - 1] - vertices[vertices.Count - 4],
                                                                 vertices[vertices.Count - 2] - vertices[vertices.Count - 4]));
            normals.AddInOrder(topNormal, topNormal, sideNormal, sideNormal, topNormal, sideNormal, sideNormal);
         }

         mesh.vertices = vertices.ToArray();
         mesh.uv = uvs.ToArray();
         mesh.normals = normals.ToArray();
         mesh.triangles = triangles.ToArray();
      }

      private void AddBorderPoints(List<Vector3> vertices, List<Vector2> uvs, float u, float v) {
         vertices.Add(new Vector3(u * _size, _height, v * _size));
         uvs.Add(new Vector2(u / 2.0f + 0.5f, v / 2.0f + 0.5f));
      }

      private void AddSidePoints(List<Vector3> vertices, List<Vector2> uvs, float u, float v) {
         vertices.Add(new Vector3(u * _size, _height, v * _size));
         uvs.Add(new Vector2(0, 0));

         vertices.Add(new Vector3(u * _size, 0, v * _size));
         uvs.Add(new Vector2(0, 0));
      }

      #endregion methods
   }
}
