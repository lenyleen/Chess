using System.Collections.Generic;
using ServiceObjects;
namespace GameLogic.Moves
{
  public class KingMoves : Move
  {
    public KingMoves(List<(int, int)> moves) : base(moves)
    {
    }
    public override List<CellPlaceholder> ShowPossibleMoves(PieceColor pieceColor, Position position, CellPlaceholder[][] chessBoard)
    {
      var pieceMatrixPos = position.MatrixPosition;
      var maxLength = Constants.ChessBoardHeight;
      var suitableCells = new List<CellPlaceholder>();
      foreach (var move in _moves)
      {
        var curX = pieceMatrixPos.Item1;
        var curY = pieceMatrixPos.Item2;
        curX += move.Item1;
        curY += move.Item2;
        if (curX < 0 || curY < 0 || curX >= maxLength || curY >= maxLength) continue;
        var moveRes =  CheckMove(chessBoard[curX][curY]);
        var attackRes = CheckAttack(pieceColor,chessBoard[curX][curY]);
        if (attackRes)
          suitableCells.Add(chessBoard[curX][curY]);
        if (!moveRes) continue;
        suitableCells.Add(chessBoard[curX][curY]);
      }
      return suitableCells;
    }
  }
}
