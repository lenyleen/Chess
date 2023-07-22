using ServiceObjects;
namespace Editor.TaskLoader.CorrectMoveCheckers
{
    public interface IMoveCorrectnessChecker
    {
        public bool CheckPieceToMove((int, int) selectedCell, (int, int) pieceCell, PieceColor color = PieceColor.None);
    }
}
