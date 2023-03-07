using CodeBase.CubeObject.CubeObject.States;
using CodeBase.CubeObject.Infrastructure.DependencyInjector;
using CodeBase.GroundObject;
using CodeBase.Infrastructure.StateMachine;
using UnityEngine;

namespace CodeBase.CubeObject.CubeObject
{
  public class CubeMover : MonoBehaviour, IDependency<IGroundField>
  {
    [SerializeField] private float _speed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _forceSpeed;
    [SerializeField] private float _forceDistance;
    [SerializeField] private int _minForceTimeDelay = 3;
    [SerializeField] private int _maxForceTimeDelay = 6;

    private IGroundField _field;
    private StateMachine _stateMachine;
    private Vector3 _nextPosition;
    private float _forceTimer;
    private bool _inForceMode;

    private void Awake()
    {
      _stateMachine = new StateMachine();
      _forceTimer = Random.Range(_maxForceTimeDelay, _maxForceTimeDelay);
    }

    private void Start()
    {
      InitStates();

      _stateMachine.Enter<MoveState, Vector3>(transform.position);
      _nextPosition = GetNewDestination();
    }

    private void Update()
    {
      _stateMachine.Update();

      if (_forceTimer <= 0)
      {
        _nextPosition = GetNewDestination();
        _stateMachine.Enter<ForceState, Vector3>(new Vector3(transform.position.x, _field.Bounds.max.y,
          transform.position.z));
        _inForceMode = true;
        _forceTimer = Random.Range(_minForceTimeDelay, _maxForceTimeDelay);
      }

      if (_inForceMode == false)
        _forceTimer -= Time.deltaTime;
    }

    private void OnDisable()
    {
      _inForceMode = false;
      _forceTimer = Random.Range(_maxForceTimeDelay, _maxForceTimeDelay);
      
      _nextPosition = GetNewDestination();
      SetNextState(_nextPosition);
    }

    public void Inject(IGroundField field) => 
      _field = field;

    private Vector3 GetNewDestination()
    {
      var boundsMax = _field.Bounds.max;
      var boundsMin = _field.Bounds.min;

      float nextPositionX = Random.Range(boundsMin.x, boundsMax.x);
      float nextYPosition = transform.position.y;
      float nextZPosition = Random.Range(boundsMin.z, boundsMax.z);

      var targetPosition = new Vector3(nextPositionX, nextYPosition, nextZPosition);

      return targetPosition;
    }

    private void SetNextState(Vector3 targetPosition)
    {
      if (Vector3.Angle(transform.position, targetPosition) > Mathf.Epsilon)
      {
        var direction = (targetPosition - transform.position).normalized;
        _stateMachine.Enter<RotateState, Vector3>(direction);
      }
      else
      {
        _stateMachine.Enter<MoveState, Vector3>(targetPosition);
      }
    }

    private void InitStates()
    {
      MoveState moveState = new MoveState(_speed, transform);
      moveState.MoveEnded += OnMoveEnded;
      _stateMachine.AddState(moveState);

      RotateState rotateState = new RotateState(_rotateSpeed, transform);
      rotateState.RotateEnded += OnRotateEnded;
      _stateMachine.AddState(rotateState);

      ForceState forceState = new ForceState(_forceSpeed, _forceDistance, transform, _field.Bounds);
      forceState.ForceEnded += OnForceEnded;
      _stateMachine.AddState(forceState);
    }

    private void OnMoveEnded()
    {
      _nextPosition = GetNewDestination();

      SetNextState(_nextPosition);
    }

    private void OnRotateEnded()
    {
      _stateMachine.Enter<MoveState, Vector3>(_nextPosition);
    }

    private void OnForceEnded()
    {
      _inForceMode = false;
      _nextPosition = GetNewDestination();

      SetNextState(_nextPosition);
    }
  }
}