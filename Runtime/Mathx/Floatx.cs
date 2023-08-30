using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SensenToolkit.Mathx
{
    public static class Floatx
    {
        public const float DefaultPrecision = 1e-6f;
        public static bool IsUsual(float f)
        {
            return !float.IsNaN(f) && float.IsFinite(f);
        }
        public static bool IsEqual(float f1, float f2, float precision=DefaultPrecision)
        {
            float diff = f1 - f2;
            return diff * diff <= precision * precision;
        }

        public static bool IsNotEqual(float f1, float f2, float precision=DefaultPrecision)
        {
            return !IsEqual(f1, f2, precision);
        }

        public static bool IsGreater(float f1, float f2, float precision=DefaultPrecision)
        {
            return f1 - f2 >= precision;
        }

        public static bool IsLess(float f1, float f2, float precision=DefaultPrecision)
        {
            return f1 - f2 <= precision;
        }

        public static bool IsGreaterOrEqual(float f1, float f2, float precision=DefaultPrecision)
        {
            return IsGreater(f1, f2, precision) || IsEqual(f1, f2, precision);
        }

        public static bool IsLessOrEqual(float f1, float f2, float precision=DefaultPrecision)
        {
            return IsLess(f1, f2, precision) || IsEqual(f1, f2, precision);
        }

        public static bool IsZero(float val) => IsEqual(val, 0f);
        public static bool IsNotZero(float val) => IsNotEqual(val, 0f);

        public static bool IsBetween(float val, float min, float max)
        {
            return IsGreaterOrEqual(val, min) && IsLessOrEqual(val, max);
        }

        public static bool IsEqual(Vector2 v1, Vector2 v2, float precision=DefaultPrecision)
        {
            return IsEqual(v1.x, v2.x, precision) && IsEqual(v1.y, v2.y, precision);
        }

        public static bool IsEqual(Vector3 v1, Vector3 v2, float precision=DefaultPrecision)
        {
            return IsEqual(v1.x, v2.x, precision) && IsEqual(v1.y, v2.y, precision) && IsEqual(v1.z, v2.z, precision);
        }

        public static bool IsPositive(float interval, float precision=DefaultPrecision)
        {
            return IsGreater(interval, 0, precision);
        }

        public static bool IsNegative(float interval, float precision=DefaultPrecision)
        {
            return IsLess(interval, 0, precision);
        }

        public static (int count, float decimalPart) Split(float v)
        {
            int count = Mathf.FloorToInt(v);
            return (count, v - count);
        }
    }
}
