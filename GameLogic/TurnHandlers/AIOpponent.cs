using System;
using UnityEngine;
using Chess_AI;
using ServiceObjects;
namespace GameLogic.TurnHandlers
{
  public class AIOpponent : IPlayerTurnHandler
  {
    private readonly AIChessBoard _aiChessBoard;
    private readonly Turn.Factory _turnFactory;
    public event Action<Turn> TurnEnded;
    public AIOpponent(Turn.Factory turnFactory, AIChessBoard aiChessBoard)
    {
      _aiChessBoard = aiChessBoard;
      _turnFactory = turnFactory;
    }
    public void StartTurn()
    {
      var aiTurn = _aiChessBoard.GetBestTurn(3);
      var boardTurn = _turnFactory.Create(PlayerType.Opponent).WithInitialCell(aiTurn.Position).WithSelectedCell(aiTurn.ToPlacePosition);
      TurnEnded?.Invoke(boardTurn);
    }
    public void Undo()
    {
      throw new NotImplementedException();
    }
    public void EndTurn()
    {
      throw new NotImplementedException();
    }
    public void PieceSelected(PieceInfo selectedPiece)
    {
      throw new NotImplementedException();
    }
    public void CellSelected(CellPlaceholder capturedPiece)
    {
      
    }
  }
}
