using System.Collections.Generic;
using ServiceObjects;

namespace Chess_AI.AIPiecesMoves
{
    public class QueenMoves : IAIMove
    {
        private IAIMove _rookMoves;
        private IAIMove _bishopMoves;
        public QueenMoves()
        {
            _bishopMoves = new BishopMoves();
            _rookMoves = new RookMoves();
        }

        public List<AITurn> GetPossibleMoves(AIPiece[][] board, PieceColor color, (int, int) position,AITurn.Pool turnsPool)
        {
            var possibleMoves = _rookMoves.GetPossibleMoves(board, color, position,turnsPool);
            possibleMoves.AddRange(_bishopMoves.GetPossibleMoves(board,color,position,turnsPool));
            return possibleMoves;
        }
    }
}