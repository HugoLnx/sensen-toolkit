using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SensenToolkit.Mathx
{
    public readonly struct Segment
    {
        public Vector2 Begin {get;}
        public Vector2 End {get;}
        public Vector2 Normal {get;}
        public Vector2 Diff {get;}
        public Vector2 DiffAbs {get;}
        public float Length {get;}

        public Segment(Vector2 begin, Vector2 end)
        {
            Begin = begin;
            End = end;
            Normal = Math2Dx.NormalForSegment(begin, end);
            Diff = End - Begin;
            DiffAbs = new Vector2(Mathf.Abs(Diff.x), Mathf.Abs(Diff.y));
            Length = Diff.magnitude;
        }

        public float XtoY(float x)
        {
            return Begin.y + (Diff.y / Diff.x * (x - Begin.x));
        }

        public float YtoX(float y)
        {
            return Begin.x + (Diff.x / Diff.y * (y - Begin.y));
        }

        public override bool Equals(object obj)
        {
            if (obj is not Segment) return false;
            return Equals((Segment) obj);
        }

        public bool Equals(Segment other)
        {
            return Floatx.IsEqual(Begin, other.Begin) && Floatx.IsEqual(End, other.End);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(Begin.x, Begin.y, End.x, End.y);
        }

        public override string ToString()
        {
            return $"Segment({Begin}~{End},Normal:{Normal})";
        }
    }
}
