using System.Collections;
using System.Collections.Generic;
using SensenToolkit.DataStructures.RangeBinaryTree;
using SensenToolkit.Mathx;
using UnityEngine;

namespace SensenToolkit.TwoDimensions
{
    public class EdgeColliderIndexFactory
    {
        public EdgeColliderIndex BuildHorizontalIndex(EdgeCollider2D collider)
        {
            Vector2[] points = collider.points;
            int segmentsCount = collider.pointCount - 1;
            List<EdgeColliderIndexEntry> entries = new(segmentsCount + 2);
            if (collider.useAdjacentStartPoint)
            {
                entries.Add(BuildEntryForAdjacent(collider.adjacentStartPoint, points[0], inx: 0));
            }
            entries.AddRange(BuildEntriesForPoints(points, startInx: entries.Count));
            if (collider.useAdjacentEndPoint)
            {
                entries.Add(BuildEntryForAdjacent(points[^1], collider.adjacentEndPoint, inx: entries.Count));
            }
            List<RBLeafData<EdgeColliderIndexEntry?>> leaves = new();
            foreach (EdgeColliderIndexEntry entry in entries)
            {
                if (!entry.Segment.HasValue) continue;
                Segment seg = entry.Segment.Value;
                leaves.Add(new RBLeafData<EdgeColliderIndexEntry?>(
                    range: new DataStructures.Range(seg.Begin.x, seg.End.x),
                    content: entry
                ));
            }
            RBTree<EdgeColliderIndexEntry?> searchTree = RBTree.FromLeaves(leaves);
            return new EdgeColliderIndex(entries.ToArray(), searchTree);
        }

        private IEnumerable<EdgeColliderIndexEntry> BuildEntriesForPoints(Vector2[] points, int startInx)
        {
            for (int i = 0; i < points.Length-1; i++)
            {
                Vector2 pt1 = points[i];
                Vector2 pt2 = points[i + 1];
                Segment segment = new(pt1, pt2);
                yield return new EdgeColliderIndexEntry() {
                    Segment = segment,
                    Normal = segment.Normal,
                    Inx = startInx + i
                };
            }
        }

        private static EdgeColliderIndexEntry BuildEntryForAdjacent(Vector2 pt1, Vector2 pt2, int inx)
        {
            return new EdgeColliderIndexEntry() {
                Segment = null,
                Normal = Math2Dx.NormalForSegment(pt1, pt2),
                Inx = inx
            };
        }
    }
}
