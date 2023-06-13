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
            return Vector2.SignedAngle(Vector2.up, vec);
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
            float maxRadiansDelta = float.MaxValue,

            // Preferred rotation direction when vectors are opposites
            Vector2? preferredDirection = null)
        {
            float maxDelta = maxRadiansDelta * Mathf.Rad2Deg;
            float targetAngle = Vector2.SignedAngle(current, target);
            if (preferredDirection.HasValue && Mathf.Abs(targetAngle) >= 179.999f)
            {
                float preferredSign = Mathf.Sign(Vector2.SignedAngle(current, preferredDirection.Value));
                targetAngle = preferredSign * 179.999f;
            }
            float angle = Mathf.MoveTowardsAngle(0f, targetAngle, maxDelta);
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
    }
}
