// -----------------------------------------------
// Filename: Board.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System;
using UnityEngine;

namespace HexBoard {
   public class Board : MonoBehaviour {
      #region events

      public event EventHandler HexClicked;

      #endregion events

      #region properties

      public int EdgeSize {
         get { return _edgeSize; }
      }

      public Hex[][] Hexes {
         get { return _hexes; }
      }

      #endregion properties

      #region fields

      // Must have Hex component
      [SerializeField]
      private GameObject _hexPrefab;

      [SerializeField]
      private Vector2 _hexPositionOffset = new Vector3(1, 1);

      [SerializeField]
      private Material[] _materials;

      private int _edgeSize;

      private Hex[][] _hexes;

      private bool _initialized = false;

      #endregion fields

      #region methods

      #region unity methods

      private void Awake() {
         if (_hexPrefab == null || _hexPrefab.GetComponent<Hex>() == null) {
            Debug.LogError("Hex Prefab must contain Hex component");
         }
      }

      #endregion unity methods

      public void Initialize(int edgeSize) {
         if (edgeSize <= 1) {
            Debug.LogError("Edge Size must be greater than 1.");
         } else {
            if (_initialized) {
               ClearBoard();
            }

            _edgeSize = edgeSize;

            CreateEmptyHexes();
            InitializeHexes();

            _initialized = true;
         }
      }

      public void ClearBoard() {
         if (_hexes != null) {
            foreach (Hex[] hexRow in _hexes) {
               foreach (Hex hex in hexRow) {
                  hex.ClearPiece();
                  GameObject.Destroy(hex.gameObject);
               }
            }
         }
      }

      private void CreateEmptyHexes() {
         int currentRowLength = _edgeSize;

         _hexes = new Hex[_edgeSize * 2 - 1][];

         int i;
         for (i = 0; i < _edgeSize; ++i) {
            _hexes[i] = new Hex[currentRowLength++];
         }

         --currentRowLength;

         for (; i < _edgeSize * 2 - 1; ++i) {
            _hexes[i] = new Hex[--currentRowLength];
         }

         GameObject newHex;
         for (i = 0; i < _hexes.Length; ++i) {
            for (int j = 0; j < _hexes[i].Length; ++j) {
               newHex = (GameObject)Instantiate(_hexPrefab);
               newHex.transform.parent = transform;
               _hexes[i][j] = newHex.GetComponent<Hex>();
               _hexes[i][j].HexClicked += OnHexClicked;
            }
         }
      }

      private void InitializeHexes() {
         PositionHexes();

         SetHexNeighbours();

         ApplyMaterials();
      }

      private void PositionHexes() {
         Vector3 pos;

         for (int i = 0; i < _hexes.Length; ++i) {
            float offset =  ((_edgeSize * 2 - 1) - _hexes[i].Length) / 2.0f;

            for (int j = 0; j < _hexes[i].Length; ++j) {
               pos = transform.position;

               pos.x += _hexPositionOffset.x * i;
               pos.z -= _hexPositionOffset.y * (j + offset);

               _hexes[i][j].transform.position = pos;
            }
         }
      }

      private void SetHexNeighbours() {
         // Set all left/right neighbours
         for (int i = 0; i < _edgeSize * 2 - 1; ++i) {
            for (int j = 0; j < _hexes[i].Length; ++j) {
               SetHexSideNeighbours(i, j);
            }
         }

         SetHexNeighboursBottomHalf();

         SetHexNeighboursMiddleRow();

         SetHexNeighboursTopHalf();
      }

      private void SetHexSideNeighbours(int x, int y) {
         if (y != 0) {
            SetHexNeigbour(Hex.HexDirections.Left, x, y);
         }

         if (y != _hexes[x].Length - 1) {
            SetHexNeigbour(Hex.HexDirections.Right, x, y);
         }
      }

      private void SetHexNeighboursBottomHalf() {
         int bottomRow = 0;
         SetHexNeighbourSingleRow(bottomRow, 0, _hexes[bottomRow].Length, Hex.HexDirections.UpperLeft, Hex.HexDirections.UpperRight);

         SetHexNeighboursRange(1, _edgeSize - 2, Hex.HexDirections.UpperLeft, Hex.HexDirections.UpperRight);
      }

