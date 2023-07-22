using System.Collections.Generic;
using ServiceObjects;
using WithMVCS;
using Zenject;

namespace Chess_AI.AIPiecesMoves
{
    public class PawnMoves : IAIMove
    {
        private readonly List<(int, int)> _moves = new List<(int, int)>() {(0, 1)};
        private readonly List<(int, int)> _attackMoves = new List<(int, int)>() {(1, 1), (1, -1)};
        private readonly HashSet<(int, int)> _defaultPawnCells;
        public PawnMoves()
        {
            _defaultPawnCells = new HashSet<(int, int)>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i is 1 or 6)
                    {
                        _defaultPawnCells.Add((i, j));
                    }
                }
            }
        }

        public List<AITurn> GetPossibleMoves(AIPiece[][] board, PieceColor color,(int,int) position, AITurn.Pool turnsPool)
        {
            var possibleTurns = new List<AITurn>();
            var direction = color == PieceColor.Black ? 1 : -1;
            var y = _defaultPawnCells.Contains(position) ? 2 : 1;
            var toPlacePosition = position;
            for (var i = 1;
                 i <= y && (toPlacePosition.Item1 < Constants.ChessBoardHeight || toPlacePosition.Item1 > 0);
                 i++)
            {
                toPlacePosition.Item1 += direction;
                if(board[toPlacePosition.Item1][toPlacePosition.Item2] is not null) break;
                possibleTurns.Add(turnsPool.Spawn(position,toPlacePosition,PieceType.Pawn,null));
            } possibleTurns.AddRange(GetPossibleAttackTurns(board,color,position,turnsPool));
            return possibleTurns;
        }

        private List<AITurn> GetPossibleAttackTurns(AIPiece[][] board, PieceColor color, (int, int) position, AITurn.Pool turnsPool)
        {
            var possibleTurns = new List<AITurn>();
            var direction = color == PieceColor.Black ? 1 : -1;
            var maxLength = Constants.ChessBoardHeight;
            foreach (var move in _attackMoves)
            {
                var curX = position.Item1 + (move.Item1 * direction);
                var curY = position.Item2 + (move.Item2 * direction);
                if (curX < 0 || curY < 0 || curX >= maxLength || curY >= maxLength) continue;
                if(board[curX][curY] is null || board[curX][curY].Color == color) continue;
                possibleTurns.Add(turnsPool.Spawn(position,(curX,curY),PieceType.Pawn,board[curX][curY]));
            }
            return possibleTurns;
        }
    }
}