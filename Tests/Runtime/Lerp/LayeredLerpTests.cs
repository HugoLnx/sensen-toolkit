using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SensenToolkit.Lerp;
using UnityEngine;
using UnityEngine.TestTools;

public class LayeredLerpTests
{
    private const float Precision = 1e-3f;

    [Test]
    public void Lerp_CanLerpBetween6Values()
    {
        LayeredLerp<float> layeredLerp = new LayeredLerp<float>()
        .SetLerper(FloatLerper.Instance)
        .AddLayer(target: -5f)
        .AddLayer(target: -1f)
        .AddLayer(target: 1f)
        .AddLayer(target: 2f)
        .AddLayer(target: 5f)
        .AddLayer(target: 10f);
        Assert.That(layeredLerp.Lerp(0f), Is.EqualTo(-5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.1f), Is.EqualTo(-3f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.2f), Is.EqualTo(-1f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.3f), Is.EqualTo(0f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.4f), Is.EqualTo(1f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.5f), Is.EqualTo(1.5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.6f), Is.EqualTo(2f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.7f), Is.EqualTo(3.5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.8f), Is.EqualTo(5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.9f), Is.EqualTo(7.5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(1f), Is.EqualTo(10f).Within(Precision));
    }

    [Test]
    public void Lerp_ReturnMinMaxValues_WhenInterpolationValueIsHigherOrLowerThanTheLimits()
    {
        LayeredLerp<float> layeredLerp = new LayeredLerp<float>()
        .SetLerper(FloatLerper.Instance)
        .AddLayer(target: -5f)
        .AddLayer(target: -1f)
        .AddLayer(target: 1f)
        .AddLayer(target: 2f)
        .AddLayer(target: 5f)
        .AddLayer(target: 10f);
        Assert.That(layeredLerp.Lerp(-1f), Is.EqualTo(-5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(2f), Is.EqualTo(10f).Within(Precision));
    }

    [Test]
    public void Lerp_CanSpecifyTheInterpolationDistanceBetweenValues()
    {
        LayeredLerp<float> layeredLerp = new LayeredLerp<float>()
        .SetLerper(FloatLerper.Instance)
        .AddLayer(target: -5f, t: 0f)
        .AddLayer(target: -1f, t: 0.5f)
        .AddLayer(target: 1f, t: 0.6f)
        .AddLayer(target: 2f, t: 0.7f)
        .AddLayer(target: 5f, t: 0.8f)
        .AddLayer(target: 10f, t: 1f);
        Assert.That(layeredLerp.Lerp(0f), Is.EqualTo(-5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.25f), Is.EqualTo(-3f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.5f), Is.EqualTo(-1f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.55f), Is.EqualTo(0f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.6f), Is.EqualTo(1f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.65f), Is.EqualTo(1.5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.7f), Is.EqualTo(2f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.75f), Is.EqualTo(3.5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.8f), Is.EqualTo(5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.9f), Is.EqualTo(7.5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(1f), Is.EqualTo(10f).Within(Precision));
    }

    [Test]
    public void Lerp_Weight_MultiplyTheDistanceBetweenLayers_WhenInterpolationMultiplyIsHigherThan1()
    {
        LayeredLerp<float> layeredLerp = new LayeredLerp<float>()
        .SetLerper(FloatLerper.Instance)
        .AddLayer(target: -5f)
        .AddLayer(target: -1f, weight: 2f)
        .AddLayer(target: 2f);
        Assert.That(layeredLerp.Lerp(0f), Is.EqualTo(-5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.25f), Is.EqualTo(-1f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.5f), Is.EqualTo(3f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.75f), Is.EqualTo(2.5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(1f), Is.EqualTo(2f).Within(Precision));
    }

    [Test]
    public void Lerp_Weight_DividesTheDistanceBetweenLayers_WhenInterpolationMultiplyIsBetween0And1()
    {
        LayeredLerp<float> layeredLerp = new LayeredLerp<float>()
        .SetLerper(FloatLerper.Instance)
        .AddLayer(target: -5f)
        .AddLayer(target: -1f, weight: 0.5f)
        .AddLayer(target: 2f);
        Assert.That(layeredLerp.Lerp(0f), Is.EqualTo(-5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.25f), Is.EqualTo(-4f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.5f), Is.EqualTo(-3f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.75f), Is.EqualTo(-0.5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(1f), Is.EqualTo(2f).Within(Precision));
    }

    [Test]
    public void Lerp_Weight_MustSetInitialWeightTargetReference_WhenFirstLayerHasWeight()
    {
        LayeredLerp<float> layeredLerp = new LayeredLerp<float>()
        .SetLerper(FloatLerper.Instance)
        .SetInitialWeightTargetReference(-4f)
        .AddLayer(target: -5f, weight: 2f)
        .AddLayer(target: 2f);
        Assert.That(layeredLerp.Lerp(0f), Is.EqualTo(-6f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0.5f), Is.EqualTo(-2f).Within(Precision));
        Assert.That(layeredLerp.Lerp(1f), Is.EqualTo(2f).Within(Precision));
    }

    [Test]
    public void Lerp_OnCustomRange_CanLerpBetween6Values()
    {
        LayeredLerp<float> layeredLerp = new LayeredLerp<float>()
        .SetLerper(FloatLerper.Instance)
        .SetTRange(-50f, 50f)
        .AddLayer(target: -5f)
        .AddLayer(target: -1f)
        .AddLayer(target: 1f)
        .AddLayer(target: 2f)
        .AddLayer(target: 5f)
        .AddLayer(target: 10f);
        Assert.That(layeredLerp.Lerp(-50f), Is.EqualTo(-5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(-40f), Is.EqualTo(-3f).Within(Precision));
        Assert.That(layeredLerp.Lerp(-30f), Is.EqualTo(-1f).Within(Precision));
        Assert.That(layeredLerp.Lerp(-20f), Is.EqualTo(0f).Within(Precision));
        Assert.That(layeredLerp.Lerp(-10f), Is.EqualTo(1f).Within(Precision));
        Assert.That(layeredLerp.Lerp(0f), Is.EqualTo(1.5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(10f), Is.EqualTo(2f).Within(Precision));
        Assert.That(layeredLerp.Lerp(20f), Is.EqualTo(3.5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(30f), Is.EqualTo(5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(40f), Is.EqualTo(7.5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(50f), Is.EqualTo(10f).Within(Precision));
    }

    [Test]
    public void Lerp_OnCustomRange_ReturnMinMaxValues_WhenInterpolationValueIsHigherOrLowerThanTheLimits()
    {
        LayeredLerp<float> layeredLerp = new LayeredLerp<float>()
        .SetTRange(-50f, 50f)
        .SetLerper(FloatLerper.Instance)
        .AddLayer(target: -5f)
        .AddLayer(target: -1f)
        .AddLayer(target: 1f)
        .AddLayer(target: 2f)
        .AddLayer(target: 5f)
        .AddLayer(target: 10f);
        Assert.That(layeredLerp.Lerp(-60f), Is.EqualTo(-5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(100f), Is.EqualTo(10f).Within(Precision));
    }

    [Test]
    public void Lerp_ComplexScenarioUsingAllOptions()
    {
        LayeredLerp<float> layeredLerp = new LayeredLerp<float>()
        .SetLerper(FloatLerper.Instance)
        .SetTRange(-50f, 50f)
        .SetInitialWeightTargetReference(-15f)
        .AddLayer(target: -5f, t: 0f, weight: 2f)
        .AddLayer(target: -1f, t: 0.1f)
        .AddLayer(target: 1f, t: 0.2f, weight: 0.25f)
        .AddLayer(target: 2f, t: 0.3f, weight: 0.2f)
        .AddLayer(target: 5f, t: 0.4f)
        .AddLayer(target: 10f, t: 1f);
        Assert.That(layeredLerp.Lerp(-50f), Is.EqualTo(5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(-45f), Is.EqualTo(2f).Within(Precision));
        Assert.That(layeredLerp.Lerp(-40f), Is.EqualTo(-1f).Within(Precision));
        Assert.That(layeredLerp.Lerp(-35f), Is.EqualTo(-0.75f).Within(Precision));
        Assert.That(layeredLerp.Lerp(-30f), Is.EqualTo(-0.5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(-25f), Is.EqualTo(-0.25f).Within(Precision));
        Assert.That(layeredLerp.Lerp(-20f), Is.EqualTo(0f).Within(Precision));
        Assert.That(layeredLerp.Lerp(-15f), Is.EqualTo(2.5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(-10f), Is.EqualTo(5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(20f), Is.EqualTo(7.5f).Within(Precision));
        Assert.That(layeredLerp.Lerp(50f), Is.EqualTo(10f).Within(Precision));
    }
}
