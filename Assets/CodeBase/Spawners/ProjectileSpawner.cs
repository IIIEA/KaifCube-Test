using System.Collections.Generic;
using CodeBase.Player;
using UnityEngine;

namespace CodeBase.Spawners
{
  [RequireComponent(typeof(ProjectileFactory))]
  public class ProjectileSpawner : MonoBehaviour
  {
    private ProjectileFactory _factory;

    private void Awake()
    {
      _factory = GetComponent<ProjectileFactory>();
    }

    public List<Projectile> SpawnProjectiles(int count)
    {
      List<Projectile> projectiles = new List<Projectile>();
      
      for (int i = 0; i < count; i++)
      {
        var projectile = _factory.GetProjectileInstance();
        projectile.gameObject.SetActive(false);
          
        projectiles.Add(projectile);
      }

      return projectiles;
    }
  }
}