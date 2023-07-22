using System;
using System.Collections.Generic;
using System.Numerics;
using GameLogic;
using Models;
using ServiceObjects;
using UnityEngine;
using Views;
using Zenject;
using Vector3 = UnityEngine.Vector3;
namespace Controllers
{
  public class ChessBoardController : IInitializable,IDisposable
  {
    private PieceView[][] _views;
    private ChessBoardModel _model;
    private CellsHighlighter _cellsHighlighter;
    private CapturedPiecesPool _capturedPieces;
    private PieceView _selectedPiece;
    public ChessBoardController(PieceView[][] views,ChessBoardModel model,CellsHighlighter cellsHighlighter,CapturedPiecesPool capturedPieces)
    {
      _views = views;
      _model = model;
      _cellsHighlighter = cellsHighlighter;
      _capturedPieces = capturedPieces;
      
    }
    public void Initialize()
    {
      for (int i = 0; i < _views.Length; i++)
      {
        for (int j = 0; j < _views[i].Length; j++)
        {
          if(_views[i][j] == null) continue;
          _views[i][j].OnSelectedEvent += PieceSelected;
        }
      }
      _model.SetOpponentTurn += SetOpponentsTurn;
    }
    private void PieceSelected(PieceView view)
    {
      var cellsToHighlight = _model.PieceSelected(view.Info);
      if (cellsToHighlight.Count == 0)
      {
        view.Deselect();
        return;
      }
      _selectedPiece = view;
      var cellViews = _cellsHighlighter.HighlightCells(cellsToHighlight);
      foreach (var cell in cellViews)
      {
        cell.OnSelectedEvent += CellSelected;
      }
    }
    private async void CellSelected((int,int) matrixPosition)
    {
      var cells = _cellsHighlighter._activeCells;
      foreach (var cell in cells)
      {
        cell.OnSelectedEvent -= CellSelected;
      }
      _cellsHighlighter.DeactivateCells();
      var cellToPlace = _model.CellSelected(matrixPosition,_selectedPiece.Info);
      var position = cellToPlace.Position;
      var capturedPiece = _views[position.MatrixPosition.Item1][position.MatrixPosition.Item2];
      if (capturedPiece != null)
      {
        _capturedPieces.CapturePiece(capturedPiece);
      }
      SetViewNewPosition(_selectedPiece,matrixPosition);
      await _selectedPiece.Replace(position);
      _selectedPiece = null;
      _model.PiecePlaced();
      Debug.Log("placed");
    }
    private void SetOpponentsTurn(Turn turn)
    {
      var position = turn.InitialCellPosition;
      _selectedPiece = _views[position.Item1][position.Item2];
      CellSelected(turn.SelectedCellPosition);
    }
    private void SetViewNewPosition(PieceView view, (int,int) matrixPosition)
    {
      var selectedPiecePosition = view.Info.Position.MatrixPosition;
      (_views[matrixPosition.Item1][matrixPosition.Item2], _views[selectedPiecePosition.Item1][selectedPiecePosition.Item2]) =
        (view, null);
    }
    public void Dispose()
    {
      throw new NotImplementedException();
    }
  }
}
