using CodeBase.CubeObject.CubeObject;
using CodeBase.Infrastructure.Factory;

namespace CodeBase.Spawners
{
  public class CubeFactory : Factory<Cube>
  {
    public Cube GetCubeInstance() => 
      GetNewInstance();
  }
}
