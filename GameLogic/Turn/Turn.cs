using ServiceObjects;
using Views;
using Zenject;
namespace GameLogic
{
  public class Turn
  {
    public PlayerType PlayerType { get; private set; }
    public (int,int) InitialCellPosition { get; set; }
    public (int,int) SelectedCellPosition { get;set; }
    public Turn(PlayerType playerType)
    {
      PlayerType = playerType;
    }
    public bool CheckTurnToCorrectness((int,int) correctPiecePlacedPosition, (int,int) correctInitPosition)
    {
      return correctPiecePlacedPosition.Equals(SelectedCellPosition) && InitialCellPosition.Equals(correctInitPosition);
    }
    public class Factory : PlaceholderFactory<PlayerType,Turn>
    {
      
    }
  }
}
