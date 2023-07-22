using Zenject;
namespace InputHandler
{
  public interface IState
  {
    public void Enter();
    public void LogicUpdate();
    public void Exit();

  } 
}

