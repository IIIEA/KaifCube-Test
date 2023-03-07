using System;
using CodeBase.Infrastructure.StateMachine;
using UnityEngine;

namespace CodeBase.CubeObject.CubeObject.States
{
  public class RotateState : IPayloadedState<Vector3>
  {
    private readonly float _rotateSpeed;
    private readonly Transform _cubeTransform;

    private Vector3 _direction;

    public event Action RotateEnded;
    
    public RotateState(float rotateSpeed, Transform cubeTransform)
    {
      _rotateSpeed = rotateSpeed;
      _cubeTransform = cubeTransform;
    }

    public void Enter(Vector3 direction)
    {
      _direction = direction;
    }

    public void Exit()
    {

    }

    public void Update()
    {
      Quaternion rotation = Quaternion.LookRotation(_direction);
      _cubeTransform.rotation = Quaternion.RotateTowards(_cubeTransform.rotation, rotation, _rotateSpeed * Time.deltaTime);
      
      if(Vector3.Angle(_cubeTransform.forward, _direction) <= Mathf.Epsilon)
        RotateEnded?.Invoke();
    }
  }
}