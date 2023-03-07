namespace CodeBase.Infrastructure.Factory
{
  public interface IFactory<T>
  {
    T GetNewInstance();
  }
}