using UnityEngine;

namespace CodeBase.Extensions
{
  public class BoundsExtension
  {
    public static Vector3 GetRandomPointOnBounds(Bounds bounds)
    {
      var pointX = Random.Range(bounds.min.x, bounds.max.x);
      var pointY = bounds.max.y;
      var pointZ = Random.Range(bounds.min.z, bounds.max.z);

      return new Vector3(pointX, pointY, pointZ);
    }
  }
}