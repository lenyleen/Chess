namespace GameLogic.Signals
{
  public class TurnEndedSignal
  {
    public Turn TurnInfo { get; }
    public TurnEndedSignal(Turn turnInfo)
    {
      TurnInfo = turnInfo;
    }
  }
}
