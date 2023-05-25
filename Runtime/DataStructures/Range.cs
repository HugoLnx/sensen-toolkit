using System;
using System.Collections.Generic;
using SensenToolkit.Mathx;
using UnityEngine;

namespace SensenToolkit.DataStructures
{
    public readonly struct Range : IEquatable<Range>
    {
        private const float DefaultPrecision = 10e-6f;
        public float Begin { get; }
        public float End { get; }
        public float Min {get;}
        public float Max {get;}
        public bool IsDescending {get;}
        public bool IsAscending {get;}
        public bool IsStationary {get;}
        public float Middle {get;}
        public float Length {get;}


        public Range(float begin, float end)
        {
            Begin = begin;
            End = end;
            Middle = (Begin + End) / 2f;
            IsAscending = Floatx.IsLess(Begin, End);
            IsDescending = Floatx.IsGreater(Begin, End);
            IsStationary = Floatx.IsEqual(Begin, End);
            (Min, Max) = IsDescending ? (End, Begin) : (Begin, End);

            Length = Max - Min;
        }

        public bool Intersects(Range range, float precision=DefaultPrecision)
        {
            return Contains(range.Begin, precision) || Contains(range.End, precision) || IsContainedBy(range, precision);
        }

        public bool Contains(float val, float precision=DefaultPrecision)
        {
            return Floatx.IsGreaterOrEqual(val, Min, precision)
                && Floatx.IsLessOrEqual(val, Max, precision);
        }

        public bool Contains(Range range, float precision=DefaultPrecision)
        {
            return Floatx.IsLessOrEqual(Min, range.Min, precision)
                && Floatx.IsGreaterOrEqual(Max, range.Max, precision);
        }

        public bool IsContainedBy(Range range, float precision=DefaultPrecision)
        {
            return range.Contains(this, precision);
        }

        public Range Shrink(float endBy = 0f, float beginBy = 0f)
        {
            return new Range(
                begin: Begin + beginBy,
                end: End - endBy
            );
        }

        public override bool Equals(object obj)
        {
            if (obj is not Range) return false;
            return Equals((Range) obj);
        }

        public bool Equals(Range other)
        {
            return Floatx.IsEqual(other.Begin, Begin) && Floatx.IsEqual(other.End, End);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Begin, End);
        }

        public static bool operator ==(Range left, Range right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Range left, Range right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"Range({Begin}~{End})";
        }

        public IEnumerable<float> Each(float interval, bool alwaysIncludeLimits=false)
        {
            if (Floatx.IsZero(interval))
            {
                throw new ArgumentException("interval can not be 0");
            }

            (float begin, float end) = (Begin, End);
            if (Floatx.IsNegative(interval)) SwapVariables(ref begin, ref end);

            float absInterval = Mathf.Abs(interval);
            float signedInterval = absInterval * Mathf.Sign(end - begin);

            (int count, float decimalPart) = Floatx.Split(Mathf.Abs(end - begin) / absInterval);
            bool rangeIsMultipleOfInterval = Floatx.IsZero(decimalPart);
            for (int i = 0; i <= count; i++)
            {
                yield return begin + (i * signedInterval);
            }

            if (alwaysIncludeLimits && !rangeIsMultipleOfInterval)
            {
                yield return end;
            }
        }

        private static void SwapVariables<T>(ref T v1, ref T v2)
        {
            (v1, v2) = (v2, v1);
        }
    }
}
