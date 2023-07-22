using System;
using System.Collections.Generic;
using ServiceObjects;
using Views;
namespace GameLogic
{
  public interface IPlayerTurnHandler : ITurnHandler
  {
    public event Action<Turn> TurnEnded;
    public void PieceSelected(PieceInfo selectedPiece);
    public void CellSelected(CellPlaceholder capturedPiece);
  }
}
