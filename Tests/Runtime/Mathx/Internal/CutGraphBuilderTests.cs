using NUnit.Framework;
using SensenToolkit;
using SensenToolkit.Internal;
using UnityEngine;
using UnityEngine.TestTools.Utils;

public class CutGraphBuilderTests
{
    private static readonly Vector2EqualityComparer s_v2Comparer = new(10e-2f);

    [Test]
    public void Build_CreatesNodesInTheCorrectPositions_WhenCuttingASquare()
    {
        Polygon2D square = new(new Vector2[]
        {
            new(-1f, 1f),
            new(1f, 1f),
            new(1f, -1f),
            new(-1f, -1f)
        });
        SimpleSegment2D cutSegment = new(
            position: new(-0.5f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(square, cutSegment);
        Assert.That(graph.AllNodes.Length, Is.EqualTo(6));
        Assert.That(graph.AllNodes[0].Position, Is.EqualTo(new Vector2(-1f, 1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[1].Position, Is.EqualTo(new Vector2(-0.5f, 1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[2].Position, Is.EqualTo(new Vector2(1f, 1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[3].Position, Is.EqualTo(new Vector2(1f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[4].Position, Is.EqualTo(new Vector2(-0.5f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[5].Position, Is.EqualTo(new Vector2(-1f, -1f)).Using(s_v2Comparer));
    }

    [Test]
    public void Build_CreatesNodesRightfullyLinked_WhenCuttingASquare()
    {
        Polygon2D square = new(new Vector2[]
        {
            new(-1f, 1f),
            new(1f, 1f),
            new(1f, -1f),
            new(-1f, -1f)
        });
        SimpleSegment2D cutSegment = new(
            position: new(-0.5f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(square, cutSegment);
        Assert.That(graph.AllNodes[0].NextNode, Is.SameAs(graph.AllNodes[1]));
        Assert.That(graph.AllNodes[1].NextNode, Is.SameAs(graph.AllNodes[2]));
        Assert.That(graph.AllNodes[2].NextNode, Is.SameAs(graph.AllNodes[3]));
        Assert.That(graph.AllNodes[3].NextNode, Is.SameAs(graph.AllNodes[4]));
        Assert.That(graph.AllNodes[4].NextNode, Is.SameAs(graph.AllNodes[5]));
        Assert.That(graph.AllNodes[5].NextNode, Is.SameAs(graph.AllNodes[0]));

        Assert.That(graph.AllNodes[0].PreviousNode, Is.SameAs(graph.AllNodes[5]));
        Assert.That(graph.AllNodes[1].PreviousNode, Is.SameAs(graph.AllNodes[0]));
        Assert.That(graph.AllNodes[2].PreviousNode, Is.SameAs(graph.AllNodes[1]));
        Assert.That(graph.AllNodes[3].PreviousNode, Is.SameAs(graph.AllNodes[2]));
        Assert.That(graph.AllNodes[4].PreviousNode, Is.SameAs(graph.AllNodes[3]));
        Assert.That(graph.AllNodes[5].PreviousNode, Is.SameAs(graph.AllNodes[4]));
    }

    [Test]
    public void Build_CreatesCutNodesCorrectly_WhenCuttingASquare()
    {
        Polygon2D square = new(new Vector2[]
        {
            new(-1f, 1f),
            new(1f, 1f),
            new(1f, -1f),
            new(-1f, -1f)
        });
        SimpleSegment2D cutSegment = new(
            position: new(-0.5f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(square, cutSegment);
        Assert.That(graph.CutNodes.Length, Is.EqualTo(2));
        Assert.IsFalse(graph.AllNodes[0].IsCutIntersection);
        Assert.IsTrue(graph.AllNodes[1].IsCutIntersection);
        Assert.IsFalse(graph.AllNodes[2].IsCutIntersection);
        Assert.IsFalse(graph.AllNodes[3].IsCutIntersection);
        Assert.IsTrue(graph.AllNodes[4].IsCutIntersection);
        Assert.IsFalse(graph.AllNodes[5].IsCutIntersection);

        Assert.That(graph.CutNodes[0], Is.SameAs(graph.AllNodes[1]));
        Assert.That(graph.CutNodes[1], Is.SameAs(graph.AllNodes[4]));
    }

    [Test]
    public void Build_CreatesCutNodesWithSidesCorrectly_WhenCuttingASquare()
    {
        Polygon2D square = new(new Vector2[]
        {
            new(-1f, 1f),
            new(1f, 1f),
            new(1f, -1f),
            new(-1f, -1f)
        });
        SimpleSegment2D cutSegment = new(
            position: new(-0.5f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(square, cutSegment);
        Assert.That(graph.AllNodes[0].IsSideA, Is.True);
        Assert.That(graph.AllNodes[0].IsSideB, Is.False);
        Assert.That(graph.AllNodes[1].IsSideA, Is.True);
        Assert.That(graph.AllNodes[1].IsSideB, Is.True);
        Assert.That(graph.AllNodes[2].IsSideA, Is.False);
        Assert.That(graph.AllNodes[2].IsSideB, Is.True);
        Assert.That(graph.AllNodes[3].IsSideA, Is.False);
        Assert.That(graph.AllNodes[3].IsSideB, Is.True);
        Assert.That(graph.AllNodes[4].IsSideA, Is.True);
        Assert.That(graph.AllNodes[4].IsSideB, Is.True);
        Assert.That(graph.AllNodes[5].IsSideA, Is.True);
        Assert.That(graph.AllNodes[5].IsSideB, Is.False);
    }

    [Test]
    public void Build_SetCornersAndSidesCorrectly_WhenCuttingASquareExactlyOnTheLeftSegment()
    {
        Polygon2D square = new(new Vector2[]
        {
            new(-1f, 1f),
            new(1f, 1f),
            new(1f, -1f),
            new(-1f, -1f)
        });
        SimpleSegment2D cutSegment = new(
            position: new(-1f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(square, cutSegment);
        Assert.That(graph.AllNodes.Length, Is.EqualTo(4));

        Assert.That(graph.AllNodes[0].IsSideB, Is.True);
        Assert.That(graph.AllNodes[1].IsSideB, Is.True);
        Assert.That(graph.AllNodes[2].IsSideB, Is.True);
        Assert.That(graph.AllNodes[3].IsSideB, Is.True);
        Assert.That(graph.AllNodes[0].IsSideA, Is.False);
        Assert.That(graph.AllNodes[1].IsSideA, Is.False);
        Assert.That(graph.AllNodes[2].IsSideA, Is.False);
        Assert.That(graph.AllNodes[3].IsSideA, Is.False);

        Assert.That(graph.AllNodes[0], Is.SameAs(graph.CutNodes[0]));
        Assert.That(graph.AllNodes[3], Is.SameAs(graph.CutNodes[1]));
    }

    [Test]
    public void Build_SetCornersAndSidesCorrectly_WhenCuttingASquareExactlyOnTheRightSegment()
    {
        Polygon2D square = new(new Vector2[]
        {
            new(-1f, 1f),
            new(1f, 1f),
            new(1f, -1f),
            new(-1f, -1f)
        });
        SimpleSegment2D cutSegment = new(
            position: new(1f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(square, cutSegment);
        Assert.That(graph.AllNodes.Length, Is.EqualTo(4));

        Assert.That(graph.AllNodes[0].IsSideA, Is.True);
        Assert.That(graph.AllNodes[1].IsSideA, Is.True);
        Assert.That(graph.AllNodes[2].IsSideA, Is.True);
        Assert.That(graph.AllNodes[3].IsSideA, Is.True);
        Assert.That(graph.AllNodes[0].IsSideB, Is.False);
        Assert.That(graph.AllNodes[1].IsSideB, Is.False);
        Assert.That(graph.AllNodes[2].IsSideB, Is.False);
        Assert.That(graph.AllNodes[3].IsSideB, Is.False);

        Assert.That(graph.AllNodes[1], Is.SameAs(graph.CutNodes[0]));
        Assert.That(graph.AllNodes[2], Is.SameAs(graph.CutNodes[1]));
    }

    [Test]
    public void Build_SetCorners_WhenCuttingASquareDiamondExactlyOnTheLeftTip()
    {
        Polygon2D squareDiamond = new(new Vector2[]
        {
            new(0f, 1f),
            new(1f, 0f),
            new(0f, -1f),
            new(-1f, 0f),
        });
        SimpleSegment2D cutSegment = new(
            position: new(-1f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(squareDiamond, cutSegment);
        Assert.That(graph.AllNodes.Length, Is.EqualTo(4));

        Assert.That(graph.AllNodes[0].IsSideB, Is.True);
        Assert.That(graph.AllNodes[1].IsSideB, Is.True);
        Assert.That(graph.AllNodes[2].IsSideB, Is.True);
        Assert.That(graph.AllNodes[3].IsSideB, Is.True);
        Assert.That(graph.AllNodes[0].IsSideA, Is.False);
        Assert.That(graph.AllNodes[1].IsSideA, Is.False);
        Assert.That(graph.AllNodes[2].IsSideA, Is.False);
        Assert.That(graph.AllNodes[3].IsSideA, Is.False);

        Assert.That(graph.AllNodes[3], Is.SameAs(graph.CutNodes[0]));
    }

    [Test]
    public void Build_LinkCrossingNodesCorrectly_WhenCuttingASquareThrough()
    {
        Polygon2D square = new(new Vector2[]
        {
            new(-1f, 1f),
            new(1f, 1f),
            new(1f, -1f),
            new(-1f, -1f)
        });
        SimpleSegment2D cutSegment = new(
            position: new(-0.5f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(square, cutSegment);
        Assert.That(graph.AllNodes[0].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[2].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[2].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[5].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[5].PreviousCrossingCutNode, Is.Null);

        Assert.That(graph.CutNodes[0].NextCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[0]));
    }

    [Test]
    public void Build_LinkCrossingNodesCorrectly_WhenCuttingASquareAtLeftSegment()
    {
        Polygon2D square = new(new Vector2[]
        {
            new(-1f, 1f),
            new(1f, 1f),
            new(1f, -1f),
            new(-1f, -1f)
        });
        SimpleSegment2D cutSegment = new(
            position: new(-1f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(square, cutSegment);
        Assert.That(graph.AllNodes.Length, Is.EqualTo(4));
        Assert.That(graph.AllNodes[1].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[1].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[2].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[2].PreviousCrossingCutNode, Is.Null);

        Assert.That(graph.CutNodes[0].NextCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[0]));
    }


    [Test]
    public void Build_Works_WhenCuttingAComplexPolygonWithGapsThrough()
    {
        //     |
        //  +--x--+
        //  |     |
        //  +--x  |
        //     |  |
        //  +--x  |
        //   \    |
        //  +--x  |
        //   \    |
        //     x--+
        //     |
        //    \ /
        Polygon2D polygon = new Polygon2DEasyBuilder()
            .WorldPoint(Vector2.zero)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.down + Vector2.right)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.down + Vector2.right)
            .NextPoint(Vector2.right)
            .WorldPoint(new Vector2(2f, 0f))
            .Build();

        SimpleSegment2D cutSegment = new(
            position: new(0.5f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(polygon, cutSegment);
        Assert.That(graph.AllNodes.Length, Is.EqualTo(16));
        Assert.That(graph.AllNodes[0].Position, Is.EqualTo(Vector2.zero).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[1].Position, Is.EqualTo(new Vector2(0f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[2].Position, Is.EqualTo(new Vector2(0.5f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[3].Position, Is.EqualTo(new Vector2(1f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[4].Position, Is.EqualTo(new Vector2(1f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[5].Position, Is.EqualTo(new Vector2(0.5f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[6].Position, Is.EqualTo(new Vector2(0f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[7].Position, Is.EqualTo(new Vector2(0.5f, -2.5f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[8].Position, Is.EqualTo(new Vector2(1f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[9].Position, Is.EqualTo(new Vector2(0.5f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[10].Position, Is.EqualTo(new Vector2(0f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[11].Position, Is.EqualTo(new Vector2(0.5f, -3.5f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[12].Position, Is.EqualTo(new Vector2(1f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[13].Position, Is.EqualTo(new Vector2(2f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[14].Position, Is.EqualTo(new Vector2(2f, 0f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[15].Position, Is.EqualTo(new Vector2(0.5f, 0f)).Using(s_v2Comparer));

        Assert.That(graph.CutNodes.Length, Is.EqualTo(6));
        Assert.That(graph.CutNodes[0], Is.SameAs(graph.AllNodes[15]));
        Assert.That(graph.CutNodes[1], Is.SameAs(graph.AllNodes[2]));
        Assert.That(graph.CutNodes[2], Is.SameAs(graph.AllNodes[5]));
        Assert.That(graph.CutNodes[3], Is.SameAs(graph.AllNodes[7]));
        Assert.That(graph.CutNodes[4], Is.SameAs(graph.AllNodes[9]));
        Assert.That(graph.CutNodes[5], Is.SameAs(graph.AllNodes[11]));

        // Assert sides are set correctly
        Assert.That(graph.CutNodes[0].IsSideA, Is.True);
        Assert.That(graph.CutNodes[0].IsSideB, Is.True);
        Assert.That(graph.CutNodes[1].IsSideA, Is.True);
        Assert.That(graph.CutNodes[1].IsSideB, Is.True);
        Assert.That(graph.CutNodes[2].IsSideA, Is.True);
        Assert.That(graph.CutNodes[2].IsSideB, Is.True);
        Assert.That(graph.CutNodes[3].IsSideA, Is.True);
        Assert.That(graph.CutNodes[3].IsSideB, Is.True);
        Assert.That(graph.CutNodes[4].IsSideA, Is.True);
        Assert.That(graph.CutNodes[4].IsSideB, Is.True);
        Assert.That(graph.CutNodes[5].IsSideA, Is.True);
        Assert.That(graph.CutNodes[5].IsSideB, Is.True);

        Assert.That(graph.AllNodes[0].IsSideA, Is.True);
        Assert.That(graph.AllNodes[0].IsSideB, Is.False);
        Assert.That(graph.AllNodes[1].IsSideA, Is.True);
        Assert.That(graph.AllNodes[1].IsSideB, Is.False);

        Assert.That(graph.AllNodes[3].IsSideA, Is.False);
        Assert.That(graph.AllNodes[3].IsSideB, Is.True);
        Assert.That(graph.AllNodes[4].IsSideA, Is.False);
        Assert.That(graph.AllNodes[4].IsSideB, Is.True);

        Assert.That(graph.AllNodes[6].IsSideA, Is.True);
        Assert.That(graph.AllNodes[6].IsSideB, Is.False);

        Assert.That(graph.AllNodes[8].IsSideA, Is.False);
        Assert.That(graph.AllNodes[8].IsSideB, Is.True);

        Assert.That(graph.AllNodes[10].IsSideA, Is.True);
        Assert.That(graph.AllNodes[10].IsSideB, Is.False);

        Assert.That(graph.AllNodes[12].IsSideA, Is.False);
        Assert.That(graph.AllNodes[12].IsSideB, Is.True);
        Assert.That(graph.AllNodes[13].IsSideA, Is.False);
        Assert.That(graph.AllNodes[13].IsSideB, Is.True);
        Assert.That(graph.AllNodes[14].IsSideA, Is.False);
        Assert.That(graph.AllNodes[14].IsSideB, Is.True);


        // Assert crossing nodes are linked correctly
        Assert.That(graph.CutNodes[0].NextCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[0]));
        Assert.That(graph.CutNodes[2].NextCrossingCutNode, Is.SameAs(graph.CutNodes[3]));
        Assert.That(graph.CutNodes[2].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[3].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[2]));
        Assert.That(graph.CutNodes[4].NextCrossingCutNode, Is.SameAs(graph.CutNodes[5]));
        Assert.That(graph.CutNodes[4].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[5].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[5].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[4]));

        Assert.That(graph.AllNodes[0].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[1].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[1].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[4].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[4].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[6].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[6].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[8].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[8].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[10].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[10].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[12].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[12].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[13].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[13].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[14].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[14].PreviousCrossingCutNode, Is.Null);
    }

    [Test]
    public void Build_Works_WhenCuttingAComplexPolygonWithGapsAtTheEdge()
    {
        //  |
        //  x-----+
        //  |     |
        //  x--+  |
        //     |  |
        //  x--+  |
        //   \    |
        //  x--+  |
        //   \    |
        //     +--+
        //  |
        // \ /
        Polygon2D polygon = new Polygon2DEasyBuilder()
            .WorldPoint(Vector2.zero)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.down + Vector2.right)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.down + Vector2.right)
            .NextPoint(Vector2.right)
            .WorldPoint(new Vector2(2f, 0f))
            .Build();

        SimpleSegment2D cutSegment = new(
            position: new(0f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(polygon, cutSegment);
        Assert.That(graph.AllNodes.Length, Is.EqualTo(10));
        Assert.That(graph.AllNodes[0].Position, Is.EqualTo(Vector2.zero).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[1].Position, Is.EqualTo(new Vector2(0f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[2].Position, Is.EqualTo(new Vector2(1f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[3].Position, Is.EqualTo(new Vector2(1f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[4].Position, Is.EqualTo(new Vector2(0f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[5].Position, Is.EqualTo(new Vector2(1f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[6].Position, Is.EqualTo(new Vector2(0f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[7].Position, Is.EqualTo(new Vector2(1f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[8].Position, Is.EqualTo(new Vector2(2f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[9].Position, Is.EqualTo(new Vector2(2f, 0f)).Using(s_v2Comparer));

        Assert.That(graph.CutNodes.Length, Is.EqualTo(4));
        Assert.That(graph.CutNodes[0], Is.SameAs(graph.AllNodes[0]));
        Assert.That(graph.CutNodes[1], Is.SameAs(graph.AllNodes[1]));
        Assert.That(graph.CutNodes[2], Is.SameAs(graph.AllNodes[4]));
        Assert.That(graph.CutNodes[3], Is.SameAs(graph.AllNodes[6]));

        // Assert sides are set correctly
        Assert.That(graph.AllNodes[0].IsSideA, Is.False);
        Assert.That(graph.AllNodes[1].IsSideA, Is.False);
        Assert.That(graph.AllNodes[2].IsSideA, Is.False);
        Assert.That(graph.AllNodes[3].IsSideA, Is.False);
        Assert.That(graph.AllNodes[4].IsSideA, Is.False);
        Assert.That(graph.AllNodes[5].IsSideA, Is.False);
        Assert.That(graph.AllNodes[6].IsSideA, Is.False);
        Assert.That(graph.AllNodes[7].IsSideA, Is.False);
        Assert.That(graph.AllNodes[8].IsSideA, Is.False);
        Assert.That(graph.AllNodes[9].IsSideA, Is.False);

        Assert.That(graph.AllNodes[0].IsSideB, Is.True);
        Assert.That(graph.AllNodes[1].IsSideB, Is.True);
        Assert.That(graph.AllNodes[2].IsSideB, Is.True);
        Assert.That(graph.AllNodes[3].IsSideB, Is.True);
        Assert.That(graph.AllNodes[4].IsSideB, Is.True);
        Assert.That(graph.AllNodes[5].IsSideB, Is.True);
        Assert.That(graph.AllNodes[6].IsSideB, Is.True);
        Assert.That(graph.AllNodes[7].IsSideB, Is.True);
        Assert.That(graph.AllNodes[8].IsSideB, Is.True);
        Assert.That(graph.AllNodes[9].IsSideB, Is.True);

        // Assert crossing nodes are linked correctly
        Assert.That(graph.CutNodes[0].NextCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[0]));
        Assert.That(graph.CutNodes[2].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[2].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[3].PreviousCrossingCutNode, Is.Null);

        Assert.That(graph.AllNodes[2].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[2].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[5].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[5].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[8].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[8].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[9].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[9].PreviousCrossingCutNode, Is.Null);
    }

    [Test]
    public void Build_Works_WhenCuttingAPolygonWithOneGap()
    {
        //   |
        //   x-----+
        //   |     |
        //   x--+  |
        //      |  |
        //   x--+  |
        //   |     |
        //   x-----+
        //   |
        //  \ /
        Polygon2D polygon = new Polygon2DEasyBuilder()
            .WorldPoint(Vector2.zero)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right * 2f)
            .WorldPoint(new Vector2(2f, 0f))
            .Build();

        SimpleSegment2D cutSegment = new(
            position: new(0f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(polygon, cutSegment);
        Assert.That(graph.AllNodes.Length, Is.EqualTo(8));
        Assert.That(graph.AllNodes[0].Position, Is.EqualTo(Vector2.zero).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[1].Position, Is.EqualTo(new Vector2(0f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[2].Position, Is.EqualTo(new Vector2(1f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[3].Position, Is.EqualTo(new Vector2(1f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[4].Position, Is.EqualTo(new Vector2(0f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[5].Position, Is.EqualTo(new Vector2(0f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[6].Position, Is.EqualTo(new Vector2(2f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[7].Position, Is.EqualTo(new Vector2(2f, 0f)).Using(s_v2Comparer));

        Assert.That(graph.CutNodes.Length, Is.EqualTo(4));
        Assert.That(graph.CutNodes[0], Is.SameAs(graph.AllNodes[0]));
        Assert.That(graph.CutNodes[1], Is.SameAs(graph.AllNodes[1]));
        Assert.That(graph.CutNodes[2], Is.SameAs(graph.AllNodes[4]));
        Assert.That(graph.CutNodes[3], Is.SameAs(graph.AllNodes[5]));

        // Assert sides are set correctly
        Assert.That(graph.AllNodes[0].IsSideA, Is.False);
        Assert.That(graph.AllNodes[1].IsSideA, Is.False);
        Assert.That(graph.AllNodes[2].IsSideA, Is.False);
        Assert.That(graph.AllNodes[3].IsSideA, Is.False);
        Assert.That(graph.AllNodes[4].IsSideA, Is.False);
        Assert.That(graph.AllNodes[5].IsSideA, Is.False);
        Assert.That(graph.AllNodes[6].IsSideA, Is.False);
        Assert.That(graph.AllNodes[7].IsSideA, Is.False);

        Assert.That(graph.AllNodes[0].IsSideB, Is.True);
        Assert.That(graph.AllNodes[1].IsSideB, Is.True);
        Assert.That(graph.AllNodes[2].IsSideB, Is.True);
        Assert.That(graph.AllNodes[3].IsSideB, Is.True);
        Assert.That(graph.AllNodes[4].IsSideB, Is.True);
        Assert.That(graph.AllNodes[5].IsSideB, Is.True);
        Assert.That(graph.AllNodes[6].IsSideB, Is.True);
        Assert.That(graph.AllNodes[7].IsSideB, Is.True);

        // Assert crossing nodes are linked correctly
        Assert.That(graph.CutNodes[0].NextCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[0]));
        Assert.That(graph.CutNodes[2].NextCrossingCutNode, Is.SameAs(graph.CutNodes[3]));
        Assert.That(graph.CutNodes[2].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[3].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[2]));

        Assert.That(graph.AllNodes[2].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[2].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[6].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[6].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].PreviousCrossingCutNode, Is.Null);
    }


    [Test]
    public void Build_Works_WhenCuttingAPolygonWithOneHill()
    {
        //      |
        //      x--+
        //      |  |
        //   +--x  |
        //   |     |
        //   +--x  |
        //      |  |
        //      x--+
        //      |
        //     \ /
        Polygon2D polygon = new Polygon2DEasyBuilder()
            .WorldPoint(Vector2.zero)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right)
            .WorldPoint(new Vector2(1f, 0f))
            .Build();

        SimpleSegment2D cutSegment = new(
            position: new(0f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(polygon, cutSegment);
        Assert.That(graph.AllNodes.Length, Is.EqualTo(8));
        Assert.That(graph.AllNodes[0].Position, Is.EqualTo(Vector2.zero).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[1].Position, Is.EqualTo(new Vector2(0f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[2].Position, Is.EqualTo(new Vector2(-1f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[3].Position, Is.EqualTo(new Vector2(-1f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[4].Position, Is.EqualTo(new Vector2(0f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[5].Position, Is.EqualTo(new Vector2(0f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[6].Position, Is.EqualTo(new Vector2(1f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[7].Position, Is.EqualTo(new Vector2(1f, 0f)).Using(s_v2Comparer));

        Assert.That(graph.CutNodes.Length, Is.EqualTo(4));
        Assert.That(graph.CutNodes[0], Is.SameAs(graph.AllNodes[0]));
        Assert.That(graph.CutNodes[1], Is.SameAs(graph.AllNodes[1]));
        Assert.That(graph.CutNodes[2], Is.SameAs(graph.AllNodes[4]));
        Assert.That(graph.CutNodes[3], Is.SameAs(graph.AllNodes[5]));

        // Assert sides are set correctly
        Assert.That(graph.AllNodes[0].IsSideA, Is.False);
        Assert.That(graph.AllNodes[1].IsSideA, Is.True);
        Assert.That(graph.AllNodes[2].IsSideA, Is.True);
        Assert.That(graph.AllNodes[3].IsSideA, Is.True);
        Assert.That(graph.AllNodes[4].IsSideA, Is.True);
        Assert.That(graph.AllNodes[5].IsSideA, Is.False);
        Assert.That(graph.AllNodes[6].IsSideA, Is.False);
        Assert.That(graph.AllNodes[7].IsSideA, Is.False);

        Assert.That(graph.AllNodes[0].IsSideB, Is.True);
        Assert.That(graph.AllNodes[1].IsSideB, Is.True);
        Assert.That(graph.AllNodes[2].IsSideB, Is.False);
        Assert.That(graph.AllNodes[3].IsSideB, Is.False);
        Assert.That(graph.AllNodes[4].IsSideB, Is.True);
        Assert.That(graph.AllNodes[5].IsSideB, Is.True);
        Assert.That(graph.AllNodes[6].IsSideB, Is.True);
        Assert.That(graph.AllNodes[7].IsSideB, Is.True);

        // Assert crossing nodes are linked correctly
        Assert.That(graph.CutNodes[0].NextCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].NextCrossingCutNode, Is.SameAs(graph.CutNodes[2]));
        Assert.That(graph.CutNodes[1].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[0]));
        Assert.That(graph.CutNodes[2].NextCrossingCutNode, Is.SameAs(graph.CutNodes[3]));
        Assert.That(graph.CutNodes[2].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[3].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[2]));

        Assert.That(graph.AllNodes[2].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[2].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[6].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[6].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].PreviousCrossingCutNode, Is.Null);
    }

    [Test]
    public void Build_Works_WhenCuttingThroughAPolygonWithTwoHills()
    {
        //      |
        //      x--+
        //      |  |
        //   +--x  |
        //   |     |
        //   +--x  |
        //      |  |
        //   +--x  |
        //   |     |
        //   +--x  |
        //      |  |
        //      x--+
        //      |
        //     \ /
        Polygon2D polygon = new Polygon2DEasyBuilder()
            .WorldPoint(Vector2.zero)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right)
            .WorldPoint(new Vector2(1f, 0f))
            .Build();

        SimpleSegment2D cutSegment = new(
            position: new(0f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(polygon, cutSegment);
        Assert.That(graph.AllNodes.Length, Is.EqualTo(12));
        Assert.That(graph.AllNodes[0].Position, Is.EqualTo(Vector2.zero).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[1].Position, Is.EqualTo(new Vector2(0f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[2].Position, Is.EqualTo(new Vector2(-1f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[3].Position, Is.EqualTo(new Vector2(-1f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[4].Position, Is.EqualTo(new Vector2(0f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[5].Position, Is.EqualTo(new Vector2(0f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[6].Position, Is.EqualTo(new Vector2(-1f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[7].Position, Is.EqualTo(new Vector2(-1f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[8].Position, Is.EqualTo(new Vector2(0f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[9].Position, Is.EqualTo(new Vector2(0f, -5f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[10].Position, Is.EqualTo(new Vector2(1f, -5f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[11].Position, Is.EqualTo(new Vector2(1f, 0f)).Using(s_v2Comparer));

        Assert.That(graph.CutNodes.Length, Is.EqualTo(6));
        Assert.That(graph.CutNodes[0], Is.SameAs(graph.AllNodes[0]));
        Assert.That(graph.CutNodes[1], Is.SameAs(graph.AllNodes[1]));
        Assert.That(graph.CutNodes[2], Is.SameAs(graph.AllNodes[4]));
        Assert.That(graph.CutNodes[3], Is.SameAs(graph.AllNodes[5]));
        Assert.That(graph.CutNodes[4], Is.SameAs(graph.AllNodes[8]));
        Assert.That(graph.CutNodes[5], Is.SameAs(graph.AllNodes[9]));

        // Assert sides are set correctly
        Assert.That(graph.AllNodes[0].IsSideA, Is.False);
        Assert.That(graph.AllNodes[1].IsSideA, Is.True);
        Assert.That(graph.AllNodes[2].IsSideA, Is.True);
        Assert.That(graph.AllNodes[3].IsSideA, Is.True);
        Assert.That(graph.AllNodes[4].IsSideA, Is.True);
        Assert.That(graph.AllNodes[5].IsSideA, Is.True);
        Assert.That(graph.AllNodes[6].IsSideA, Is.True);
        Assert.That(graph.AllNodes[7].IsSideA, Is.True);
        Assert.That(graph.AllNodes[8].IsSideA, Is.True);
        Assert.That(graph.AllNodes[9].IsSideA, Is.False);
        Assert.That(graph.AllNodes[10].IsSideA, Is.False);
        Assert.That(graph.AllNodes[11].IsSideA, Is.False);

        Assert.That(graph.AllNodes[0].IsSideB, Is.True);
        Assert.That(graph.AllNodes[1].IsSideB, Is.True);
        Assert.That(graph.AllNodes[2].IsSideB, Is.False);
        Assert.That(graph.AllNodes[3].IsSideB, Is.False);
        Assert.That(graph.AllNodes[4].IsSideB, Is.True);
        Assert.That(graph.AllNodes[5].IsSideB, Is.True);
        Assert.That(graph.AllNodes[6].IsSideB, Is.False);
        Assert.That(graph.AllNodes[7].IsSideB, Is.False);
        Assert.That(graph.AllNodes[8].IsSideB, Is.True);
        Assert.That(graph.AllNodes[9].IsSideB, Is.True);
        Assert.That(graph.AllNodes[10].IsSideB, Is.True);
        Assert.That(graph.AllNodes[11].IsSideB, Is.True);

        // Assert crossing nodes are linked correctly
        Assert.That(graph.CutNodes[0].NextCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].NextCrossingCutNode, Is.SameAs(graph.CutNodes[2]));
        Assert.That(graph.CutNodes[1].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[0]));
        Assert.That(graph.CutNodes[2].NextCrossingCutNode, Is.SameAs(graph.CutNodes[3]));
        Assert.That(graph.CutNodes[2].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[3].NextCrossingCutNode, Is.SameAs(graph.CutNodes[4]));
        Assert.That(graph.CutNodes[3].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[2]));
        Assert.That(graph.CutNodes[4].NextCrossingCutNode, Is.SameAs(graph.CutNodes[5]));
        Assert.That(graph.CutNodes[4].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[3]));
        Assert.That(graph.CutNodes[5].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[5].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[4]));

        Assert.That(graph.AllNodes[2].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[2].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[6].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[6].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[10].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[10].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[11].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[11].PreviousCrossingCutNode, Is.Null);
    }

    [Test]
    public void Build_Works_WhenCuttingAPolygonWithLotsOfCutCorners()
    {
        //   |
        //   x-----+
        //    \    |
        //   x--+  |
        //   |     |
        //   x--+  |
        //     /   |
        //   x     |
        //    \    |
        //   x--+  |
        //   |     |
        //   x--+  |
        //     /   |
        //   x-----+
        //   |
        //  \ /
        Polygon2D polygon = new Polygon2DEasyBuilder()
            .WorldPoint(Vector2.zero)
            .NextPoint(Vector2.down + Vector2.right)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right)
            .NextPoint(Vector2.down + Vector2.left)
            .NextPoint(Vector2.down + Vector2.right)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right)
            .NextPoint(Vector2.down + Vector2.left)
            .NextPoint(Vector2.right * 2f)
            .WorldPoint(new Vector2(2f, 0f))
            .Build();

        SimpleSegment2D cutSegment = new(
            position: new(0f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(polygon, cutSegment);
        Assert.That(graph.AllNodes.Length, Is.EqualTo(13));
        Assert.That(graph.AllNodes[0].Position, Is.EqualTo(Vector2.zero).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[1].Position, Is.EqualTo(new Vector2(1f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[2].Position, Is.EqualTo(new Vector2(0f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[3].Position, Is.EqualTo(new Vector2(0f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[4].Position, Is.EqualTo(new Vector2(1f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[5].Position, Is.EqualTo(new Vector2(0f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[6].Position, Is.EqualTo(new Vector2(1f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[7].Position, Is.EqualTo(new Vector2(0f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[8].Position, Is.EqualTo(new Vector2(0f, -5f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[9].Position, Is.EqualTo(new Vector2(1f, -5f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[10].Position, Is.EqualTo(new Vector2(0f, -6f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[11].Position, Is.EqualTo(new Vector2(2f, -6f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[12].Position, Is.EqualTo(new Vector2(2f, 0f)).Using(s_v2Comparer));

        Assert.That(graph.CutNodes.Length, Is.EqualTo(7));
        Assert.That(graph.CutNodes[0], Is.SameAs(graph.AllNodes[0]));
        Assert.That(graph.CutNodes[1], Is.SameAs(graph.AllNodes[2]));
        Assert.That(graph.CutNodes[2], Is.SameAs(graph.AllNodes[3]));
        Assert.That(graph.CutNodes[3], Is.SameAs(graph.AllNodes[5]));
        Assert.That(graph.CutNodes[4], Is.SameAs(graph.AllNodes[7]));
        Assert.That(graph.CutNodes[5], Is.SameAs(graph.AllNodes[8]));
        Assert.That(graph.CutNodes[6], Is.SameAs(graph.AllNodes[10]));

        // Assert sides are set correctly
        Assert.That(graph.AllNodes[0].IsSideA, Is.False);
        Assert.That(graph.AllNodes[1].IsSideA, Is.False);
        Assert.That(graph.AllNodes[2].IsSideA, Is.False);
        Assert.That(graph.AllNodes[3].IsSideA, Is.False);
        Assert.That(graph.AllNodes[4].IsSideA, Is.False);
        Assert.That(graph.AllNodes[5].IsSideA, Is.False);
        Assert.That(graph.AllNodes[6].IsSideA, Is.False);
        Assert.That(graph.AllNodes[7].IsSideA, Is.False);
        Assert.That(graph.AllNodes[8].IsSideA, Is.False);
        Assert.That(graph.AllNodes[9].IsSideA, Is.False);
        Assert.That(graph.AllNodes[10].IsSideA, Is.False);
        Assert.That(graph.AllNodes[11].IsSideA, Is.False);
        Assert.That(graph.AllNodes[12].IsSideA, Is.False);

        Assert.That(graph.AllNodes[0].IsSideB, Is.True);
        Assert.That(graph.AllNodes[1].IsSideB, Is.True);
        Assert.That(graph.AllNodes[2].IsSideB, Is.True);
        Assert.That(graph.AllNodes[3].IsSideB, Is.True);
        Assert.That(graph.AllNodes[4].IsSideB, Is.True);
        Assert.That(graph.AllNodes[5].IsSideB, Is.True);
        Assert.That(graph.AllNodes[6].IsSideB, Is.True);
        Assert.That(graph.AllNodes[7].IsSideB, Is.True);
        Assert.That(graph.AllNodes[8].IsSideB, Is.True);
        Assert.That(graph.AllNodes[9].IsSideB, Is.True);
        Assert.That(graph.AllNodes[10].IsSideB, Is.True);
        Assert.That(graph.AllNodes[11].IsSideB, Is.True);
        Assert.That(graph.AllNodes[12].IsSideB, Is.True);

        // Assert crossing nodes are linked correctly
        Assert.That(graph.CutNodes[0].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].NextCrossingCutNode, Is.SameAs(graph.CutNodes[2]));
        Assert.That(graph.CutNodes[1].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[2].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[2].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[3].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[4].NextCrossingCutNode, Is.SameAs(graph.CutNodes[5]));
        Assert.That(graph.CutNodes[4].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[5].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[5].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[4]));
        Assert.That(graph.CutNodes[6].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[6].PreviousCrossingCutNode, Is.Null);

        Assert.That(graph.AllNodes[1].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[1].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[4].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[4].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[6].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[6].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[9].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[9].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[11].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[11].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[12].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[12].PreviousCrossingCutNode, Is.Null);
    }

    [Test]
    public void Build_Works_WhenCuttingAPolygonThatStartsCutInANonVertexAndThenGoesForVertices()
    {
        //      |
        //   +--x-----+
        //   |        |
        //   +--x--+  |
        //         |  |
        //      x--+  |
        //      |     |
        //   +--x     |
        //   |        |
        //   +--x-----+
        //      |
        //     \ /
        Polygon2D polygon = new Polygon2DEasyBuilder()
            .WorldPoint(Vector2.zero)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right * 2f)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right * 3f)
            .WorldPoint(new Vector2(3f, 0f))
            .Build();

        SimpleSegment2D cutSegment = new(
            position: new(1f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(polygon, cutSegment);
        Assert.That(graph.AllNodes.Length, Is.EqualTo(13));
        Assert.That(graph.AllNodes[0].Position, Is.EqualTo(Vector2.zero).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[1].Position, Is.EqualTo(new Vector2(0f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[2].Position, Is.EqualTo(new Vector2(1f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[3].Position, Is.EqualTo(new Vector2(2f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[4].Position, Is.EqualTo(new Vector2(2f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[5].Position, Is.EqualTo(new Vector2(1f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[6].Position, Is.EqualTo(new Vector2(1f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[7].Position, Is.EqualTo(new Vector2(0f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[8].Position, Is.EqualTo(new Vector2(0f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[9].Position, Is.EqualTo(new Vector2(1f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[10].Position, Is.EqualTo(new Vector2(3f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[11].Position, Is.EqualTo(new Vector2(3f, 0f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[12].Position, Is.EqualTo(new Vector2(1f, 0f)).Using(s_v2Comparer));

        Assert.That(graph.CutNodes.Length, Is.EqualTo(5));
        Assert.That(graph.CutNodes[0], Is.SameAs(graph.AllNodes[12]));
        Assert.That(graph.CutNodes[1], Is.SameAs(graph.AllNodes[2]));
        Assert.That(graph.CutNodes[2], Is.SameAs(graph.AllNodes[5]));
        Assert.That(graph.CutNodes[3], Is.SameAs(graph.AllNodes[6]));
        Assert.That(graph.CutNodes[4], Is.SameAs(graph.AllNodes[9]));

        // Assert sides are set correctly
        Assert.That(graph.AllNodes[0].IsSideA, Is.True);
        Assert.That(graph.AllNodes[1].IsSideA, Is.True);
        Assert.That(graph.AllNodes[2].IsSideA, Is.True);
        Assert.That(graph.AllNodes[3].IsSideA, Is.False);
        Assert.That(graph.AllNodes[4].IsSideA, Is.False);
        Assert.That(graph.AllNodes[5].IsSideA, Is.False);
        Assert.That(graph.AllNodes[6].IsSideA, Is.True);
        Assert.That(graph.AllNodes[7].IsSideA, Is.True);
        Assert.That(graph.AllNodes[8].IsSideA, Is.True);
        Assert.That(graph.AllNodes[9].IsSideA, Is.True);
        Assert.That(graph.AllNodes[10].IsSideA, Is.False);
        Assert.That(graph.AllNodes[11].IsSideA, Is.False);
        Assert.That(graph.AllNodes[12].IsSideA, Is.True);

        Assert.That(graph.AllNodes[0].IsSideB, Is.False);
        Assert.That(graph.AllNodes[1].IsSideB, Is.False);
        Assert.That(graph.AllNodes[2].IsSideB, Is.True);
        Assert.That(graph.AllNodes[3].IsSideB, Is.True);
        Assert.That(graph.AllNodes[4].IsSideB, Is.True);
        Assert.That(graph.AllNodes[5].IsSideB, Is.True);
        Assert.That(graph.AllNodes[6].IsSideB, Is.True);
        Assert.That(graph.AllNodes[7].IsSideB, Is.False);
        Assert.That(graph.AllNodes[8].IsSideB, Is.False);
        Assert.That(graph.AllNodes[9].IsSideB, Is.True);
        Assert.That(graph.AllNodes[10].IsSideB, Is.True);
        Assert.That(graph.AllNodes[11].IsSideB, Is.True);
        Assert.That(graph.AllNodes[12].IsSideB, Is.True);

        // Assert crossing nodes are linked correctly
        Assert.That(graph.CutNodes[0].NextCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[0]));
        Assert.That(graph.CutNodes[2].NextCrossingCutNode, Is.SameAs(graph.CutNodes[3]));
        Assert.That(graph.CutNodes[2].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[3].NextCrossingCutNode, Is.SameAs(graph.CutNodes[4]));
        Assert.That(graph.CutNodes[3].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[2]));
        Assert.That(graph.CutNodes[4].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[4].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[3]));

        Assert.That(graph.AllNodes[0].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[1].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[1].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[4].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[4].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[8].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[8].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[10].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[10].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[11].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[11].PreviousCrossingCutNode, Is.Null);
    }

    [Test]
    public void Build_Works_WhenCuttingAPolygonThatStartsOnSideBAndFinishesOnSideA()
    {
        //
        //      |
        //      x--+
        //      |  |
        //   +--x  |
        //   |     |
        //   +--x  |
        //      |  |
        //   +--x  |
        //   |     |
        //   |  x--+
        //   |  |
        //   +--x
        //      |
        //     \ /
        //
        Polygon2D polygon = new Polygon2DEasyBuilder()
            .WorldPoint(Vector2.zero)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.down * 2f)
            .NextPoint(Vector2.right)
            .NextPoint(Vector2.up)
            .NextPoint(Vector2.right)
            .WorldPoint(new Vector2(1f, 0f))
            .Build();

        SimpleSegment2D cutSegment = new(
            position: new(0f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(polygon, cutSegment);
        Assert.That(graph.AllNodes.Length, Is.EqualTo(12));
        Assert.That(graph.AllNodes[0].Position, Is.EqualTo(Vector2.zero).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[1].Position, Is.EqualTo(new Vector2(0f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[2].Position, Is.EqualTo(new Vector2(-1f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[3].Position, Is.EqualTo(new Vector2(-1f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[4].Position, Is.EqualTo(new Vector2(0f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[5].Position, Is.EqualTo(new Vector2(0f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[6].Position, Is.EqualTo(new Vector2(-1f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[7].Position, Is.EqualTo(new Vector2(-1f, -5f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[8].Position, Is.EqualTo(new Vector2(0f, -5f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[9].Position, Is.EqualTo(new Vector2(0f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[10].Position, Is.EqualTo(new Vector2(1f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[11].Position, Is.EqualTo(new Vector2(1f, 0f)).Using(s_v2Comparer));

        Assert.That(graph.CutNodes.Length, Is.EqualTo(6));
        Assert.That(graph.CutNodes[0], Is.SameAs(graph.AllNodes[0]));
        Assert.That(graph.CutNodes[1], Is.SameAs(graph.AllNodes[1]));
        Assert.That(graph.CutNodes[2], Is.SameAs(graph.AllNodes[4]));
        Assert.That(graph.CutNodes[3], Is.SameAs(graph.AllNodes[5]));
        Assert.That(graph.CutNodes[4], Is.SameAs(graph.AllNodes[9]));
        Assert.That(graph.CutNodes[5], Is.SameAs(graph.AllNodes[8]));

        // Assert sides are set correctly
        Assert.That(graph.AllNodes[0].IsSideA, Is.False);
        Assert.That(graph.AllNodes[1].IsSideA, Is.True);
        Assert.That(graph.AllNodes[2].IsSideA, Is.True);
        Assert.That(graph.AllNodes[3].IsSideA, Is.True);
        Assert.That(graph.AllNodes[4].IsSideA, Is.True);
        Assert.That(graph.AllNodes[5].IsSideA, Is.True);
        Assert.That(graph.AllNodes[6].IsSideA, Is.True);
        Assert.That(graph.AllNodes[7].IsSideA, Is.True);
        Assert.That(graph.AllNodes[8].IsSideA, Is.True);
        Assert.That(graph.AllNodes[9].IsSideA, Is.True);
        Assert.That(graph.AllNodes[10].IsSideA, Is.False);
        Assert.That(graph.AllNodes[11].IsSideA, Is.False);

        Assert.That(graph.AllNodes[0].IsSideB, Is.True);
        Assert.That(graph.AllNodes[1].IsSideB, Is.True);
        Assert.That(graph.AllNodes[2].IsSideB, Is.False);
        Assert.That(graph.AllNodes[3].IsSideB, Is.False);
        Assert.That(graph.AllNodes[4].IsSideB, Is.True);
        Assert.That(graph.AllNodes[5].IsSideB, Is.True);
        Assert.That(graph.AllNodes[6].IsSideB, Is.False);
        Assert.That(graph.AllNodes[7].IsSideB, Is.False);
        Assert.That(graph.AllNodes[8].IsSideB, Is.False);
        Assert.That(graph.AllNodes[9].IsSideB, Is.True);
        Assert.That(graph.AllNodes[10].IsSideB, Is.True);
        Assert.That(graph.AllNodes[11].IsSideB, Is.True);

        // Assert crossing nodes are linked correctly
        Assert.That(graph.CutNodes[0].NextCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].NextCrossingCutNode, Is.SameAs(graph.CutNodes[2]));
        Assert.That(graph.CutNodes[1].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[0]));
        Assert.That(graph.CutNodes[2].NextCrossingCutNode, Is.SameAs(graph.CutNodes[3]));
        Assert.That(graph.CutNodes[2].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[3].NextCrossingCutNode, Is.SameAs(graph.CutNodes[4]));
        Assert.That(graph.CutNodes[3].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[2]));
        Assert.That(graph.CutNodes[4].NextCrossingCutNode, Is.SameAs(graph.CutNodes[5]));
        Assert.That(graph.CutNodes[4].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[3]));
        Assert.That(graph.CutNodes[5].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[5].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[4]));

        Assert.That(graph.AllNodes[2].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[2].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[6].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[6].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[10].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[10].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[11].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[11].PreviousCrossingCutNode, Is.Null);
    }


    [Test]
    public void Build_Works_WhenCuttingAPolygonThatStartsOnSideBAndFinishesOnSideATip()
    {
        //
        //         |
        //         x--+
        //         |  |
        //      +--x  |
        //      |     |
        //      +--x  |
        //         |  |
        //   +-----x  |
        //   |        |
        //   |  +--x--+
        //   |  |
        //   |  +--x
        //   |     |
        //   +-----x
        //         |
        //        \ /
        //
        Polygon2D polygon = new Polygon2DEasyBuilder()
            .WorldPoint(Vector2.zero)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.left * 2f)
            .NextPoint(Vector2.down * 3f)
            .NextPoint(Vector2.right * 2f)
            .NextPoint(Vector2.up)
            .NextPoint(Vector2.left)
            .NextPoint(Vector2.up)
            .NextPoint(Vector2.right * 2f)
            .WorldPoint(new Vector2(1f, 0f))
            .Build();

        SimpleSegment2D cutSegment = new(
            position: new(0f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(polygon, cutSegment);
        Assert.That(graph.AllNodes.Length, Is.EqualTo(15));
        Assert.That(graph.AllNodes[0].Position, Is.EqualTo(Vector2.zero).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[1].Position, Is.EqualTo(new Vector2(0f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[2].Position, Is.EqualTo(new Vector2(-1f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[3].Position, Is.EqualTo(new Vector2(-1f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[4].Position, Is.EqualTo(new Vector2(0f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[5].Position, Is.EqualTo(new Vector2(0f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[6].Position, Is.EqualTo(new Vector2(-2f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[7].Position, Is.EqualTo(new Vector2(-2f, -6f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[8].Position, Is.EqualTo(new Vector2(0f, -6f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[9].Position, Is.EqualTo(new Vector2(0f, -5f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[10].Position, Is.EqualTo(new Vector2(-1f, -5f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[11].Position, Is.EqualTo(new Vector2(-1f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[12].Position, Is.EqualTo(new Vector2(0f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[13].Position, Is.EqualTo(new Vector2(1f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[14].Position, Is.EqualTo(new Vector2(1f, 0f)).Using(s_v2Comparer));

        Assert.That(graph.CutNodes.Length, Is.EqualTo(7));
        Assert.That(graph.CutNodes[0], Is.SameAs(graph.AllNodes[0]));
        Assert.That(graph.CutNodes[1], Is.SameAs(graph.AllNodes[1]));
        Assert.That(graph.CutNodes[2], Is.SameAs(graph.AllNodes[4]));
        Assert.That(graph.CutNodes[3], Is.SameAs(graph.AllNodes[5]));
        Assert.That(graph.CutNodes[4], Is.SameAs(graph.AllNodes[12]));
        Assert.That(graph.CutNodes[5], Is.SameAs(graph.AllNodes[9]));
        Assert.That(graph.CutNodes[6], Is.SameAs(graph.AllNodes[8]));

        // Assert sides are set correctly
        Assert.That(graph.AllNodes[0].IsSideA, Is.False);
        Assert.That(graph.AllNodes[1].IsSideA, Is.True);
        Assert.That(graph.AllNodes[2].IsSideA, Is.True);
        Assert.That(graph.AllNodes[3].IsSideA, Is.True);
        Assert.That(graph.AllNodes[4].IsSideA, Is.True);
        Assert.That(graph.AllNodes[5].IsSideA, Is.True);
        Assert.That(graph.AllNodes[6].IsSideA, Is.True);
        Assert.That(graph.AllNodes[7].IsSideA, Is.True);
        Assert.That(graph.AllNodes[8].IsSideA, Is.True);
        Assert.That(graph.AllNodes[9].IsSideA, Is.True);
        Assert.That(graph.AllNodes[10].IsSideA, Is.True);
        Assert.That(graph.AllNodes[11].IsSideA, Is.True);
        Assert.That(graph.AllNodes[12].IsSideA, Is.True);
        Assert.That(graph.AllNodes[13].IsSideA, Is.False);
        Assert.That(graph.AllNodes[14].IsSideA, Is.False);

        Assert.That(graph.AllNodes[0].IsSideB, Is.True);
        Assert.That(graph.AllNodes[1].IsSideB, Is.True);
        Assert.That(graph.AllNodes[2].IsSideB, Is.False);
        Assert.That(graph.AllNodes[3].IsSideB, Is.False);
        Assert.That(graph.AllNodes[4].IsSideB, Is.True);
        Assert.That(graph.AllNodes[5].IsSideB, Is.True);
        Assert.That(graph.AllNodes[6].IsSideB, Is.False);
        Assert.That(graph.AllNodes[7].IsSideB, Is.False);
        Assert.That(graph.AllNodes[8].IsSideB, Is.False);
        Assert.That(graph.AllNodes[9].IsSideB, Is.False);
        Assert.That(graph.AllNodes[10].IsSideB, Is.False);
        Assert.That(graph.AllNodes[11].IsSideB, Is.False);
        Assert.That(graph.AllNodes[12].IsSideB, Is.True);
        Assert.That(graph.AllNodes[13].IsSideB, Is.True);
        Assert.That(graph.AllNodes[14].IsSideB, Is.True);

        // Assert crossing nodes are linked correctly
        Assert.That(graph.CutNodes[0].NextCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].NextCrossingCutNode, Is.SameAs(graph.CutNodes[2]));
        Assert.That(graph.CutNodes[1].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[0]));
        Assert.That(graph.CutNodes[2].NextCrossingCutNode, Is.SameAs(graph.CutNodes[3]));
        Assert.That(graph.CutNodes[2].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[3].NextCrossingCutNode, Is.SameAs(graph.CutNodes[4]));
        Assert.That(graph.CutNodes[3].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[2]));
        Assert.That(graph.CutNodes[4].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[4].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[3]));
        Assert.That(graph.CutNodes[5].NextCrossingCutNode, Is.SameAs(graph.CutNodes[6]));
        Assert.That(graph.CutNodes[5].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[6].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[6].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[5]));

        Assert.That(graph.AllNodes[2].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[2].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[6].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[6].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[10].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[10].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[11].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[11].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[13].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[13].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[14].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[14].PreviousCrossingCutNode, Is.Null);
    }

    [Test]
    public void Build_Works_WhenCuttingAPolygonShapedInSpiral()
    {
        //          |
        //    +-----x
        //    |     |
        //    |  +--x
        //    |  |
        //    |  |  x-----+
        //    |  |  |     |
        //    |  |  x--+  |
        //    |  |     |  |
        //    |  +--x--+  |
        //    |           |
        //    +-----x-----+
        //          |
        //         \ /
        Polygon2D polygon = new Polygon2DEasyBuilder()
            .WorldPoint(Vector2.zero)
            .NextPoint(Vector2.down * 5f)
            .NextPoint(Vector2.right * 4f)
            .NextPoint(Vector2.up * 3f)
            .NextPoint(Vector2.left * 2f)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.right)
            .NextPoint(Vector2.down)
            .NextPoint(Vector2.left * 2f)
            .NextPoint(Vector2.up * 3f)
            .NextPoint(Vector2.right)
            .NextPoint(Vector2.up)
            .Build();

        SimpleSegment2D cutSegment = new(
            position: new(2f, 0f),
            direction: Vector2.down,
            lengthForward: Mathf.Infinity,
            lengthBackward: Mathf.Infinity
        );
        CutGraph graph = CutGraphBuilder.Build(polygon, cutSegment);
        Assert.That(graph.AllNodes.Length, Is.EqualTo(14));
        Assert.That(graph.AllNodes[0].Position, Is.EqualTo(Vector2.zero).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[1].Position, Is.EqualTo(new Vector2(0f, -5f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[2].Position, Is.EqualTo(new Vector2(2f, -5f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[3].Position, Is.EqualTo(new Vector2(4f, -5f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[4].Position, Is.EqualTo(new Vector2(4f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[5].Position, Is.EqualTo(new Vector2(2f, -2f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[6].Position, Is.EqualTo(new Vector2(2f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[7].Position, Is.EqualTo(new Vector2(3f, -3f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[8].Position, Is.EqualTo(new Vector2(3f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[9].Position, Is.EqualTo(new Vector2(2f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[10].Position, Is.EqualTo(new Vector2(1f, -4f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[11].Position, Is.EqualTo(new Vector2(1f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[12].Position, Is.EqualTo(new Vector2(2f, -1f)).Using(s_v2Comparer));
        Assert.That(graph.AllNodes[13].Position, Is.EqualTo(new Vector2(2f, 0f)).Using(s_v2Comparer));

        Assert.That(graph.CutNodes.Length, Is.EqualTo(6));
        Assert.That(graph.CutNodes[0], Is.SameAs(graph.AllNodes[13]));
        Assert.That(graph.CutNodes[1], Is.SameAs(graph.AllNodes[12]));
        Assert.That(graph.CutNodes[2], Is.SameAs(graph.AllNodes[5]));
        Assert.That(graph.CutNodes[3], Is.SameAs(graph.AllNodes[6]));
        Assert.That(graph.CutNodes[4], Is.SameAs(graph.AllNodes[9]));
        Assert.That(graph.CutNodes[5], Is.SameAs(graph.AllNodes[2]));

        // Assert sides are set correctly
        Assert.That(graph.AllNodes[0].IsSideA, Is.True);
        Assert.That(graph.AllNodes[1].IsSideA, Is.True);
        Assert.That(graph.AllNodes[2].IsSideA, Is.True);
        Assert.That(graph.AllNodes[3].IsSideA, Is.False);
        Assert.That(graph.AllNodes[4].IsSideA, Is.False);
        Assert.That(graph.AllNodes[5].IsSideA, Is.False);
        Assert.That(graph.AllNodes[6].IsSideA, Is.False);
        Assert.That(graph.AllNodes[7].IsSideA, Is.False);
        Assert.That(graph.AllNodes[8].IsSideA, Is.False);
        Assert.That(graph.AllNodes[9].IsSideA, Is.True);
        Assert.That(graph.AllNodes[10].IsSideA, Is.True);
        Assert.That(graph.AllNodes[11].IsSideA, Is.True);
        Assert.That(graph.AllNodes[12].IsSideA, Is.True);
        Assert.That(graph.AllNodes[13].IsSideA, Is.True);

        Assert.That(graph.AllNodes[0].IsSideB, Is.False);
        Assert.That(graph.AllNodes[1].IsSideB, Is.False);
        Assert.That(graph.AllNodes[2].IsSideB, Is.True);
        Assert.That(graph.AllNodes[3].IsSideB, Is.True);
        Assert.That(graph.AllNodes[4].IsSideB, Is.True);
        Assert.That(graph.AllNodes[5].IsSideB, Is.True);
        Assert.That(graph.AllNodes[6].IsSideB, Is.True);
        Assert.That(graph.AllNodes[7].IsSideB, Is.True);
        Assert.That(graph.AllNodes[8].IsSideB, Is.True);
        Assert.That(graph.AllNodes[9].IsSideB, Is.True);
        Assert.That(graph.AllNodes[10].IsSideB, Is.False);
        Assert.That(graph.AllNodes[11].IsSideB, Is.False);
        Assert.That(graph.AllNodes[12].IsSideB, Is.False);
        Assert.That(graph.AllNodes[13].IsSideB, Is.False);

        // Assert crossing nodes are linked correctly
        Assert.That(graph.CutNodes[0].NextCrossingCutNode, Is.SameAs(graph.CutNodes[1]));
        Assert.That(graph.CutNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[1].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[0]));
        Assert.That(graph.CutNodes[2].NextCrossingCutNode, Is.SameAs(graph.CutNodes[3]));
        Assert.That(graph.CutNodes[2].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[3].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[2]));
        Assert.That(graph.CutNodes[4].NextCrossingCutNode, Is.SameAs(graph.CutNodes[5]));
        Assert.That(graph.CutNodes[4].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[5].NextCrossingCutNode, Is.Null);
        Assert.That(graph.CutNodes[5].PreviousCrossingCutNode, Is.SameAs(graph.CutNodes[4]));

        Assert.That(graph.AllNodes[0].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[0].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[1].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[1].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[3].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[4].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[4].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[7].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[8].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[8].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[10].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[10].PreviousCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[11].NextCrossingCutNode, Is.Null);
        Assert.That(graph.AllNodes[11].PreviousCrossingCutNode, Is.Null);
    }
}
