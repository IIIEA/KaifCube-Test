namespace CodeBase.Infrastructure.StateMachine
{
  public interface IPayloadedState<TPayloaded> : IUpdatedState
  {
    void Enter(TPayloaded payloaded);
    void Update();
  }
}