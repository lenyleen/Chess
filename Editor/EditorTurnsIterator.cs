using System;
using System.Collections.Generic;
using ServiceObjects;
using UnityEngine;
namespace Editor
{
   public class EditorTurnsIterator
        {
        private List<PreloadedTurn> _turns;
        private Stack<((Texture, (int, int)),(Texture, (int, int)))> _changedTextures;
        public int IterationIndex
        {
                get => _iterationIndex;
                set
                {
                       
                        _iterationIndex = value;
                        _currentColor = _currentColor == PieceColor.Black ? PieceColor.White : PieceColor.Black;
                }
        }
        private int _iterationIndex;
        private PieceColor _currentColor;
        private string _taskName;
        public EditorTurnsIterator(List<PreloadedTurn> playerTurns, List<PreloadedTurn> comTurns,PieceColor color, string taskName)
        {
                FillTurnsData(playerTurns,comTurns); 
                _iterationIndex = 0;
                _currentColor = color;
                _taskName = taskName;
                _changedTextures = new Stack<((Texture, (int, int)), (Texture, (int, int)))>();
        }

        public void ReInitialize(List<PreloadedTurn> playerTurns, List<PreloadedTurn> comTurns,PieceColor color, string taskName)
        {
                  FillTurnsData(playerTurns,comTurns);      
                _iterationIndex = 0;
                _currentColor = color;
                _taskName = taskName;
                _changedTextures = new Stack<((Texture, (int, int)), (Texture, (int, int)))>();
        }

        private void FillTurnsData(List<PreloadedTurn> playerTurns, List<PreloadedTurn> comTurns)
        {
                _turns = new List<PreloadedTurn>();
                for (int i = 0; i < playerTurns.Count; i++)
                {
                        _turns.Add(playerTurns[i]);
                        if (i < comTurns.Count)
                        {
                                _turns.Add(comTurns[i]);
                        }
                }
        }
        public void NextTurn(Texture defaultImage, Texture[,] chessBoard,Dictionary<string,Texture> piecesTextures)
        {
                if(_iterationIndex >= _turns.Count) return;
                var textureName = $"{_currentColor}{_turns[_iterationIndex].piece}";
                var piecePosition = GetCellWithRequiredPiece(_turns[_iterationIndex].initialCells, chessBoard, textureName);
                var texture = piecesTextures[textureName];
                var selectedCell = (_turns[_iterationIndex]._selectedCellPosition.Item2, _turns[_iterationIndex]._selectedCellPosition.Item1);
                var initCellChangedData = (texture, piecePosition);
                var selectedCellChangedData = (chessBoard[selectedCell.Item1,selectedCell.Item2],(selectedCell.Item1,selectedCell.Item2));
                var changedTextures = (initCellChangedData, selectedCellChangedData);
                _changedTextures.Push(changedTextures); 
                chessBoard[piecePosition.Item2, piecePosition.Item1] = defaultImage;
                chessBoard[selectedCell.Item1, selectedCell.Item2] = texture;
                IterationIndex++;
        } 

        public void PreviousTurn(Texture[,] chessBoard)
        {
                if(_iterationIndex <= 0) return;
                var changes = _changedTextures.Pop();
                var initialCell = changes.Item1;
                var selectedCell = changes.Item2;
                var initialCellPosition = initialCell.Item2;
                var selectedCellPosition = selectedCell.Item2;
                (chessBoard[initialCellPosition.Item2, initialCellPosition.Item1],
                                chessBoard[selectedCellPosition.Item1, selectedCellPosition.Item2]) =
                        (initialCell.Item1, selectedCell.Item1);
                IterationIndex--;
        }
        private (int, int) GetCellWithRequiredPiece(List<(int,int)> cells, Texture[,] chessBoard, string textureName)
        {
                foreach (var cell in cells)
                {
                        if (chessBoard[cell.Item2, cell.Item1].name == textureName) 
                                return cell;
                }
                throw new Exception($"Cant find required piece, at :\n Task : {_taskName} \n Curren color : {_currentColor} \n Turn index : {_iterationIndex}");
        }     
}

        
}
