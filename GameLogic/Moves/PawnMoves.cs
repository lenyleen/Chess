using System.Collections.Generic;
using ServiceObjects;
using UnityEngine;

namespace GameLogic.Moves
{
    public class PawnMoves : Move
    {
        private readonly List<(int, int)> _attackMoves;
        private readonly HashSet<(int, int)> _defaultPawnCells;
        public PawnMoves(List<(int,int)> moves,List<(int,int)> attackMoves) : base(moves)
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
            _attackMoves = attackMoves;
        }
        public override List<CellPlaceholder> ShowPossibleMoves(PieceColor pieceColor, Position position, CellPlaceholder[][] chessBoard)
        {
            var pieceMatrixPos = position.MatrixPosition;
            var direction = pieceColor == PieceColor.Black ? 1 : -1;
            var y = _defaultPawnCells.Contains(pieceMatrixPos) ? 2 : 1;
            var suitableCells = new List<CellPlaceholder>();
            for (var i = 1;
                 i <= y && (pieceMatrixPos.Item1 < Constants.ChessBoardHeight || pieceMatrixPos.Item1 > 0);
                 i++)
            {
                pieceMatrixPos.Item1 += direction;
                var res = CheckMove(chessBoard[pieceMatrixPos.Item1][pieceMatrixPos.Item2]);
                if (!res) break;
                suitableCells.Add(chessBoard[pieceMatrixPos.Item1][ pieceMatrixPos.Item2]);
            }
            var attackMoves = PossibleAttack(pieceColor,position, chessBoard);
            suitableCells.AddRange(attackMoves);
            return suitableCells;
        }
        private List<CellPlaceholder> PossibleAttack(PieceColor pieceColor, Position position, CellPlaceholder[][] chessBoard)
        {
            var pieceMatrixPos = position.MatrixPosition;
            var suitableCells = new List<CellPlaceholder>();
            var direction = pieceColor == PieceColor.Black ? 1 : -1;
            var maxLength = Constants.ChessBoardHeight;
            foreach (var move in _attackMoves)
            {
                var curX = pieceMatrixPos.Item1 + (move.Item1 * direction);
                var curY = pieceMatrixPos.Item2 + (move.Item2 * direction);
                if (curX < 0 || curY < 0 || curX >= maxLength || curY >= maxLength) continue;
                var res = CheckAttack(pieceColor,chessBoard[curX][curY]);
                if (!res) continue;
                suitableCells.Add(chessBoard[curX][curY]);
            }
            return suitableCells;
        }
    }
}
