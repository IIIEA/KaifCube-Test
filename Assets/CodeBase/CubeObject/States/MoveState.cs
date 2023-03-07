using System;
using CodeBase.Infrastructure.StateMachine;
using UnityEngine;

namespace CodeBase.CubeObject.CubeObject.States
{
  public class MoveState : IPayloadedState<Vector3>
  {
    private readonly float _speed;
    private readonly Transform _cubeTransform;
    
    private Vector3 _targetPosition;

    public event Action MoveEnded; 
    
    public MoveState(float speed, Transform cubeTransform)
    {
      _speed = speed;
      _cubeTransform = cubeTransform;
    }

    public void Enter(Vector3 targetPosition)
    {
      _targetPosition = targetPosition;
      _targetPosition.y = _cubeTransform.position.y;
    }

    public void Exit()
    {
    }

    public void Update()
    {
      _cubeTransform.position = Vector3.MoveTowards(_cubeTransform.position, _targetPosition, _speed * Time.deltaTime);
      
      if(_cubeTransform.position == _targetPosition)
        MoveEnded?.Invoke();
    }
  }
}