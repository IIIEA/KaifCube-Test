using UnityEngine;

namespace CodeBase.GroundObject
{
  [RequireComponent(typeof(Collider))]
  public class Field : MonoBehaviour, IGroundField
  {
    [SerializeField] private Collider _collider;

    public Bounds Bounds => _collider.bounds;
  }
}
