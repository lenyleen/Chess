using System.Collections.Generic;
using ServiceObjects;
using WithMVCS;
using Zenject;

namespace Chess_AI.AIPiecesMoves
{
    public class KnightMoves : IAIMove
    {
        private readonly List<(int, int)> _moves = new List<(int, int)>() {(2, 1), (2, -1), (-2, 1), (-2, -1), (1, 2), (-1, 2), (-1, -2), (1, -2)};
        public List<AITurn> GetPossibleMoves(AIPiece[][] board, PieceColor color,(int,int) position,AITurn.Pool turnsPool)
        {
            var maxLength = Constants.ChessBoardHeight;
            var possibleMove = new List<AITurn>();
            foreach (var move in _moves)
            {
                var toPlacePosition = (position.Item1 + move.Item1, position.Item2 + move.Item2);
                if (toPlacePosition.Item1 < 0 || toPlacePosition.Item2 < 0 || toPlacePosition.Item1 >= maxLength 
                    || toPlacePosition.Item2 >= maxLength) continue;
                if(board[toPlacePosition.Item1][toPlacePosition.Item2] is not null && board[toPlacePosition.Item1][toPlacePosition.Item2].Color == color) continue;
                possibleMove.Add(turnsPool.Spawn(position,toPlacePosition,PieceType.Knight,board[toPlacePosition.Item1][toPlacePosition.Item2]));
            }

            return possibleMove;
        }
    }
}