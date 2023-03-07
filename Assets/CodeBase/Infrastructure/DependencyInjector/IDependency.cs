namespace CodeBase.CubeObject.Infrastructure.DependencyInjector
{
  public interface IDependency<T> where T : class
  {
    void Inject(T dependency);
  }
}