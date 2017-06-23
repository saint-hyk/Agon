// -----------------------------------------------
// Filename: GameController.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using HexBoard;
using System;
using UnityEngine;

namespace Agon {
   /// <summary>
   /// Contains the rules for a game of Agon
   /// </summary>
   [RequireComponent(typeof(Board))]
   public class GameController : MonoBehaviour {
      #region classes

      private class SelectedHex {
         private Hex _selectedHex = null;

         public Hex Current {
            get { return _selectedHex; }
            set {
               if (_selectedHex != null) {
                  _selectedHex.Selected = false;
               }

               _selectedHex = value;

               if (_selectedHex != null) {
                  _selectedHex.Selected = true;
               }
            }
         }
      }

      #endregion classes

      #region enums

      private enum Player {
         PlayerOne = 0,
         PlayerTwo = 1
      }

      #endregion enums

      #region constants

      private const int AGON_BOARD_SIZE = 6;

      #endregion constants

      #region fields

      [SerializeField]
      private Utils.Backboard _backboard;

      [SerializeField]
      private GameObject _queenPrefab;

      [SerializeField]
      private GameObject _knightPrefab;

      [SerializeField]
      private Material _playerOneMaterial;

      [SerializeField]
      private Material _playerTwoMaterial;

      [SerializeField]
      private float _verticalPieceOffset;

      private Board _board;
      private SelectedHex _selectedHex;

      #endregion fields

      #region methods

      #region unity methods

      private void Awake() {
         _board = GetComponent<Board>();
         _selectedHex = new SelectedHex();
      }

      private void Start() {
         _board.Initialize(AGON_BOARD_SIZE);
         _board.HexClicked += OnHexClicked;

         _backboard.BackboardClicked += OnBackboardClicked;

         InitializePieces();
      }

      #endregion unity methods

      private void Reset() {
         _board.ClearBoard();

         InitializePieces();
      }

      private void InitializePieces() {
         InitializePiece(_queenPrefab, Player.PlayerOne, 10, 0);
         InitializePiece(_knightPrefab, Player.PlayerOne, 10, 4);
         InitializePiece(_knightPrefab, Player.PlayerOne, 6, 9);
         InitializePiece(_knightPrefab, Player.PlayerOne, 2, 7);
         InitializePiece(_knightPrefab, Player.PlayerOne, 0, 3);
         InitializePiece(_knightPrefab, Player.PlayerOne, 1, 0);
         InitializePiece(_knightPrefab, Player.PlayerOne, 6, 0);

         InitializePiece(_queenPrefab, Player.PlayerTwo, 0, 5);
         InitializePiece(_knightPrefab, Player.PlayerTwo, 10, 2);
         InitializePiece(_knightPrefab, Player.PlayerTwo, 9, 6);
         InitializePiece(_knightPrefab, Player.PlayerTwo, 4, 9);
         InitializePiece(_knightPrefab, Player.PlayerTwo, 0, 1);
         InitializePiece(_knightPrefab, Player.PlayerTwo, 4, 0);
         InitializePiece(_knightPrefab, Player.PlayerTwo, 8, 0);
      }

      private void InitializePiece(GameObject prefab, Player player, int x, int y) {
         GameObject obj = GameObject.Instantiate(prefab) as GameObject;

         obj.transform.parent = _board.Hexes[x][y].transform;
         obj.transform.position = new Vector3(_board.Hexes[x][y].transform.position.x,
                                              obj.transform.position.y,
                                              _board.Hexes[x][y].transform.position.z);

         GamePiece piece = obj.GetComponent<GamePiece>();

         if (piece != null) {
            piece.Initialize();
            piece.SetMaterial(player == Player.PlayerOne ? _playerOneMaterial : _playerTwoMaterial);

            _board.Hexes[x][y].CurrentPiece = piece;
         }
      }

      private void OnHexClicked(object sender, EventArgs e) {
         Hex hex = (Hex)sender;
         if (_selectedHex.Current == null) {
            _selectedHex.Current = hex;
         } else {
            //if legal
               //move selected hex to this hex
               _selectedHex.Current = null;
            //else
               //complain?
         }
      }

      private void OnBackboardClicked(object sender, EventArgs e) {
         _selectedHex.Current = null;
      }

      #endregion methods
   }
}
