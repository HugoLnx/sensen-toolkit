using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using SensenToolkit;
using System.Linq;

public class BinarySearchRangeTreeTests
{
    [Test]
    public void Lookup_ReturnsTheContent_WhenTargetValueIsIncludedInALeafRange()
    {
        List<Segment> segments = BuildChainedSegments(
            begin: Vector2.zero,
            chain: new (bool addToList, Vector2 vector)[] {
                (addToList: true , vector: Vector2.one * 2f),
                (addToList: true , vector: new Vector2(1f, -1f) * 5f),
                (addToList: true , vector: Vector2.right * 3f),
            }
        );
        RBTree<Segment?> tree = BuildTreeFromSegments(segments);

        Assert.That(tree.Lookup(1f).Value, Is.EqualTo(segments[0]));
        Assert.That(tree.Lookup(3f).Value, Is.EqualTo(segments[1]));
        Assert.That(tree.Lookup(4f).Value, Is.EqualTo(segments[1]));
        Assert.That(tree.Lookup(8f), Is.EqualTo(segments[2]));
    }

    [Test]
    public void Lookup_ReturnsDefaultValue_WhenTargetValueIsAroundTheLeafRanges()
    {
        List<Segment> segments = BuildChainedSegments(
            begin: Vector2.zero,
            chain: new (bool addToList, Vector2 vector)[] {
                (addToList: true , vector: Vector2.one * 2f),
                (addToList: true , vector: new Vector2(1f, -1f) * 5f),
                (addToList: true , vector: Vector2.right * 3f),
            }
        );
        RBTree<Segment?> tree = BuildTreeFromSegments(segments);

        Assert.That(tree.Lookup(-1f), Is.Null);
        Assert.That(tree.Lookup(11f), Is.Null);
    }


    [Test]
    public void Lookup_ReturnsDefaultValue_WhenTargetValueIsInAHoleBetweenLeafRanges()
    {
        List<Segment> segments = BuildChainedSegments(
            begin: Vector2.zero,
            chain: new (bool addToList, Vector2 vector)[] {
                (addToList: true , vector: Vector2.one * 2f),
                (addToList: true , vector: new Vector2(1f, -1f) * 5f),
                (addToList: false, vector: new Vector2(1f, -1f) * 3f),
                (addToList: true , vector: Vector2.right * 3f),
            }
        );
        RBTree<Segment?> tree = BuildTreeFromSegments(segments);

        Assert.That(tree.Lookup(7.1f), Is.Null);
        Assert.That(tree.Lookup(8f), Is.Null);
        Assert.That(tree.Lookup(9.9f), Is.Null);
    }

    [Test]
    public void Build_FailsIfLeafSegmentsIntersectEachOther()
    {
        List<Vector2> points = new();
        points.Add(Vector2.zero);
        points.Add(points[^1] + (Vector2.one * 2f));
        points.Add(points[^1] + (new Vector2(1f, -1f) * 5f));
        points.Add(points[^1] + (Vector2.right * 3f));
        List<Segment> segments = new() {
            new Segment(points[0], points[1]),
            new Segment(points[1], points[2]),
            new Segment(points[2], points[3])
        };
        RBLeafData<Segment>[] leaves = new[] {
            new RBLeafData<Segment>(new Range(begin: segments[0].Begin.x, end: segments[0].End.x), content: segments[0]),
            new RBLeafData<Segment>(new Range(begin: segments[1].Begin.x-1f, end: segments[1].End.x), content: segments[1]),
            new RBLeafData<Segment>(new Range(begin: segments[2].Begin.x, end: segments[2].End.x), content: segments[2]),
        };
        Assert.Throws<IntersectingSegmentsException>(() => {
            RBTree<Segment> tree = RBTree.FromLeaves(leaves);
        });
    }

    private static RBTree<Segment?> BuildTreeFromSegments(List<Segment> segments)
    {
        RBLeafData<Segment?>[] leaves = segments.Select(segment => {
            return new RBLeafData<Segment?>(
                range: new Range(begin: segment.Begin.x, end: segment.End.x),
                content: segment
            );
        }).ToArray();
        return RBTree.FromLeaves(leaves);
    }

    private static List<Segment> BuildChainedSegments(Vector2 begin, (bool addToList, Vector2 vector)[] chain)
    {
        List<Segment> segments = new();
        Vector2 segmentBegin = begin;
        foreach ((bool addToList, Vector2 vector) in chain)
        {
            Vector2 segmentEnd = segmentBegin + vector;
            if (addToList)
            {
                segments.Add(new Segment(segmentBegin, segmentEnd));
            }
            segmentBegin = segmentEnd;
        }
        return segments;
    }
}
