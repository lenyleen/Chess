using ServiceObjects;
using Unity.VisualScripting;
namespace GameLogic
{
  public static class TurnBuilderExtensions
  {
    public static Turn WithInitialCell(this Turn turn, (int,int) initialCellPosition)
    {
      turn.InitialCellPosition = initialCellPosition;
      return turn;
    }
    public static Turn WithSelectedCell(this Turn turn, (int,int) selectedCellPosition)
    {
      turn.SelectedCellPosition = selectedCellPosition;
      return turn;
    }
  }
}
