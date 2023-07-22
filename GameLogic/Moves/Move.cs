using System.Collections.Generic;
using ServiceObjects;
namespace GameLogic.Moves
{
  public abstract class Move : IMove
  {
    protected List<(int, int)> _moves;
    public Move(List<(int,int)> moves)
    {
      _moves = moves;
    }
    public abstract List<CellPlaceholder> ShowPossibleMoves(PieceColor pieceColor, Position position, CellPlaceholder[][] chessBoard);
    public bool CheckAttack(PieceColor pieceColor,CellPlaceholder cell)
    {
      if (cell.PieceInfo == null) return false;
      return pieceColor != cell.PieceInfo.Color;
    }
    public bool CheckMove(CellPlaceholder cell)
    {
      return cell.PieceInfo == null;
    }

  }
}
