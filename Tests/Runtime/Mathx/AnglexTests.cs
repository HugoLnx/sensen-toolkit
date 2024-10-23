using NUnit.Framework;
using SensenToolkit;

public class AnglexTests
{
#region RelocateAngleToRange
    [Test]
    [Category("RelocateAngleToRange")]
    public void RelocateAngleToRange_ReturnOwnAngle_WhenItsWithinRange()
    {
        Assert.That(
            Anglex.RelocateAngleToRange(
                degrees: 30f,
                min: 0f,
                max: 90f
            ), Is.EqualTo(30f).Within(10e-3f)
        );
        Assert.That(
            Anglex.RelocateAngleToRange(
                degrees: -30f,
                min: -45f,
                max: 45f
            ), Is.EqualTo(-30f).Within(10e-3f)
        );
    }

    [Test]
    [Category("RelocateAngleToRange")]
    public void RelocateAngleToRange_ReturnAngleRelativeToMinLimit_WhenItIsOutOfRange()
    {
        Assert.That(
            Anglex.RelocateAngleToRange(
                degrees: 330f,
                min: -45f,
                max: 45f
            ), Is.EqualTo(-30f).Within(10e-3f)
        );
        Assert.That(
            Anglex.RelocateAngleToRange(
                degrees: 390f,
                min: -45f,
                max: 45f
            ), Is.EqualTo(30f).Within(10e-3f)
        );
    }

    [Test]
    [Category("RelocateAngleToRange")]
    public void RelocateAngleToRange_ReturnAngleRelativeToMaxLimit_WhenItsWithinNegativeRangeOfMaxLimit()
    {
        Assert.That(
            Anglex.RelocateAngleToRange(
                degrees: 330f,
                min: 0f,
                max: -45f
            ), Is.EqualTo(-30f).Within(10e-3f)
        );
        Assert.That(
            Anglex.RelocateAngleToRange(
                degrees: 270f,
                min: 0f,
                max: -45f
            ), Is.EqualTo(270f).Within(10e-3f)
        );
    }

