using CodeBase.Infrastructure.Factory;
using CodeBase.Player;

namespace CodeBase.Spawners
{
  public class ProjectileFactory : Factory<Projectile>
  {
    public Projectile GetProjectileInstance() => 
      GetNewInstance();
  }
}