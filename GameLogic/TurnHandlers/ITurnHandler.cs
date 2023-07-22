namespace GameLogic
{
  public interface ITurnHandler
  {
    public void StartTurn();
    public void Undo();
    public void EndTurn();
  }
}
