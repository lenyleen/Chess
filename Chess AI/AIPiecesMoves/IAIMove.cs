using System.Collections.Generic;
using ServiceObjects;

namespace Chess_AI.AIPiecesMoves
{
    public interface IAIMove
    {
        public List<AITurn> GetPossibleMoves(AIPiece[][] board, PieceColor color, (int, int) position,AITurn.Pool turnsPool);
    }
}