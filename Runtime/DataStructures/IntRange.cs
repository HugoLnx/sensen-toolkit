using System;
using SensenToolkit.Mathx;

namespace SensenToolkit.DataStructures
{
    public readonly struct IntRange : IEquatable<IntRange>
    {
        public int Begin { get; }
        public int End { get; }
        public int Min {get;}
        public int Max {get;}
        public bool IsDescending {get;}
        public bool IsAscending {get;}
        public bool IsStationary {get;}
        public float Middle {get;}
        public int Length {get;}

        public IntRange(int begin, int end)
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

        public bool Intersects(IntRange range)
        {
            return Contains(range.Begin) || Contains(range.End) || IsContainedBy(range);
        }

        public bool Contains(int val)
        {
            return val >= Min && val <= Max;
        }

        public bool Contains(IntRange range)
        {
            return Min <= range.Min && Max >= range.Max;
        }

        public bool IsContainedBy(IntRange range)
        {
            return range.Contains(this);
        }

        public Range Shrink(int endBy = 0, int beginBy = 0)
        {
            return new Range(
                begin: Begin + beginBy,
                end: End - endBy
            );
        }

        public override bool Equals(object obj)
        {
            if (obj is not IntRange) return false;
            return Equals((IntRange) obj);
        }

        public bool Equals(IntRange other)
        {
            return other.Begin == Begin && other.End == End;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Begin, End);
        }

        public static bool operator ==(IntRange left, IntRange right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IntRange left, IntRange right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"IntRange({Begin}~{End})";
        }

        private static void SwapVariables<T>(ref T v1, ref T v2)
        {
            (v1, v2) = (v2, v1);
        }
    }
}
