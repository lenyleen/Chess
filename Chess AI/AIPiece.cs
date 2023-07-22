using ServiceObjects;
using WithMVCS;
using Zenject;
namespace Chess_AI
{
    public class AIPiece
    {
        public PieceType PieceType;
        public int Value;
        public PieceColor Color;
        public (int, int) Position;
        public bool Captured = false;
        public AIPiece(PieceType pieceType, int value, PieceColor color, (int, int) position)
        {
            PieceType = pieceType;
            Value = value;
            Color = color;
            Position = position;
        }
        public class Factory : PlaceholderFactory<PieceType,int,PieceColor,(int,int),AIPiece>
        {
            
        }
    }
}