namespace CodeBase.Infrastructure.StateMachine
{
  public interface IState : IUpdatedState
  {
    void Enter();
  }
}