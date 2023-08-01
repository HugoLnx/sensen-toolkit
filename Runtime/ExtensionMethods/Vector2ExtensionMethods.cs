using SensenToolkit.Mathx;
using UnityEngine;

namespace SensenToolkit
{
    public static class Vector2ExtensionMethods
    {
        public static Vector3 X0Y(this Vector2 v2)
        {
            return new Vector3(v2.x, 0, v2.y);
        }

        public static float Angle(this Vector2 vec)
        {
            return Anglex.Vector2ToAngle(vec);
        }

        public static Vector2 RotateBy(this Vector2 vec, float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);
            return new Vector2(
                x: (vec.x * cos) - (vec.y * sin),
                y: (vec.x * sin) + (vec.y * cos)
            );
        }

        public static Vector2 RotateTowards(
            this Vector2 current, Vector2 target,
            float maxDelta = 0)
        {
            if (maxDelta == 0) return target;
            float angleDiff = Vector2.SignedAngle(current, target);
            float angle = Mathf.MoveTowardsAngle(0f, angleDiff, maxDelta);
            return current.RotateBy(angle);
        }

        public static Vector2 PerpendicularAntiClockwise(this Vector2 v)
        {
            return new Vector2(-v.y, v.x);
        }

        public static Vector2 PerpendicularClockwise(this Vector2 v)
        {
            return -PerpendicularAntiClockwise(v);
        }

        public static Vector2 AsLocalOf(this Vector2 vec, Vector2 refVect)
        {
            refVect = refVect.normalized;
            return new Vector2(
                x: Vector2.Dot(vec, refVect.PerpendicularAntiClockwise()),
                y: Vector2.Dot(vec, refVect)
            );
        }

        public static Vector2 With(this Vector2 vec, float? x = null, float? y = null)
        {
            return new Vector2(
                x: x ?? vec.x,
                y: y ?? vec.y
            );
        }
    }
}
