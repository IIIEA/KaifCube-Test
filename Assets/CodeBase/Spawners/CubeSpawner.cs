using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.CubeObject.CubeObject;
using CodeBase.CubeObject.Infrastructure.DependencyInjector;
using CodeBase.Extensions;
using CodeBase.GroundObject;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Spawners
{
  [RequireComponent(typeof(CubeFactory))]
  public class CubeSpawner : MonoBehaviour
  {
    [SerializeField] private Field _field;
    [SerializeField] private int _maxSpawnCount;
    [SerializeField] private int _minTimeToRestore;
    [SerializeField] private int _maxTimeToRestore;
    [SerializeField] private float _ySpawnOffset;
    
    private List<Cube> _cubes = new();
    private CubeFactory _factory;
    private IGroundField _groundField;

    public event Action CubeDestroyed;
    
    private void OnValidate()
    {
      if (_minTimeToRestore > _maxTimeToRestore)
        _minTimeToRestore = _maxTimeToRestore;
    }

    private void Awake()
    {
      _groundField = _field.GetComponent<IGroundField>();
      _factory = GetComponent<CubeFactory>();
    }

    private void Start() => 
      SpawnCubes(_maxSpawnCount);

    private void OnDestroy()
    {
      foreach (var cube in _cubes)
      {
        if (cube != null)
          cube.Destroyed -= OnCubeDestroyed;
      }
    }

    private Vector3 GetRandomSpawnPoint()
    {
      var spawnPoint = BoundsExtension.GetRandomPointOnBounds(_groundField.Bounds);
      spawnPoint.y += _ySpawnOffset;
      
      return spawnPoint;
    }

    private void SpawnCubes(int count)
    {
      for (int i = 0; i < count; i++)
      {
        var cube = _factory.GetCubeInstance();
        cube.transform.position = GetRandomSpawnPoint();
        cube.Destroyed += OnCubeDestroyed;
        _cubes.Add(cube);
        
        var cubeDependency = cube.GetComponent<IDependency<IGroundField>>();
        cubeDependency.Inject(_groundField);
      }
    }

    private void OnCubeDestroyed()
    {
      CubeDestroyed.Invoke();
      StartCoroutine(RestoreCubeRoutine());
    }

    private void RestoreCube()
    {
      var destroyedCube = _cubes.Find(cube => cube.gameObject.activeSelf == false);
      
      if(destroyedCube == null)
        return;
      
      destroyedCube.transform.position = GetRandomSpawnPoint();
      destroyedCube.gameObject.SetActive(true);
    }

    private IEnumerator RestoreCubeRoutine()
    {
      var delayTime = Random.Range(_minTimeToRestore, _maxTimeToRestore);
      var delay = new WaitForSeconds(delayTime);

      yield return delay;
      
      RestoreCube();
    }
  }
}