namespace CodeBase.Extensions
{
  using UnityEngine;

  public static class MathBezier
  {
    public static Vector3 GetMiddlePoint(Vector3 startPoint, Vector3 endPoint, float height = 0)
    {
      float middlePointRange = Vector3.Distance(startPoint, endPoint) / 2;

      Vector3 middlePoint = startPoint + middlePointRange / (startPoint - endPoint).magnitude * (endPoint - startPoint);

      Vector3 targetPoint = new Vector3(middlePoint.x, middlePoint.y + height, middlePoint.z);

      return targetPoint;
    }

    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
      t = Mathf.Clamp01(t);
      float oneMinusT = 1f - t;

      return
        oneMinusT * oneMinusT * oneMinusT * p0 +
        3f * oneMinusT * oneMinusT * t * p1 +
        3f * oneMinusT * t * t * p2 +
        t * t * t * p3;
    }

    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
      t = Mathf.Clamp01(t);
      float oneMinusT = 1f - t;

      return
        3f * oneMinusT * oneMinusT * (p1 - p0) +
        6f * oneMinusT * t * (p2 - p1) +
        3f * t * t * (p3 - p2);
    }

    public static Vector3 GetPoint(Vector3 startPosition, Vector3 auxiliaryPoint, Vector3 endPoint, float t)
    {
      //B(t) = (1-t)2P0 + 2(1-t)tP1 + t2P2
      t = Mathf.Clamp01(t);

      float u = 1 - t;
      Vector3 currentPoint = (u * u * startPosition) + (2 * u * t * auxiliaryPoint) + (t * t * endPoint);

      return currentPoint;
    }

    public static float CalculateBezierLength(Vector3 startPosition, Vector3 auxiliaryPoint, Vector3 endPoint,
      float steps = 10f)
    {
      if (steps == 0)
        return Mathf.Epsilon;

      float step = 1f / steps;
      step = Mathf.Clamp01(step);
      float t = 0;

      Vector3[] points = new Vector3[(int)steps];

      for (int i = 0; i < points.Length; i++)
      {
        var u = 1 - t;
        points[i] = (u * u * startPosition) + (2 * u * t * auxiliaryPoint) + (t * t * endPoint);

        t += step;
      }

      float lenght = 0;
      
      for (int i = 1; i < points.Length; i++)
      {
        var distance = Vector3.Distance(points[i - 1], points[i]);
        lenght += distance;
      }

      return lenght;
    }
  }
}