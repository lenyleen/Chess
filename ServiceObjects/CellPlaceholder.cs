using Views;
namespace ServiceObjects
{
  public class CellPlaceholder
  {
    public PieceInfo PieceInfo;
    public readonly Position Position;
    public CellPlaceholder(PieceInfo pieceInfo, Position position)
    {
      PieceInfo = pieceInfo;
      Position = position;
    }
  }
}
