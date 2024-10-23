using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using SensenToolkit;

public class Math2DxTests
{
    private static readonly Vector2EqualityComparer V2Comparer = new(10e-2f);
#region ScaleToHorizontalLength
    [Test]
    [Category("ScaleToHorizontalLength")]
    public void ScaleToHorizontalLength_AnHorizontalVectorIsScaledToMatchTheLength()
    {
        Assert.That(
            Math2Dx.ScaleToHorizontalLength(
                vector: Vector2.right,
                length: 20f
            ),
            Is.EqualTo(Vector2.right * 20f).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.ScaleToHorizontalLength(
                vector: Vector2.left,
                length: 20f
            ),
            Is.EqualTo(Vector2.left * 20f).Using(V2Comparer)
        );
    }

    [Test]
    [Category("ScaleToHorizontalLength")]
    public void ScaleToHorizontalLength_ScalesVectorUntilItMatchesTheSpecifiedLengthInX()
    {
        Assert.That(
            Math2Dx.ScaleToHorizontalLength(
                vector: new Vector2(1f, 2f),
                length: 5f
            ),
            Is.EqualTo(new Vector2(5f, 10f)).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.ScaleToHorizontalLength(
                vector: new Vector2(-0.5f, -1f),
                length: 5f
            ),
            Is.EqualTo(new Vector2(-5f, -10f)).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.ScaleToHorizontalLength(
                vector: new Vector2(1f, 3f),
                length: 7f
            ),
            Is.EqualTo(new Vector2(7f, 21f)).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.ScaleToHorizontalLength(
                vector: new Vector2(1f, -3f),
                length: 7f
            ),
            Is.EqualTo(new Vector2(7f, -21f)).Using(V2Comparer)
        );
    }
#endregion
#region AngleToHeadVector2
    [Test]
    [Category("AngleToHeadVector2")]
    public void AngleToHeadVector2_CreateVectorsOfLengthOneInWithThisAngleToTheXAxis()
    {
        Assert.That(
            Math2Dx.AngleToHeadVector2(0f),
            Is.EqualTo(Vector2.right).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.AngleToHeadVector2(90f),
            Is.EqualTo(Vector2.up).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.AngleToHeadVector2(180f),
            Is.EqualTo(Vector2.left).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.AngleToHeadVector2(270f),
            Is.EqualTo(Vector2.down).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.AngleToHeadVector2(60f),
            Is.EqualTo(new Vector2(0.5f, 0.866f)).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.AngleToHeadVector2(-15f),
            Is.EqualTo(new Vector2(0.966f, -0.259f)).Using(V2Comparer)
        );
    }
#endregion
#region Vector2ToAngleAndLength
    [Test]
    [Category("Vector2ToAngleAndLength")]
    public void Vector2ToAngleAndLength_ExtractAngleAndLengthOfVector()
    {
        Assert.That(
            Math2Dx.Vector2ToAngleAndLength(Vector2.right),
            Is.EqualTo((0f, 1f))
        );
        Assert.That(
            Math2Dx.Vector2ToAngleAndLength(Vector2.up * 10f),
            Is.EqualTo((90f, 10f))
        );
        Assert.That(
            Math2Dx.Vector2ToAngleAndLength(Vector2.left * 5f),
            Is.EqualTo((180f, 5f))
        );
        Assert.That(
            Math2Dx.Vector2ToAngleAndLength(Vector2.down * 2f),
            Is.EqualTo((270f, 2f))
        );

        (float angle, float length) vec60 = Math2Dx.Vector2ToAngleAndLength(new Vector2(0.5f, 0.866f));
        Assert.That(vec60.angle, Is.EqualTo(60f).Within(10e-2f));
        Assert.That(vec60.length, Is.EqualTo(1f).Within(10e-2f));

        (float angle, float length) vec345 = Math2Dx.Vector2ToAngleAndLength(new Vector2(0.966f, -0.259f) * 3f);
        Assert.That(vec345.angle, Is.EqualTo(345f).Within(10e-2f));
        Assert.That(vec345.length, Is.EqualTo(3f).Within(10e-2f));
    }
#endregion
#region AngleAndLengthToVector2
    [Test]
    [Category("AngleAndLengthToVector2")]
    public void AngleAndLengthToVector2_CreateAVector2GivenThatAngleAndLength()
    {
        Assert.That(
            Math2Dx.AngleAndLengthToVector2(0f, 1f),
            Is.EqualTo(Vector2.right).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.AngleAndLengthToVector2(90f, 10f),
            Is.EqualTo(Vector2.up * 10f).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.AngleAndLengthToVector2(180f, 5f),
            Is.EqualTo(Vector2.left * 5f).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.AngleAndLengthToVector2(270f, 2f),
            Is.EqualTo(Vector2.down * 2f).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.AngleAndLengthToVector2(60f, 1f),
            Is.EqualTo(new Vector2(0.5f, 0.866f)).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.AngleAndLengthToVector2(345f, 3f),
            Is.EqualTo(new Vector2(0.966f, -0.259f) * 3f).Using(V2Comparer)
        );
    }
#endregion
#region ClampVector2Angle
    [Test]
    [Category("ClampVector2Angle")]
    public void ClampVector2Angle_RotatesTheVectorToMatchTheAngleRange()
    {
        Assert.That(
            Math2Dx.ClampVector2Angle(Vector2.left, min: 0f, max: 90f),
            Is.EqualTo(Vector2.up).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.ClampVector2Angle(Vector2.down * 3f, min: 0f, max: 90f),
            Is.EqualTo(Vector2.right * 3f).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.ClampVector2Angle(Vector2.right * 7f, min: 0f, max: 90f),
            Is.EqualTo(Vector2.right * 7f).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.ClampVector2Angle(Vector2.up * 3f, min: 0f, max: 90f),
            Is.EqualTo(Vector2.up * 3f).Using(V2Comparer)
        );
        Assert.That(
            Math2Dx.ClampVector2Angle(Vector2.up * 13f, min: 0f, max: 180f),
            Is.EqualTo(Vector2.up * 13f).Using(V2Comparer)
        );
    }
#endregion
}
