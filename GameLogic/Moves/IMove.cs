using System.Collections.Generic;
using ServiceObjects;

namespace GameLogic.Moves
{
    public interface IMove
    { 
        public List<CellPlaceholder> ShowPossibleMoves(PieceColor pieceColor, Position position, CellPlaceholder[][] chessBoard);
    }
}