    [Test]
    [Category("RelocateAngleToRange")]
    public void RelocateAngleToRange_ReturnAngleRelativeToMinLimit_OnReversedRange()
    {
        Assert.That(
            Anglex.RelocateAngleToRange(
                degrees: 30f,
                min: 45f,
                max: 0f
            ), Is.EqualTo(390f).Within(10e-3f)
        );
    }
#endregion
#region ClampAngle
    [Test]
    [Category("ClampAngle")]
    public void ClampAngle_ReturnsOwnAngle_WhenItIsWithinRange()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 30f,
                min: 0f,
                max: 90f
            ), Is.EqualTo(30f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 60f,
                min: 25f,
                max: 90f
            ), Is.EqualTo(60f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 0f,
                min: 0f,
                max: 45f
            ), Is.EqualTo(0f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 90f,
                min: 30f,
                max: 90f
            ), Is.EqualTo(90f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle")]
    public void ClampAngle_ReturnsCloserLimit_WhenAngleIsOutOfRange()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: -30f,
                min: 0f,
                max: 90f
            ), Is.EqualTo(0f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 120f,
                min: 0f,
                max: 90f
            ), Is.EqualTo(90f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle")]
    public void ClampAngle_ReturnsCloserLimitConsideringItsFullRotation_WhenAngleIsOutOfRange()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 360f-30f,
                min: 0f,
                max: 90f
            ), Is.EqualTo(0f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 200f,
                min: 0f,
                max: 90f
            ), Is.EqualTo(90f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle")]
    public void ClampAngle_ReturnsOwnAngle_WhenWithinReversedRange()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 120f,
                min: 90f,
                max: 0f
            ), Is.EqualTo(120f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 330f,
                min: 90f,
                max: 0f
            ), Is.EqualTo(330f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle")]
    public void ClampAngle_ReturnsClosestReversedLimit_WhenOutOfReversedRange()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 60f,
                min: 90f,
                max: 0f
            ), Is.EqualTo(90f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 30f,
                min: 90f,
                max: 0f
            ), Is.EqualTo(0f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle")]
    public void ClampAngle_AcceptsRangeWithNegativeLimits()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 30f,
                min: -45f,
                max: 45f
            ), Is.EqualTo(30f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -30f,
                min: -45f,
                max: 45f
            ), Is.EqualTo(330f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -60f,
                min: -45f,
                max: 45f
            ), Is.EqualTo(315f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 60f,
                min: -45f,
                max: 45f
            ), Is.EqualTo(45f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle")]
    public void ClampAngle_AcceptsAngleBeyond360()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 720f,
                min: -45f,
                max: 45f
            ), Is.EqualTo(0f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 60f,
                min: 390f,
                max: 480f
            ), Is.EqualTo(60f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 10f,
                min: 390f,
                max: 480f
            ), Is.EqualTo(30f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 180f,
                min: 390f,
                max: 480f
            ), Is.EqualTo(120f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle")]
    public void ClampAngle_ReturnsAngleNormalizedByDefault()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: -30f,
                min: -90f,
                max: 90f
            ), Is.EqualTo(330f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle")]
    public void ClampAngle_CanSkipNormalization()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 30f,
                min: -45f,
                max: 45f,
                normalize: false
            ), Is.EqualTo(30f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -30f,
                min: -45f,
                max: 45f,
                normalize: false
            ), Is.EqualTo(-30f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -60f,
                min: -45f,
                max: 45f,
                normalize: false
            ), Is.EqualTo(-45f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 60f,
                min: -45f,
                max: 45f,
                normalize: false
            ), Is.EqualTo(45f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle")]
    public void ClampAngle_CanSkipNormalization_OnReversedRanges()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 30f,
                min: 45f,
                max: -45f,
                normalize: false
            ), Is.EqualTo(45f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -30f,
                min: 45f,
                max: -45f,
                normalize: false
            ), Is.EqualTo(-45f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -60f,
                min: 45f,
                max: -45f,
                normalize: false
            ), Is.EqualTo(300f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle")]
    public void ClampAngle_CanSkipNormalization_OnAngleBeyond360()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 690f,
                min: -45f,
                max: 45f,
                normalize: false
            ), Is.EqualTo(-30f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 60f,
                min: 390f,
                max: 480f,
                normalize: false
            ), Is.EqualTo(420f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 10f,
                min: 390f,
                max: 480f,
                normalize: false
            ), Is.EqualTo(390f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 180f,
                min: 390f,
                max: 480f,
                normalize: false
            ), Is.EqualTo(480f).Within(10e-3f)
        );
    }
