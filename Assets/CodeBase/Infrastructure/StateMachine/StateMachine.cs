using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure.StateMachine
{
  public class StateMachine
  {
    private readonly Dictionary<Type, IUpdatedState> _states;
    private IUpdatedState _activeState;

    public StateMachine()
    {
      _states = new Dictionary<Type, IUpdatedState>();
    }

    public void AddState<TState>(TState state) where TState : class, IUpdatedState
    {
      _states.Add(typeof(TState), state);
    }
    
    public void Enter<TState>() where TState : class, IState
    {
      IState state = ChangeState<TState>();
      state.Enter();
    }
    
    public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
    {
      TState state = ChangeState<TState>();
      state.Enter(payload);
    }

    public void Update()
    {
      _activeState?.Update();
    }

    private TState ChangeState<TState>() where TState : class, IUpdatedState
    {
      _activeState?.Exit();

      TState state = GetState<TState>();
      _activeState = state;

      return state;
    }

    private TState GetState<TState>() where TState : class, IUpdatedState =>
      _states[typeof(TState)] as TState;
  }
}