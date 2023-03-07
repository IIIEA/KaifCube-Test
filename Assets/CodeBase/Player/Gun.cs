using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Spawners;
using InputSystem;
using UnityEngine;

namespace CodeBase.Player
{
  public class Gun : MonoBehaviour
  {
    [SerializeField] private ProjectileSpawner _spawner;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private InputHandler _input;
    [Space] 
    [SerializeField] private float _shootDelay;
    [SerializeField] private float _timeToRestoreBullet;
    [SerializeField] private int _maxProjectiles;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _deviation;
    [SerializeField] private LayerMask _enemyMask;

    private Coroutine _restoreRoutine;
    private int _magazineCount;
    private bool _canShoot = true;
    private Camera _camera;
    private List<Projectile> _projectiles = new();

    public int ProjectileCount => _maxProjectiles;
    public float TimeToRestoreBullet => _timeToRestoreBullet;

    public event Action Shooted;
    public event Action BulletRestored;
    public event Action RestoreStarted;

    private void Awake()
    {
      _magazineCount = _maxProjectiles;
      _camera = Camera.main;
      _projectiles = _spawner.SpawnProjectiles(_maxProjectiles);

      foreach (var projectile in _projectiles)
      {
        InitProjectile(projectile);
      }
    }

    private void OnEnable() =>
      _input.ShootPressed += OnShootPressed;

    private void OnDisable() =>
      _input.ShootPressed -= OnShootPressed;

    private void OnShootPressed(Vector3 mousePosition)
    {
      if (_canShoot && _magazineCount > 0)
      {
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 50f)) 
          Shoot(hit.point);
      }
    }

    private void Shoot(Vector3 shootPoint)
    {
      if (TryGetProjectile(out Projectile projectile))
      {
        projectile.Prepare(shootPoint);
        projectile.gameObject.SetActive(true);
        
        StartCoroutine(ShootCooldown());
        
        if(_restoreRoutine != null)
          StopCoroutine(_restoreRoutine);
        
        _restoreRoutine = StartCoroutine(RestoreBullet());
        
        _magazineCount--;
        Shooted?.Invoke();
      }
    }

    private bool TryGetProjectile(out Projectile projectile)
    {
      projectile = _projectiles.Find(p => p.gameObject.activeSelf == false);

      return projectile;
    }

    private void InitProjectile(Projectile projectile) =>
      projectile.Init(_projectileSpeed, _spawnPoint.position, _deviation, _enemyMask);

    private IEnumerator ShootCooldown()
    {
      _canShoot = false;

      var delay = new WaitForSeconds(_shootDelay);

      yield return delay;
      
      _canShoot = true;
    }

    private IEnumerator RestoreBullet()
    {
      var delay = new WaitForSeconds(_timeToRestoreBullet);

      RestoreStarted?.Invoke();
      
      yield return delay;

      _magazineCount++;
      _magazineCount = Mathf.Clamp(_magazineCount, _magazineCount, _maxProjectiles);
      BulletRestored?.Invoke();

      if (_magazineCount < _maxProjectiles)
        StartCoroutine(RestoreBullet());
    }
  }
}