      private void SetHexNeighboursMiddleRow() {
         int middleRow = _edgeSize - 1;
         SetHexNeighbourSingleRow(middleRow, 1, _hexes[middleRow].Length, Hex.HexDirections.UpperLeft, Hex.HexDirections.LowerLeft);
         SetHexNeighbourSingleRow(middleRow, 0, _hexes[middleRow].Length - 1, Hex.HexDirections.UpperRight, Hex.HexDirections.LowerRight);
      }

      private void SetHexNeighboursTopHalf() {
         int topRow = _edgeSize * 2 - 2;
         SetHexNeighbourSingleRow(topRow, 0, _hexes[topRow].Length, Hex.HexDirections.LowerLeft, Hex.HexDirections.LowerRight);

         SetHexNeighboursRange(_edgeSize, _edgeSize * 2 - 3, Hex.HexDirections.LowerLeft, Hex.HexDirections.LowerRight);
      }

      private void SetHexNeighbourSingleRow(int row,
                                            int firstColumn,
                                            int lastColumn,
                                            Hex.HexDirections left,
                                            Hex.HexDirections right) {
         for (int j = firstColumn; j < lastColumn; ++j) {
            SetHexNeigbour(left, row, j);
            SetHexNeigbour(right, row, j);
         }
      }

      private void SetHexNeighboursRange(int firstRow,
                                         int lastRow,
                                         Hex.HexDirections majorLeft,
                                         Hex.HexDirections majorRight) {
         Hex.HexDirections minorLeft = Hex.VerticalMirrorDirection(majorLeft);
         Hex.HexDirections minorRight = Hex.VerticalMirrorDirection(majorRight);

         for (int i = firstRow; i <= lastRow; ++i) {
            for (int j = 0; j < _hexes[i].Length; ++j) {
               SetHexNeigbour(majorLeft, i, j);
               SetHexNeigbour(majorRight, i, j);

               if (j != 0) {
                  SetHexNeigbour(minorLeft, i, j);
               }

               if (j != _hexes[i].Length - 1) {
                  SetHexNeigbour(minorRight, i, j);
               }
            }
         }
      }

      private void SetHexNeigbour(Hex.HexDirections neighbour, int x, int y) {
         switch (neighbour) {
            case Hex.HexDirections.UpperRight:
               _hexes[x][y].Neighbours[(int)Hex.HexDirections.UpperRight] = _hexes[x + 1][y + (x < _edgeSize - 1 ? 1 : 0)];
               break;

            case Hex.HexDirections.Right:
               _hexes[x][y].Neighbours[(int)Hex.HexDirections.Right] = _hexes[x][y + 1];
               break;

            case Hex.HexDirections.LowerRight:
               _hexes[x][y].Neighbours[(int)Hex.HexDirections.LowerRight] = _hexes[x - 1][y + (x <= _edgeSize - 1 ? 0 : 1)];
               break;

            case Hex.HexDirections.LowerLeft:
               _hexes[x][y].Neighbours[(int)Hex.HexDirections.LowerLeft] = _hexes[x - 1][y - (x <= _edgeSize - 1 ? 1 : 0)];
               break;

            case Hex.HexDirections.Left:
               _hexes[x][y].Neighbours[(int)Hex.HexDirections.Left] = _hexes[x][y - 1];
               break;

            case Hex.HexDirections.UpperLeft:
               _hexes[x][y].Neighbours[(int)Hex.HexDirections.UpperLeft] = _hexes[x + 1][y - (x < _edgeSize - 1 ? 0 : 1)];
               break;
         }
      }

      private void ApplyMaterials() {
         int finalIndex = _edgeSize * 2 - 2;

         for (int i = 0; i < _edgeSize; ++i) {
            Material current = _materials[i % _materials.Length];

            for (int j = i; j < _hexes[i].Length - i; ++j) {
               int farColumn = _hexes[j].Length - 1 - i;

               _hexes[i][j].SetMaterial(current);
               _hexes[j][i].SetMaterial(current);

               _hexes[finalIndex - i][j].SetMaterial(current);
               _hexes[finalIndex - j][i].SetMaterial(current);

               _hexes[j][farColumn].SetMaterial(current);
               _hexes[finalIndex - j][farColumn].SetMaterial(current);
            }
         }
      }

      private void OnHexClicked(object sender, EventArgs e) {
         EventHandler handler = HexClicked;

         if (handler != null) {
            handler(sender, e);
         }
      }

      #endregion methods
   }
}
