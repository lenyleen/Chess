using System;
using UnityEngine;
namespace ServiceObjects
{
  [Serializable]
  public class PieceInfo
  {
    [field: SerializeField] public PieceColor Color { get; private set; }
    [field: SerializeField] public PieceType Type { get; private set; }
    public Position Position;
    public PieceInfo(Position position,PieceColor color, PieceType type)
    {
      Position = position;
      Color = color;
      Type = type;
    }
    public void SetPosition(Position position)
    {
      Position = position;
    }
  }
}
