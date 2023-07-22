  using ServiceObjects;
  namespace Editor.TaskLoader.CorrectMoveCheckers
  {
    public class QueenChecker : IMoveCorrectnessChecker
    {
      public bool CheckPieceToMove((int, int) selectedCell, (int, int) pieceCell, PieceColor color = PieceColor.None)
      {
        var bishopMove = new BishopChecker();
        var rookMove = new RookChecker();
        return bishopMove.CheckPieceToMove(selectedCell, pieceCell) ||
          rookMove.CheckPieceToMove(selectedCell, pieceCell);
      }
    }
  }
