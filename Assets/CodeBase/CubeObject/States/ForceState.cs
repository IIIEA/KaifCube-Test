using System;
using CodeBase.Infrastructure.StateMachine;
using UnityEngine;

namespace CodeBase.CubeObject.CubeObject.States
{
  public class ForceState : IPayloadedState<Vector3>
  {
    private const float CheckStepDistance = 0.1f;

    private readonly float _forceSpeed;
    private readonly float _forceDistance;
    private readonly Transform _cubeTransform;
    private readonly Bounds _bounds;

    private Vector3 _targetPosition;

    public event Action ForceEnded;

    public ForceState(float forceSpeed, float forceDistance, Transform cubeTransform, Bounds bounds)
    {
      _forceSpeed = forceSpeed;
      _forceDistance = forceDistance;
      _cubeTransform = cubeTransform;
      _bounds = bounds;
    }

    public void Enter(Vector3 startPosition)
    {
      _targetPosition = startPosition;
      CalculateSafePosition(startPosition);
      _targetPosition.y = _cubeTransform.position.y;
    }

    public void Exit()
    {
    }

    public void Update()
    {
      _cubeTransform.position = Vector3.Lerp(_cubeTransform.position, _targetPosition, _forceSpeed * Time.deltaTime);
      
      if ((_targetPosition - _cubeTransform.position).magnitude <= 0.01f)
        ForceEnded?.Invoke();
    }

    private void CalculateSafePosition(Vector3 startPosition)
    {
      float step = 0;

      while (step <= _forceDistance)
      {
        if (_bounds.Contains(_targetPosition) == false)
          break;

        _targetPosition = startPosition + _cubeTransform.forward * step;
        step += CheckStepDistance;
      }
    }
  }
}