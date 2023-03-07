using System;
using CodeBase.CubeObject.CubeObject;
using CodeBase.Extensions;
using UnityEngine;

namespace CodeBase.Player
{
  public class Projectile : MonoBehaviour
  {
    [SerializeField] private float _explodeRadius;
    [SerializeField] private float _yOffset;
    
    private float _speed;
    private Vector3 _startPosition;
    private Vector3 _auxiliaryPoint;
    private Vector3 _endPosition;
    private LayerMask _enemyMask;
    private float _deviation;
    private float _distanceToEndPoint;
    private float _travelDistance;
    private bool _isTargetReached = false;

    private void Update()
    {
      Move();

      if (_isTargetReached)
      {
        Explode();
        _isTargetReached = false;
      }
    }

    public void Init(float speed, Vector3 startPosition, float deviation, LayerMask enemyMask)
    {
      _deviation = deviation;
      _enemyMask = enemyMask;
      _startPosition = startPosition;
      _speed = speed;
    }

    public void Prepare(Vector3 endPosition)
    {
      _endPosition = endPosition;
      _endPosition.y -= _yOffset;
      _isTargetReached = false;
      _auxiliaryPoint = MathBezier.GetMiddlePoint(_startPosition, _endPosition, _deviation);
      _distanceToEndPoint = MathBezier.CalculateBezierLength(_startPosition, _auxiliaryPoint, _endPosition);
      _travelDistance = 0;
      transform.position = _startPosition;
    }

    private void Move()
    {
      if (_travelDistance <= _distanceToEndPoint)
      {
        var nextPoint =
          MathBezier.GetPoint(_startPosition, _auxiliaryPoint, _endPosition,
            _travelDistance / _distanceToEndPoint);

        _travelDistance += _speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, nextPoint, _speed * Time.deltaTime);
      }
      else
      {
        _isTargetReached = true;
      }
    }

    private void Explode()
    {
      var hitColliders = Physics.OverlapSphere(transform.position, _explodeRadius, _enemyMask);

      foreach (var collider in hitColliders)
      {
        if (collider.TryGetComponent(out IDamageable damageable))
          damageable.TakeDamage();
      }

      gameObject.SetActive(false);
    }
  }
}