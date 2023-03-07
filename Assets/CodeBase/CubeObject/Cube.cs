using System;
using UnityEngine;

namespace CodeBase.CubeObject.CubeObject
{
  public class Cube : MonoBehaviour, IDamageable
  {
    public event Action Destroyed;
    
    public void TakeDamage()
    {
      gameObject.SetActive(false);
      Destroyed?.Invoke();
    }
  }
}