using System;
using System.Collections;
using System.Collections.Generic;
using SensenToolkit.Lerp;
using UnityEngine;

namespace SensenToolkit.Mathx
{
    public static class Math2Dx
    {
        public static Vector2 ClampVector2Angle(Vector2 vector, float min, float max, bool includeReflection = false)
        {
            (float angle, float length) v = Vector2ToAngleAndLength(vector);
            float angle = Anglex.ClampAngle(v.angle, min, max, includeReflection: includeReflection);
            return AngleAndLengthToVector2(angle, v.length);
        }

        public static Vector2 ScaleToHorizontalLength(Vector2 vector, float length)
        {
            return ScaleToMatchProjectionToTarget(
                vectorToScale: vector,
                targetVector: Vector2.right * length
            );
        }

        private static Vector2 ScaleToMatchProjectionToTarget(Vector2 vectorToScale, Vector2 targetVector)
        {
            Vector2 vectorHead = vectorToScale.normalized;
            float targetLength = targetVector.magnitude;
            float projection = Mathf.Abs(Vector2.Dot(vectorHead, targetVector));
            float lengthNeeded = targetLength * targetLength / projection;

            Vector2 result = vectorHead * lengthNeeded;
            return result;
        }

#region Vector2Conversions
        public static (float angle, float length) Vector2ToAngleAndLength(Vector2 vector)
        {
            float angle = Anglex.NormalizeAngle(Vector2Angle(vector));
            return (angle, vector.magnitude);
        }

        public static Vector2 AngleAndLengthToVector2(float angle, float length)
        {
            return AngleToHeadVector2(angle) * length;
        }

        public static float Vector2Angle(Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        }

        public static Vector2 AngleToHeadVector2(float angle)
        {
            float angleRad = angle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static Vector2 UnityAngleToHeadVector2(float angleRad)
        {
            return AngleToHeadVector2(angleRad * -1f);
        }

        public static Vector2 RotateBy(float angle, Vector2 vector, bool clockwise = false)
        {
            return Quaternion.AngleAxis(angle, clockwise ? Vector3.back : Vector3.forward) * vector;
            // float angleRad = angle * Mathf.Deg2Rad;
            // float cos = Mathf.Cos(angleRad);
            // float sin = Mathf.Sin(angleRad);
            // return new Vector2(
            //     x: (vector.x * cos) - (vector.y * sin),
            //     y: (vector.x * sin) + (vector.x * cos)
            // );
        }

        public static Vector2 NormalForSegment(Vector2 begin, Vector2 end)
        {
            return Math2Dx.RotateBy(90f, end - begin).normalized;
        }

        public static Vector2 VectorRotationLerp(Vector2 v1, Vector2 v2, float t)
        {
            return Vector2RotationLerper.Instance.Lerp(v1, v2, t);
        }
        #endregion
    }
}
