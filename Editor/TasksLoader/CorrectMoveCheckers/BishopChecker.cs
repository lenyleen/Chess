using ServiceObjects;
using UnityEngine;

namespace Editor.TaskLoader.CorrectMoveCheckers
{
    public class BishopChecker : IMoveCorrectnessChecker
    {
        public bool CheckPieceToMove((int, int) selectedCell, (int, int) pieceCell, PieceColor color = PieceColor.None)
        {
            var horizontalPos = Mathf.Abs(selectedCell.Item1 - pieceCell.Item1);
            var verticalPos = Mathf.Abs(selectedCell.Item2 - pieceCell.Item2);
            return horizontalPos == verticalPos;
        }
    }
}