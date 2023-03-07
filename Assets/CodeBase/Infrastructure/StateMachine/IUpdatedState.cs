namespace CodeBase.Infrastructure.StateMachine
{
  public interface IUpdatedState
  {
    void Update();
    void Exit();
  }
}