#endregion
#region ClampAngleWithReflection
    [Test]
    [Category("ClampAngle_WithReflection")]
    public void ClampAngle_WithReflection_ReturnsOwnAngle_WhenItIsWithinRange()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 30f,
                min: 0f,
                max: 90f,
                includeReflection: true
            ), Is.EqualTo(30f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 210f,
                min: 0f,
                max: 90f,
                includeReflection: true
            ), Is.EqualTo(210f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 60f,
                min: 25f,
                max: 90f,
                includeReflection: true
            ), Is.EqualTo(60f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 240f,
                min: 25f,
                max: 90f,
                includeReflection: true
            ), Is.EqualTo(240f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 0f,
                min: 0f,
                max: 45f,
                includeReflection: true
            ), Is.EqualTo(0f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 180f,
                min: 0f,
                max: 45f,
                includeReflection: true
            ), Is.EqualTo(180f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 90f,
                min: 30f,
                max: 90f,
                includeReflection: true
            ), Is.EqualTo(90f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 270f,
                min: 30f,
                max: 90f,
                includeReflection: true
            ), Is.EqualTo(270f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle_WithReflection")]
    public void ClampAngle_WithReflection_ReturnsCloserLimit_WhenAngleIsOutOfRange()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: -30f,
                min: 0f,
                max: 90f,
                includeReflection: true
            ), Is.EqualTo(0f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 150f,
                min: 0f,
                max: 90f,
                includeReflection: true
            ), Is.EqualTo(180f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 120f,
                min: 0f,
                max: 90f,
                includeReflection: true
            ), Is.EqualTo(90f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -60f,
                min: 0f,
                max: 90f,
                includeReflection: true
            ), Is.EqualTo(270f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle_WithReflection")]
    public void ClampAngle_WithReflection_ReturnsOwnAngle_IfRangeCover360()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 0f,
                min: 0f,
                max: 180f,
                includeReflection: true
            ), Is.EqualTo(0f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 75f,
                min: 0f,
                max: 180f,
                includeReflection: true
            ), Is.EqualTo(75f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 135f,
                min: 0f,
                max: 180f,
                includeReflection: true
            ), Is.EqualTo(135f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 215f,
                min: 0f,
                max: 180f,
                includeReflection: true
            ), Is.EqualTo(215f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 325f,
                min: 0f,
                max: 180f,
                includeReflection: true
            ), Is.EqualTo(325f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 360f,
                min: 0f,
                max: 180f,
                includeReflection: true
            ), Is.EqualTo(0f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle_WithReflection")]
    public void ClampAngle_WithReflection_ReturnsOwnAngle_WhenWithinReversedRange()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 320f,
                min: 300f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(320f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 140f,
                min: 300f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(140f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 195f,
                min: 300f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(195f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle_WithReflection")]
    public void ClampAngle_WithReflection_ReturnsClosestReversedLimit_WhenOutOfReversedRange()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 270f,
                min: 300f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(300f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 90f,
                min: 300f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(120f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 240f,
                min: 300f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(210f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle_WithReflection")]
    public void ClampAngle_WithReflection_AcceptsRangeWithNegativeLimits()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 15f,
                min: -30f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(15f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 165f,
                min: -30f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(165f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -15f,
                min: -30f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(345f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -180f,
                min: -30f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(180f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -60f,
                min: -30f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(330f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -120f,
                min: -30f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(210f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 60f,
                min: -30f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(30f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 120f,
                min: -30f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(150f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle_WithReflection")]
    public void ClampAngle_WithReflection_AcceptsAngleBeyond360()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 720f,
                min: -30f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(0f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 540f,
                min: -30f,
                max: 30f,
                includeReflection: true
            ), Is.EqualTo(180f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 60f,
                min: 390f,
                max: 450f,
                includeReflection: true
            ), Is.EqualTo(60f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 240f,
                min: 390f,
                max: 450f,
                includeReflection: true
            ), Is.EqualTo(240f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 10f,
                min: 390f,
                max: 450f,
                includeReflection: true
            ), Is.EqualTo(30f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 200f,
                min: 390f,
                max: 450f,
                includeReflection: true
            ), Is.EqualTo(210f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 100f,
                min: 390f,
                max: 450f,
                includeReflection: true
            ), Is.EqualTo(90f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 280f,
                min: 390f,
                max: 450f,
                includeReflection: true
            ), Is.EqualTo(270f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle_WithReflection")]
    public void ClampAngle_WithReflection_ReturnsAngleNormalizedByDefault()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: -210f,
                min: -90f,
                max: 90f,
                includeReflection: true
            ), Is.EqualTo(150f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle_WithReflection")]
    public void ClampAngle_WithReflection_CanSkipNormalization()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: -15f,
                min: -30f,
                max: 30f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(-15f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -45f,
                min: -270f,
                max: -210f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(-45f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -60f,
                min: -45f,
                max: 45f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(-45f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -100f,
                min: -270f,
                max: -210f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(-90f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 60f,
                min: -45f,
                max: 45f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(45f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -15f,
                min: -270f,
                max: -210f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(-30f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle_WithReflection")]
    public void ClampAngle_WithReflection_CanSkipNormalization_OnReversedRanges()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: -45f,
                min: -30f,
                max: -330f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(-30f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -235f,
                min: -30f,
                max: -330f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(-210f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -315f,
                min: -30f,
                max: -330f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(-330f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -135f,
                min: -30f,
                max: -330f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(-150f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -15f,
                min: -30f,
                max: -330f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(-15f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: -180f,
                min: -30f,
                max: -330f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(-180f).Within(10e-3f)
        );
    }

    [Test]
    [Category("ClampAngle_WithReflection")]
    public void ClampAngle_WithReflection_CanSkipNormalization_OnAngleBeyond360()
    {
        Assert.That(
            Anglex.ClampAngle(
                degrees: 480f,
                min: -270f,
                max: -210f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(-240f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 660f,
                min: -270f,
                max: -210f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(-60f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 60f,
                min: 390f,
                max: 480f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(420f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 240f,
                min: 390f,
                max: 480f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(600f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 10f,
                min: 390f,
                max: 480f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(390f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 190f,
                min: 390f,
                max: 480f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(570f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 150f,
                min: 390f,
                max: 480f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(480f).Within(10e-3f)
        );
        Assert.That(
            Anglex.ClampAngle(
                degrees: 330f,
                min: 390f,
                max: 480f,
                normalize: false,
                includeReflection: true
            ), Is.EqualTo(660f).Within(10e-3f)
        );
    }
#endregion
#region NormalizeAngle
    [Test]
    [Category("NormalizeAngle")]
    public void NormalizeAngle_GetsItsEquivalentValueBetween0And360()
    {
        Assert.That(
            Anglex.NormalizeAngle(-30f),
            Is.EqualTo(330f).Within(10e-3f)
        );
        Assert.That(
            Anglex.NormalizeAngle(360f),
            Is.EqualTo(0f).Within(10e-3f)
        );
        Assert.That(
            Anglex.NormalizeAngle(400f),
            Is.EqualTo(40f).Within(10e-3f)
        );
        Assert.That(
            Anglex.NormalizeAngle((360f * 100f) + 50f),
            Is.EqualTo(50f).Within(10e-3f)
        );
        Assert.That(
            Anglex.NormalizeAngle((-360f*100f) + 60f),
            Is.EqualTo(60f).Within(10e-3f)
        );
    }
#endregion
#region AngleIsBetween
    [Test]
    [Category("AngleIsBetween")]
    public void AngleIsBetween_WorksWithRegularAngles()
    {
        Assert.True(
            Anglex.AngleIsBetween(30f, min: 0f, max: 180f)
        );
        Assert.False(
            Anglex.AngleIsBetween(270f, min: 0f, max: 180f)
        );
        Assert.True(
            Anglex.AngleIsBetween(270f, min: 180f, max: 360f)
        );
        Assert.False(
            Anglex.AngleIsBetween(90f, min: 180f, max: 360f)
        );
    }

    [Test]
    [Category("AngleIsBetween")]
    public void AngleIsBetween_WorksWithNegativeAngles()
    {
        Assert.True(
            Anglex.AngleIsBetween(-330f, min: 0f, max: 180f)
        );
        Assert.False(
            Anglex.AngleIsBetween(-90f, min: 0f, max: 180f)
        );
        Assert.True(
            Anglex.AngleIsBetween(-90f, min: 180f, max: 360f)
        );
        Assert.False(
            Anglex.AngleIsBetween(-270f, min: 180f, max: 360f)
        );
    }

    [Test]
    [Category("AngleIsBetween")]
    public void AngleIsBetween_WorksWithNegativeRanges()
    {
        Assert.True(
            Anglex.AngleIsBetween(30f, min: -360f, max: -180f)
        );
        Assert.False(
            Anglex.AngleIsBetween(270f, min: -360f, max: -180f)
        );
        Assert.True(
            Anglex.AngleIsBetween(-90f, min: -180f, max: 0f)
        );
        Assert.False(
            Anglex.AngleIsBetween(-270f, min: -180, max: 0f)
        );
    }

    [Test]
    [Category("AngleIsBetween")]
    public void AngleIsBetween_WorksWithAnglesBeyond360()
    {
        Assert.True(
            Anglex.AngleIsBetween(30f, min: 720f, max: 900f)
        );
        Assert.False(
            Anglex.AngleIsBetween(270f, min: 720f, max: 900f)
        );
        Assert.True(
            Anglex.AngleIsBetween(930f, min: 180f, max: 360f)
        );
        Assert.False(
            Anglex.AngleIsBetween(750f, min: 180f, max: 360f)
        );
    }

    [Test]
    [Category("AngleIsBetween")]
    public void AngleIsBetween_ReturnsTrue_WhenRangeDistanceIncludesTheWholeCircle()
    {
        Assert.True(
            Anglex.AngleIsBetween(9999f, min: 0f, max: 360f)
        );
        Assert.True(
            Anglex.AngleIsBetween(9999f, min: -360f, max: 0f)
        );
        Assert.True(
            Anglex.AngleIsBetween(9999f, min: -360f, max: 360f)
        );
        Assert.True(
            Anglex.AngleIsBetween(9999f, min: 360f, max: 720f)
        );
        Assert.True(
            Anglex.AngleIsBetween(9999f, min: -720f, max: -360f)
        );
        Assert.False(
            Anglex.AngleIsBetween(9999f, min: 0f, max: 0f)
        );
    }
#endregion
}
