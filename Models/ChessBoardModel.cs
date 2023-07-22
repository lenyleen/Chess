using System;
using System.Collections.Generic;
using GameLogic;
using GameLogic.Signals;
using ServiceObjects;
using GameLogic.TurnHandlers;
using UnityEngine;
using Zenject;
namespace Models
{
  public class ChessBoardModel : IInitializable,IDisposable
  {
    private readonly CellPlaceholder[][] _cells;
    private readonly SignalBus _signalBus;
    private TurnsController _turnsHandler;
    public event Action<Turn> SetOpponentTurn;
    public ChessBoardModel(CellPlaceholder[][] cells, TurnsController turnsController,SignalBus signalBus)
    {
      _cells = cells;
      _turnsHandler = turnsController;
      _signalBus = signalBus;
    }
    public void Initialize()
    {
      _signalBus.Subscribe<TurnEndedSignal>(TurnEnded);
    }
    public CellPlaceholder GetCellAt((int, int) matrixPosition)
    {
      return _cells[matrixPosition.Item1][matrixPosition.Item2];
    }
    public List<CellPlaceholder> PieceSelected(PieceInfo pieceInfo)
    {
      var cellsToPlace = _turnsHandler.PieceSelected(pieceInfo,_cells);
      return cellsToPlace;
    }
    public CellPlaceholder CellSelected((int,int) matrixPosition, PieceInfo selectedPieceInfo)
    {
      var cell = GetCellAt(matrixPosition);
      _turnsHandler.CellSelected(cell);
      var initialCell = GetCellAt(selectedPieceInfo.Position.MatrixPosition);
      initialCell.PieceInfo = null;
      cell.PieceInfo = selectedPieceInfo;
      return cell;
    }
    public void PiecePlaced()
    {
      _turnsHandler.ChangeTurnsHandler();
      Debug.Log("chaged");
    }
    private void TurnEnded(TurnEndedSignal signal)
    {
      if(signal.TurnInfo.PlayerType == PlayerType.Player) return;
      SetOpponentTurn?.Invoke(signal.TurnInfo);
    }
    
    public void Dispose()
    {
      _signalBus.Unsubscribe<TurnEndedSignal>(TurnEnded);
    }
  }
}

