using System;
using System.Collections;
using System.Collections.Generic;
using SensenToolkit.Mathx;
using UnityEngine;
using SensenToolkit.DataStructures.RangeBinaryTree;
using System.Linq;

namespace SensenToolkit.TwoDimensions
{
    public struct EdgeColliderIndexEntry
    {
        public Segment? Segment { get; set; }
        public Vector2 Normal { get; set; }
        public int Inx {get; set;}
        public bool IsAdjacent => !Segment.HasValue;
    }


    public class EdgeColliderIndex
    {
        public DataStructures.Range XRange { get; }
        private EdgeColliderIndexEntry[] _entries;
        private RBTree<EdgeColliderIndexEntry?> _index;

        public EdgeColliderIndex(EdgeColliderIndexEntry[] entries, RBTree<EdgeColliderIndexEntry?> index)
        {
            _entries = entries;
            _index = index;
            XRange = GetXRangeFrom(entries);
        }

        public EdgeColliderIndexEntry? SearchByX(float x)
        {
            return _index.Lookup(x);
        }

        public EdgeColliderIndexEntry? SearchByInx(int inx)
        {
            if (inx < 0 || inx >= _entries.Length)
            {
                return null;
            }
            return _entries[inx];
        }

        private static DataStructures.Range GetXRangeFrom(EdgeColliderIndexEntry[] entries)
        {
            Segment first = entries.First(e => e.Segment.HasValue).Segment.Value;
            Segment last = entries.Last(e => e.Segment.HasValue).Segment.Value;
            return new DataStructures.Range(
                begin: first.Begin.x,
                end: last.End.x
            );
        }
    }
}
