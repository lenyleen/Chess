using ServiceObjects;
using UnityEngine;

namespace Editor.TaskLoader.CorrectMoveCheckers
{
    public class PawnChecker : IMoveCorrectnessChecker
    {
        public bool CheckPieceToMove((int, int) selectedCell, (int, int) pieceCell, PieceColor color = PieceColor.None)
        {
            var pieceDefaultPosition = color == PieceColor.Black ? 1 : 6;
            var signe = color == PieceColor.Black ? 1 : -1;
            return CheckAttackMove(selectedCell, pieceCell, signe) ||
                CheckMove(selectedCell, pieceCell, pieceDefaultPosition, signe);
        }

        private bool CheckAttackMove((int, int) selectedCell, (int, int) pieceCell, int signe)
        {
            var horizontalPos = selectedCell.Item1 - pieceCell.Item1;
            var verticalPos = selectedCell.Item2 - pieceCell.Item2;
            var deltaPos = (signe * horizontalPos, signe * verticalPos);
            return deltaPos == (1, 1) || deltaPos == (-1, 1);
        }

        private bool CheckMove((int, int) selectedCell, (int, int) pieceCell, int defaultVerticalPiecePosition, int signe)
        {
            var horizontalPos = selectedCell.Item1 - pieceCell.Item1;
            var verticalDeltaPos = selectedCell.Item2 - pieceCell.Item2;
            if (horizontalPos > 0) return false;
            if (verticalDeltaPos == 1 * signe) return true;
            return pieceCell.Item2 == defaultVerticalPiecePosition && verticalDeltaPos == 2 * signe;
        }
    }
}

