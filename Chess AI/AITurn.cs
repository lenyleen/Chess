using System;
using System.Collections.Generic;
using ServiceObjects;
using WithMVCS;
using Zenject;

namespace Chess_AI
{
    public class AITurn
    {
        public (int, int) Position;
        public (int, int) ToPlacePosition;
        public PieceType Name;
        public AIPiece CapturedPiece;

        public void Initialize((int, int) position, (int, int) toPlacePosition, PieceType name, AIPiece capturedPiece)
        {
            Name = name;
            Position = position;
            ToPlacePosition = toPlacePosition;
            CapturedPiece = capturedPiece;
        }
        public void ToDefault()
        {
            Name = PieceType.None;
            Position = (0,0);
            ToPlacePosition = (0,0);
            CapturedPiece = null;
        }
        public class Pool : MemoryPool<(int,int), (int,int), PieceType, AIPiece,AITurn>
        {
            protected override void Reinitialize((int, int) position, (int, int) toPlacePosition, PieceType name, AIPiece piece, AITurn turn)
            {
                turn.Initialize(position,toPlacePosition,name,piece);
            }

            protected override void OnDespawned(AITurn item)
            {
                item.ToDefault();
            }

            public void DespawnAll(List<AITurn> turns)
            {
                var count = turns.Count;
                for (int i = 0; i < count; i++)
                {
                    Despawn(turns[0]);
                    turns.RemoveAt(0);
                }
            }
        }
    }
}