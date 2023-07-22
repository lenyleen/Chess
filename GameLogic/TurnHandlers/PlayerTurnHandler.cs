using System;
using System.Collections.Generic;
using ServiceObjects;
using Views;
namespace GameLogic
{
  public class PlayerTurnHandler : IPlayerTurnHandler
  {
    private readonly Turn.Factory _turnFactory;
    private Turn _currentTurn;
    public event Action<Turn> TurnEnded;
    public PlayerTurnHandler(Turn.Factory factory)
    {
      _turnFactory = factory;
    }
    public void StartTurn()
    {
      
    }
    public void PieceSelected(PieceInfo selectedPiece)
    {
      _currentTurn = _turnFactory.
        Create(PlayerType.Player).
        WithInitialCell(selectedPiece.Position.MatrixPosition);
      
    }
    public void CellSelected(CellPlaceholder selectedCell)
    {
      _currentTurn.WithSelectedCell(selectedCell.Position.MatrixPosition);
      EndTurn();
    }
    public void Undo()
    {
       throw new System.NotImplementedException();
    }
    public void EndTurn()
    {
      TurnEnded?.Invoke(_currentTurn);
      _currentTurn = null;
    }
  }
}
