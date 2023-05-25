using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SensenToolkit.Mathx;
using UnityEngine;
using UnityEngine.TestTools;

public class SegmentTests
{
    private const float Precision = 1e-3f;

    [Test]
    public void XtoY_ReturnsValueOfYForAGivenX_WhenXIsInsideSegmentRange()
    {
        Segment segment = new Segment(
            begin: new Vector2(-2f, -1f),
            end: new Vector2(2f, 1f)
        );
        Assert.That(segment.XtoY(x: -2f), Is.EqualTo(-1f).Within(Precision));
        Assert.That(segment.XtoY(x: -1f), Is.EqualTo(-0.5f).Within(Precision));
        Assert.That(segment.XtoY(x: 0f), Is.EqualTo(0f).Within(Precision));
        Assert.That(segment.XtoY(x: 1f), Is.EqualTo(0.5f).Within(Precision));
        Assert.That(segment.XtoY(x: 2f), Is.EqualTo(1f).Within(Precision));
    }

    [Test]
    public void XtoY_ReturnsProjectedValueYForAGivenX_WhenXIsOutsideSegmentRange()
    {
        Segment segment = new Segment(
            begin: new Vector2(-2f, -1f),
            end: new Vector2(2f, 1f)
        );
        Assert.That(segment.XtoY(x: -4f), Is.EqualTo(-2f).Within(Precision));
        Assert.That(segment.XtoY(x: 4f), Is.EqualTo(2f).Within(Precision));
    }

    [Test]
    public void YtoX_ReturnsValueOfXForAGivenY_WhenYIsInsideSegmentRange()
    {
        Segment segment = new Segment(
            begin: new Vector2(-1f, -2f),
            end: new Vector2(1f, 2f)
        );
        Assert.That(segment.YtoX(y: -2f), Is.EqualTo(-1f).Within(Precision));
        Assert.That(segment.YtoX(y: -1f), Is.EqualTo(-0.5f).Within(Precision));
        Assert.That(segment.YtoX(y: 0f), Is.EqualTo(0f).Within(Precision));
        Assert.That(segment.YtoX(y: 1f), Is.EqualTo(0.5f).Within(Precision));
        Assert.That(segment.YtoX(y: 2f), Is.EqualTo(1f).Within(Precision));
    }

    [Test]
    public void YtoX_ReturnsProjectedValueXForAGivenY_WhenYIsOutsideSegmentRange()
    {
        Segment segment = new Segment(
            begin: new Vector2(-1f, -2f),
            end: new Vector2(1f, 2f)
        );
        Assert.That(segment.YtoX(y: -4f), Is.EqualTo(-2f).Within(Precision));
        Assert.That(segment.YtoX(y: 4f), Is.EqualTo(2f).Within(Precision));
    }
}
