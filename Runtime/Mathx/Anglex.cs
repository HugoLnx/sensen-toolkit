using System;
using UnityEngine;

namespace SensenToolkit
{
    public static class Anglex
    {
        public static bool AngleIsBetween(float degrees, float min, float max)
        {
            float rangeSize = max - min;
            if (rangeSize * rangeSize >= 360f * 360f) return true;
            degrees = NormalizeAngle(degrees - min);
            float maxNorm = NormalizeAngle(max - min);
            return degrees <= maxNorm;
        }

        public static float ClampAngle(float degrees, float min, float max, bool normalize = true, bool includeReflection = false)
        {
            // TODO: Improve it by using Modular Arithmetic
            float reflectionMinDegrees = min + 180f;
            float reflectionMaxDegrees = max + 180f;
            if (AngleIsBetween(degrees, min, max) || (includeReflection && AngleIsBetween(degrees, reflectionMinDegrees, reflectionMaxDegrees)))
            {
                return normalize ? NormalizeAngle(degrees) : RelocateAngleToRange(degrees, min, max);
            }

            float degreesRelative = NormalizeAngle(degrees - min);
            float maxRelative = NormalizeAngle(max - min);
            float distanceToReflection = 180f - maxRelative;
            float middleBetweenReflectionAndRange = maxRelative + (distanceToReflection / 2f);

            float relativeResult;
            if (includeReflection && degreesRelative > middleBetweenReflectionAndRange
                && degreesRelative < middleBetweenReflectionAndRange + 180f)
            {
                relativeResult = degreesRelative < 180f ? 180f : maxRelative + 180f;
            }
            else
            {
                relativeResult = degreesRelative <= 180f + (maxRelative / 2f) ? maxRelative : 0f;
            }
            float result = min + relativeResult;
            return normalize ? NormalizeAngle(result) : RelocateAngleToRange(result, min, max);
        }

        public static float RelocateAngleToRange(float degrees, float min, float max)
        {
            degrees = NormalizeAngle(degrees);
            if (max < 0 && degrees >= NormalizeAngle(max))
            {
                return max + NormalizeAngle(degrees - max);
            }
            else
            {
                return min + NormalizeAngle(degrees - min);
            }
        }

        public static float SignedDistance(float from, float to)
        {
            from = NormalizeAngle(from);
            to = NormalizeAngle(to);
            float cwDistance = to - from;
            if (Mathf.Abs(cwDistance) <= 180f)
            {
                return cwDistance;
            }
            else
            {
                return 360f - cwDistance;
            }
        }

        public static float NormalizeAngle(float degrees, float fullAngle = 360f)
        {
            return ((degrees % fullAngle) + fullAngle) % fullAngle;
        }

        public static Vector2 RotationToDirection2D(float angle, float initial = 0)
        {
            return Anglex.AngleToVector2(angle + initial);
        }

        public static float Direction2DToRotation(Vector2 direction, float initial = 0)
        {
            return direction.Angle() - initial;
        }

        public static float Vector2ToAngle(Vector2 vec)
        {
            return Vector2.SignedAngle(Vector2.right, vec);
        }

        public static Vector2 AngleToVector2(float angle)
        {
            return Math2Dx.RotateBy(angle, Vector2.right);
        }

        public static float FlatSignedAngle(Vector3 from, Vector3 to, Vector3 axis)
        {
            Assertx.IsNotZero(axis);
            Assertx.IsNormalized(axis);
            Vector3 fromPlaneVector = from - Vector3.Project(from, axis);
            Vector3 toPlaneVector = to - Vector3.Project(to, axis);
            return Vector3.SignedAngle(fromPlaneVector, toPlaneVector, axis);
        }

        public static float AngleForQuaternionRelatedToAxis(Quaternion quaternion, Vector3 axis)
        {
            if (quaternion == Quaternion.identity) return 0f;
            Assertx.IsNotZero(axis);
            Assertx.IsNormalized(axis);
            // quaternion.ToAngleAxis(out float angle, out Vector3 quatAxis);
            // float axesAngle = Vector3.Angle(quatAxis, axis);
            // if (Mathf.Approximately(axesAngle, 0f)) return angle;
            // if (Mathf.Approximately(axesAngle, 180f)) return -angle;
            // var vec = Vector3.Cross(axis, quatAxis);
            var vec = Vector3.Cross(axis, Mathf.Approximately(Mathf.Abs(axis.z), 1f) ? Vector3.up : Vector3.forward);
            return FlatSignedAngle(vec, quaternion * vec, axis);
        }
    }
}
