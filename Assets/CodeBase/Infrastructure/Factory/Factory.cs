using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public abstract class Factory<T> : MonoBehaviour, IFactory<T> where T : MonoBehaviour
  {
    [SerializeField] private T _prefab;

    public T GetNewInstance() => 
      Instantiate(_prefab, transform);
  }
}