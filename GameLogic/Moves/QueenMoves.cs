using System.Collections.Generic;
using GameLogic.Moves;
using ServiceObjects;

namespace GameLogic.Moves
{
    public class QueenMoves : Move
    {
        private IMove _rookMoves;
        private IMove _bishopMoves; 
        public QueenMoves(List<(int,int)> moves,IMove rookMoves, IMove bishopMoves) : base(moves)
        {
            _rookMoves = rookMoves;
            _bishopMoves = bishopMoves;
        } 
        public override List<CellPlaceholder> ShowPossibleMoves(PieceColor pieceColor, Position position, CellPlaceholder[][] chessBoard)
        {
            var linearMoves = _rookMoves.ShowPossibleMoves(pieceColor,position, chessBoard);
            var diagonalMoves = _bishopMoves.ShowPossibleMoves(pieceColor, position, chessBoard);
            linearMoves.AddRange(diagonalMoves);
            return linearMoves;;
        }
    }
}
