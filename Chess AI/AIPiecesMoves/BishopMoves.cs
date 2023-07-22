using System.Collections.Generic;
using ServiceObjects;
using WithMVCS;
using Zenject;

namespace Chess_AI.AIPiecesMoves
{
    public class BishopMoves : IAIMove
    {
        private readonly List<(int, int)> _moves = new List<(int, int)>() {(1, 1), (-1, 1), (-1, -1), (1, -1)};
        public List<AITurn> GetPossibleMoves(AIPiece[][] board, PieceColor color, (int, int) position,AITurn.Pool turnsPool)
        {
            var maxLength = Constants.ChessBoardHeight;
            var possibleMove = new List<AITurn>();
            foreach (var move in _moves)
            {
                var toPlacePosition = position;
                var t = 0;
                while (t < maxLength)
                {
                    toPlacePosition.Item1 += move.Item1;
                    toPlacePosition.Item2 += move.Item2;
                    if (toPlacePosition.Item1 < 0 || toPlacePosition.Item2 < 0 || toPlacePosition.Item1 >= maxLength || toPlacePosition.Item2 >= maxLength) break;
                    if(board[toPlacePosition.Item1][toPlacePosition.Item2] is not null && board[toPlacePosition.Item1][toPlacePosition.Item2].Color == color) break;
                    possibleMove.Add(turnsPool.Spawn(position,toPlacePosition,PieceType.Bishop,board[toPlacePosition.Item1][toPlacePosition.Item2]));
                    if(board[toPlacePosition.Item1][toPlacePosition.Item2] is not null && board[toPlacePosition.Item1][toPlacePosition.Item2].Color != color) break;
                    t++;
                }
            }
            return possibleMove;
        }
    }
}