// -----------------------------------------------
// Filename: UniformPolygonDrawer.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformPolygonDrawer : MonoBehaviour {
   #region fields

   [SerializeField]
   private int _sides;

   [SerializeField]
   private float _size;

   [SerializeField]
   private float _centreExtrusion;

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

      float u = Mathf.Sin((0) * step);
      float v = Mathf.Cos((0) * step);

      for (int i = 0; i < _sides; ++i) {
         triangles.Add(vertices.Count);
         vertices.Add(new Vector3(0, _centreExtrusion, 0));
         uvs.Add(new Vector2(0.5f, 0.5f));

         triangles.Add(vertices.Count);
         vertices.Add(new Vector3(u * _size, 0, v * _size));
         uvs.Add(new Vector2(u / 2.0f + 0.5f, v / 2.0f + 0.5f));

         triangles.Add(vertices.Count);
         u = Mathf.Sin((i + 1) * step);
         v = Mathf.Cos((i + 1) * step);
         vertices.Add(new Vector3(u * _size, 0, v * _size));
         uvs.Add(new Vector2(u / 2.0f + 0.5f, v / 2.0f + 0.5f));

         Vector3 n = Vector3.Normalize(Vector3.Cross(vertices[vertices.Count - 2] - vertices[vertices.Count - 3],
                                                     vertices[vertices.Count - 1] - vertices[vertices.Count - 3]));
         normals.Add(n); normals.Add(n); normals.Add(n);
      }

      mesh.vertices = vertices.ToArray();
      mesh.uv = uvs.ToArray();
      mesh.normals = normals.ToArray();
      mesh.triangles = triangles.ToArray();
   }

   #endregion methods
}
