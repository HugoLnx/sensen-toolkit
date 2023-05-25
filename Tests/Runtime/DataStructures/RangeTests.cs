using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SensenToolkit.DataStructures;
using UnityEngine;
using UnityEngine.TestTools;

public class RangeTests
{
    private static readonly Range Range = new(-1f, 1f);
    private static readonly Range AtLeft = new(-3f, -2f);
    private static readonly Range AtRight = new(2f, 3f);
    private static readonly Range IntersectLeft = new(-2f, -0.5f);
    private static readonly Range IntersectRight = new(0.5f, 2f);
    private static readonly Range TouchesLeft = new(-2f, -1f);
    private static readonly Range TouchesRight = new(1f, 2f);
    private static readonly Range Inner = new(-0.5f, 0.5f);
    private static readonly Range Outer = new(-1.5f, 1.5f);
    public const float ValueAtLeft = -2f;
    public const float ValueAtRight = 2f;

    [Test]
    [Category("Intersects")]
    public void Intersects_IsFalse_WhenOtherRangeIsAtLeft()
    {
        Assert.False(Range.Intersects(AtLeft));
    }

    [Test]
    [Category("Intersects")]
    public void Intersects_IsFalse_WhenOtherRangeIsAtRight()
    {
        Assert.False(Range.Intersects(AtRight));
    }

    [Test]
    [Category("Intersects")]
    public void Intersects_IsTrue_WhenOtherRangeIntersectAtLeft()
    {
        Assert.True(Range.Intersects(IntersectLeft));
    }

    [Test]
    [Category("Intersects")]
    public void Intersects_IsTrue_WhenOtherRangeIntersectAtRight()
    {
        Assert.True(Range.Intersects(IntersectRight));
    }

    [Test]
    [Category("Intersects")]
    public void Intersects_IsTrue_WhenOtherRangeTouchesLeft()
    {
        Assert.True(Range.Intersects(TouchesLeft));
    }

    [Test]
    [Category("Intersects")]
    public void Intersects_IsTrue_WhenOtherRangeTouchesRight()
    {
        Assert.True(Range.Intersects(TouchesRight));
    }

    [Test]
    [Category("Intersects")]
    public void Intersects_IsTrue_WhenOtherRangeIsContained()
    {
        Assert.True(Range.Intersects(Inner));
    }

    [Test]
    [Category("Intersects")]
    public void Intersects_IsTrue_WhenOtherRangeContains()
    {
        Assert.True(Range.Intersects(Outer));
    }

    [Test]
    [Category("Contains")]
    public void Contains_IsFalse_WhenOtherRangeIsAtLeft()
    {
        Assert.False(Range.Contains(AtLeft));
    }

    [Test]
    [Category("Contains")]
    public void Contains_IsFalse_WhenOtherRangeIsAtRight()
    {
        Assert.False(Range.Contains(AtRight));
    }

    [Test]
    [Category("Contains")]
    public void Contains_IsFalse_WhenOtherRangeIntersectAtLeft()
    {
        Assert.False(Range.Contains(IntersectLeft));
    }

    [Test]
    [Category("Contains")]
    public void Contains_IsFalse_WhenOtherRangeIntersectAtRight()
    {
        Assert.False(Range.Contains(IntersectRight));
    }

    [Test]
    [Category("Contains")]
    public void Contains_IsFalse_WhenOtherRangeTouchesLeft()
    {
        Assert.False(Range.Contains(TouchesLeft));
    }

    [Test]
    [Category("Contains")]
    public void Contains_IsFalse_WhenOtherRangeTouchesRight()
    {
        Assert.False(Range.Contains(TouchesRight));
    }

    [Test]
    [Category("Contains")]
    public void Contains_IsTrue_WhenOtherRangeIsContained()
    {
        Assert.True(Range.Contains(Inner));
    }

    [Test]
    [Category("Contains")]
    public void Contains_IsFalse_WhenOtherRangeContains()
    {
        Assert.False(Range.Contains(Outer));
    }

    [Test]
    [Category("ContainsFloat")]
    public void ContainsFloat_IsFalse_WhenValueIsAtLeft()
    {
        Assert.False(Range.Contains(ValueAtLeft));
    }

