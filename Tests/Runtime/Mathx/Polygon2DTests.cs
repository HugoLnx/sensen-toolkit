using System.Collections.Generic;
using NUnit.Framework;
using SensenToolkit;
using UnityEngine;
using UnityEngine.TestTools.Utils;

public class Polygon2DTests
{
    private static readonly Vector2EqualityComparer V2Comparer = new(10e-2f);

    #region BuildRegular
    [Test]
    [Category("BuildRegular")]
    public void BuildRegular_BuildsARegularSquare_WhenVertexCountIs4()
    {
        var square = Polygon2D.BuildRegular(vertexCount: 4, radius: 1f);
        Assert.AreEqual(4, square.Vertices.Length);
        Assert.That(square.Vertices[0], Is.EqualTo(Vector2.right).Using(V2Comparer));
        Assert.That(square.Vertices[1], Is.EqualTo(Vector2.up).Using(V2Comparer));
        Assert.That(square.Vertices[2], Is.EqualTo(Vector2.left).Using(V2Comparer));
        Assert.That(square.Vertices[3], Is.EqualTo(Vector2.down).Using(V2Comparer));
    }
    #endregion

    #region Contains
    [Test]
    [Category("Contains")]
    public void Contains_ReturnsTrue_WhenPointIsInsidePolygon()
    {
        var hexagon = Polygon2D.BuildRegular(vertexCount: 6, radius: 1f);

        Assert.IsTrue(hexagon.Contains(new Vector2(0.5f, 0.5f)));
        Assert.IsTrue(hexagon.Contains(new Vector2(-0.5f, 0.5f)));
        Assert.IsTrue(hexagon.Contains(new Vector2(0.5f, -0.5f)));
        Assert.IsTrue(hexagon.Contains(new Vector2(-0.5f, -0.5f)));
        Assert.IsTrue(hexagon.Contains(new Vector2(0.5f, 0f)));
        Assert.IsTrue(hexagon.Contains(new Vector2(-0.5f, 0f)));
        Assert.IsTrue(hexagon.Contains(new Vector2(0f, 0.5f)));
        Assert.IsTrue(hexagon.Contains(new Vector2(0f, -0.5f)));
        Assert.IsTrue(hexagon.Contains(Vector2.zero));
    }

    [Test]
    [Category("Contains")]
    public void Contains_ReturnsFalse_WhenPointIsOutsidePolygon()
    {
        var hexagon = Polygon2D.BuildRegular(vertexCount: 6, radius: 1f);
        Assert.IsFalse(hexagon.Contains(new Vector2(1.5f, 0f)));
        Assert.IsFalse(hexagon.Contains(new Vector2(-1.5f, 0f)));
        Assert.IsFalse(hexagon.Contains(new Vector2(0f, 1.5f)));
        Assert.IsFalse(hexagon.Contains(new Vector2(0f, -1.5f)));
        Assert.IsFalse(hexagon.Contains(new Vector2(1.5f, 0.5f)));
        Assert.IsFalse(hexagon.Contains(new Vector2(-1.5f, 0.5f)));
        Assert.IsFalse(hexagon.Contains(new Vector2(0.5f, 1.5f)));
        Assert.IsFalse(hexagon.Contains(new Vector2(0.5f, -1.5f)));
    }


    [Test]
    [Category("Contains")]
    public void Contains_ReturnsTrue_WhenPointIsOneOfTheVertices()
    {
        var hexagon = Polygon2D.BuildRegular(vertexCount: 6, radius: 1f);
        Assert.IsTrue(hexagon.Contains(hexagon.Vertices[0]));
        Assert.IsTrue(hexagon.Contains(hexagon.Vertices[1]));
        Assert.IsTrue(hexagon.Contains(hexagon.Vertices[2]));
        Assert.IsTrue(hexagon.Contains(hexagon.Vertices[3]));
        Assert.IsTrue(hexagon.Contains(hexagon.Vertices[4]));
        Assert.IsTrue(hexagon.Contains(hexagon.Vertices[5]));
    }

    [Test]
    [Category("Contains")]
    public void Contains_ReturnsTrue_WhenPointIsOnOneOfTheSegments()
    {
        var hexagon = Polygon2D.BuildRegular(vertexCount: 6, radius: 1f);
        Assert.IsTrue(hexagon.Contains(hexagon.Segments[0].Center));
        Assert.IsTrue(hexagon.Contains(hexagon.Segments[1].Center));
        Assert.IsTrue(hexagon.Contains(hexagon.Segments[2].Center));
        Assert.IsTrue(hexagon.Contains(hexagon.Segments[3].Center));
        Assert.IsTrue(hexagon.Contains(hexagon.Segments[4].Center));
        Assert.IsTrue(hexagon.Contains(hexagon.Segments[5].Center));
    }


    [Test]
    [Category("Contains")]
    public void Contains_ReturnsFalse_WhenPointIsOutsidePolygonAndLineCastedPassThroughAVertex()
    {
        // var triangle = Polygon2D.BuildRegular(vertexCount: 3, radius: 1f);
        // Assert.IsFalse(triangle.Contains(new Vector2(0f, -2f)));
        // Assert.IsFalse(triangle.Contains(new Vector2(0f, 2f)));
        // Assert.IsFalse(triangle.Contains(new Vector2(-2f, 0f)));
        // Assert.IsFalse(triangle.Contains(new Vector2(2f, 0f)));

        var hexagon = Polygon2D.BuildRegular(vertexCount: 6, radius: 1f);
        // Assert.IsFalse(hexagon.Contains(new Vector2(0f, -2f)));
        // Assert.IsFalse(hexagon.Contains(new Vector2(0f, 2f)));
        // Assert.IsFalse(hexagon.Contains(new Vector2(-2f, 0f)));
        Assert.IsFalse(hexagon.Contains(new Vector2(2f, 0f)));
    }
    #endregion
}
