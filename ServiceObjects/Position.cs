
using UnityEngine;
namespace ServiceObjects
{
  public struct Position
  {
    public readonly (int, int) MatrixPosition;
    public readonly (char, int) ChessBoardPosition;
    public readonly Vector3 WorldPosition;
    public Position(Vector3 worldPosition, (int, int) matrixPosition)
    {
      WorldPosition = worldPosition;
      ChessBoardPosition = ((char)(HorizontalPosition)matrixPosition.Item1,matrixPosition.Item2);
      MatrixPosition = matrixPosition;
    }
    public enum HorizontalPosition
    {
      A = 0, 
      B,
      C,
      D,
      E,
      F,
      G,
      H
    }
  }
}