    [Test]
    [Category("ContainsFloat")]
    public void ContainsFloat_IsFalse_WhenValueIsAtRight()
    {
        Assert.False(Range.Contains(ValueAtRight));
    }

    [Test]
    [Category("ContainsFloat")]
    public void ContainsFloat_IsTrue_WhenValueIsLeftEdge()
    {
        Assert.True(Range.Contains(Range.Begin));
    }

    [Test]
    [Category("ContainsFloat")]
    public void ContainsFloat_IsTrue_WhenValueIsRightEdge()
    {
        Assert.True(Range.Contains(Range.End));
    }

    [Test]
    [Category("ContainsFloat")]
    public void ContainsFloat_IsTrue_WhenValueIsInsideRange()
    {
        Assert.True(Range.Contains(0.25f));
    }

    [Test]
    [Category("Each")]
    public void Each_IteratesOverRangeInAGivenInterval()
    {
        Assert.That(new Range(-6f, 6f).Each(2f).ToArray(),
            Is.EqualTo(new float[]{-6f, -4f, -2f, 0f, 2f, 4f, 6f}));
    }

    [Test]
    [Category("Each")]
    public void Each_ByDefault_DoesNotIncludeRangeLimit_WhenRangeIsNotMultipleOfInterval()
    {
        Assert.That(new Range(-1f, 2.5f).Each(1f).ToArray(),
            Is.EqualTo(new float[]{-1f, 0f, 1f, 2f}));
    }

    [Test]
    [Category("Each")]
    public void Each_CanIncludeRangeLimit_EvenWhenRangeIsNotMultipleOfInterval()
    {
        Assert.That(new Range(-1f, 2.5f).Each(1f, alwaysIncludeLimits: true).ToArray(),
            Is.EqualTo(new float[]{-1f, 0f, 1f, 2f, 2.5f}));
    }

    [Test]
    [Category("Each")]
    public void Each_DoesNotIncludeRangeLimitTwice_WhenRangeIntervalIsMultipleOfInterval()
    {
        Assert.That(new Range(-1f, 2f).Each(1f, alwaysIncludeLimits: true).ToArray(),
            Is.EqualTo(new float[]{-1f, 0f, 1f, 2f}));
    }

    [Test]
    [Category("EachWithNegativeInterval")]
    public void Each_WithNegativeInterval_IteratesBackwards()
    {
        Assert.That(new Range(-6f, 6f).Each(-2f).ToArray(),
            Is.EqualTo(new float[]{6f, 4f, 2f, 0, -2f, -4f, -6f}));
    }

    [Test]
    [Category("EachWithNegativeInterval")]
    public void Each_WithNegativeInterval_ByDefault_DoesNotIncludeRangeLimit_WhenRangeIsNotMultipleOfInterval()
    {
        Assert.That(new Range(-1.5f, 2f).Each(-1f).ToArray(),
            Is.EqualTo(new float[]{2f, 1f, 0, -1f}));
    }

    [Test]
    [Category("EachWithNegativeInterval")]
    public void Each_WithNegativeInterval_CanIncludeRangeLimit_EvenWhenRangeIsNotMultipleOfInterval()
    {
        Assert.That(new Range(-1.5f, 2f).Each(-1f, alwaysIncludeLimits: true).ToArray(),
            Is.EqualTo(new float[]{2f, 1f, 0, -1f, -1.5f}));
    }

    [Test]
    [Category("EachOnDescendingRange")]
    public void Each_OnDescendingRange_CanIteratesForward()
    {
        Assert.That(new Range(6f, -6f).Each(2f).ToArray(),
            Is.EqualTo(new float[]{6f, 4f, 2f, 0, -2f, -4f, -6f}));
    }

    [Test]
    [Category("EachOnDescendingRange")]
    public void Each_OnDescendingRange_CanIteratesBackwards()
    {
        Assert.That(new Range(6f, -6f).Each(-2f).ToArray(),
            Is.EqualTo(new float[]{-6f, -4f, -2f, 0f, 2f, 4f, 6f}));
    }
}
