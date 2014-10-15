using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace com.tinylabproductions.TLPLib.Utilities {
  public class Geometry2D {
    private const float EPS = 0.00001f;
    private const float PI2 = Mathf.PI * 2f;

    public static float CrossProduct(Vector2 v, Vector2 w) {
      return v.x * w.y - v.y * w.x;
    }

    // check to see if the point of intersection lies on a line segment
    public static bool LineSegmentCheck(Vector2 intersection, Vector2 point1, Vector2 point2) {
      bool result = Mathf.Min(point1.x, point2.x) <= intersection.x + EPS && Mathf.Max(point1.x, point2.x) + EPS >= intersection.x &&
                    Mathf.Min(point1.y, point2.y) <= intersection.y + EPS && Mathf.Max(point1.y, point2.y) + EPS >= intersection.y;

      return result;
    }

    public static Vector3 ABC(Vector2 p1, Vector2 p2) {
      var A = p2.y - p1.y;
      var B = p1.x - p2.x;
      var C = A * p1.x + B * p1.y;
      return new Vector3(A, B, C);
    }

    public static Vector2? Intersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
      var a1 = ABC(p1, p2);
      var a2 = ABC(p3, p4);

      float det = (a1.x * a2.y - a2.x * a1.y);

      if (Math.Abs(det) <= EPS) {
        return null;
      }

      var intersection = new Vector2((a2.y * a1.z - a1.y * a2.z) / det,
        (a1.x * a2.z - a2.x * a1.z) / det);
      if (LineSegmentCheck(intersection, p1, p2) && LineSegmentCheck(intersection, p3, p4)) {
        return intersection;
      }
      return null;
    }

    public static float Area(List<Vector2> p) {
      if (p.Count < 3) return 0;
      var res = 0f;
      for (int i = 1; i < p.Count - 1; i++) {
        var va = p[i] - p[0];
        var vb = p[i + 1] - p[0];
        res += va.x * vb.y - va.y * vb.x;
      }
      return -res;
    }

    public static bool PointIsOnSegment(Vector2 p1, Vector2 p2, Vector2 point) {
      if (LineSegmentCheck(point, p1, p2)) {
        var va = p1 - p2;
        var vb = point - p2;
        if (Mathf.Abs(CrossProduct(va, vb)) < EPS && Vector2.Distance(point, p2) > EPS) {
          return true;
        }
      }
      return false;
    }

    public static float PointSegmentDistance(Vector2 p1, Vector2 p2, Vector2 pos) {
      var dist1 = Vector2.Distance(p1, pos);
      var dist2 = Vector2.Distance(p2, pos);
      var dist12 = Vector2.Distance(p1, p2);
      if (dist12 < EPS) return dist1;
      var v12 = p2 - p1;
      var dot = Vector2.Dot(pos - p1, v12) / v12.sqrMagnitude;
      if (dot < 0) return dist1;
      if (dot > 1.0) return dist2;
      var proj = p1 + dot * (v12);
      return (pos - proj).magnitude;
    }

    public static bool ContainsPoint(List<Vector2> points, Vector2 p) {
      int j = points.Count - 1;
      bool inside = false;
      for (int i = 0; i < points.Count; j = i++) {
        if (((points[i].y <= p.y && p.y < points[j].y) || (points[j].y <= p.y && p.y < points[i].y)) &&
            (p.x <
             (points[j].x - points[i].x) * (p.y - points[i].y) / (points[j].y - points[i].y) + points[i].x))
          inside = !inside;
      }
      return inside;
    }

    public static float RandomAngle() {
      return Random.value * 2f * Mathf.PI;
    }

    public static Vector2 AngleToVector(float a) {
      return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }

    public static Vector3 AngleToVector(float a, float z) {
      return new Vector3(Mathf.Cos(a), Mathf.Sin(a), z);
    }

    public static float VectorToAngle(Vector2 v) {
      return Mathf.Atan2(v.y, v.x);
    }

    public static float AngleBetweenVectors(Vector2 a, Vector2 b) {
      return NormalizeRadians(VectorToAngle(a) - VectorToAngle(b));
    }

    public static float NormalizeRadians(float rad) {
      rad %= PI2;
      if (rad < 0) rad += PI2;
      return rad;
    }

    public static float NormalizeRadiansAroundZero(float rad) {
      rad %= PI2;
      if (rad <= -Mathf.PI) rad += PI2;
      if (rad > Mathf.PI) rad -= PI2;
      return rad;
    }

    public static Quaternion AngleToQuaternion(float angle) {
      return Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.forward);
    }

    public static List<Vector2> GenerateCircle(Vector2 center, float radius, int segments = 30) {
      var vec = new List<Vector2>();
      for (int i = 0; i < segments; i++) {
        var angle = 2 * Mathf.PI / segments * i;
        vec.Add(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) + center);
      }
      return vec;
    }

    public static List<Vector2> RemoveDuplicatesInPoly(List<Vector2> points) {
      var v2 = new List<Vector2>();

      for (int i = 0; i < points.Count; i++) {
        if ((points[(i + 1) % points.Count] - points[i]).sqrMagnitude > 0.0001) {
          v2.Add(points[i]);
        }
      }

      return v2;
    }
  }
}