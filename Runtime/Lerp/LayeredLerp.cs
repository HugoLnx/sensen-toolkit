using System;
using System.Collections;
using System.Collections.Generic;
using SensenToolkit.Mathx;
using UnityEngine;

namespace SensenToolkit.Lerp
{
    public interface ILerper<T>
    {
        T Lerp(T v1, T v2, float t);
        T SuperLerp(T v1, T v2, float t);
        float InverseLerp(T v1, T v2, T v);
        float InverseSuperLerp(T v1, T v2, T v);
    }

    public class Vector2RotationLerper : ILerper<Vector2>
    {
        public readonly static Vector2RotationLerper Instance = new();
        public Vector2 Lerp(Vector2 v1, Vector2 v2, float t)
        {
            return SuperLerp(v1, v2, Mathf.Clamp01(t));
        }
        public Vector2 SuperLerp(Vector2 v1, Vector2 v2, float t)
        {
            float fullRotation = Vector2.SignedAngle(v1, v2);
            float rotation = FloatLerper.Instance.SuperLerp(0f, fullRotation, t);
            return Math2Dx.RotateBy(rotation, v1);
        }
        public float InverseLerp(Vector2 v1, Vector2 v2, Vector2 v)
        {
            return Mathf.Clamp01(InverseSuperLerp(v1, v2, v));
        }
        public float InverseSuperLerp(Vector2 v1, Vector2 v2, Vector2 v)
        {
            float v1ToV2 = Anglex.NormalizeAngle(Vector2.SignedAngle(v1, v2));
            float v1ToV = Anglex.NormalizeAngle(Vector2.SignedAngle(v1, v));
            return v1ToV / v1ToV2;
        }
    }

    public class FloatLerper : ILerper<float>
    {
        public readonly static FloatLerper Instance = new();
        public float Lerp(float v1, float v2, float t)
        {
            return Mathf.Lerp(v1, v2, t);
        }
        public float SuperLerp(float v1, float v2, float t)
        {
            return v1 + ((v2 - v1) * t);
        }
        public float InverseLerp(float v1, float v2, float v)
        {
            return Mathf.InverseLerp(v1, v2, v);
        }
        public float InverseSuperLerp(float v1, float v2, float v)
        {
            return (v - v1) / (v2 - v1);
        }
    }

    public class LayeredLerpBuildException : Exception
    {
        public LayeredLerpBuildException(string message) : base(message)
        {
        }

        public LayeredLerpBuildException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    // TODO: Make it Garbageless
    // - Make tests for that class
    // - Use pool for LayeredLerp
    // - Use pool for LerpLayer
    public class LerpLayer<T>
    {
        public T Target { get; set; }
        public float RawInterpolationValue { get; set; }
        public float Weight { get; set; }
        public int Inx { get; internal set; }

        public float GetInterpolationValue(int layersCount)
        {
            if (Floatx.IsGreaterOrEqual(RawInterpolationValue, 0f))
            {
                return RawInterpolationValue;
            }
            float avg = 1f / (float) (layersCount - 1);
            return avg * (float) Inx;
        }
    }
    public class LayeredLerp<T>
    {
        private List<LerpLayer<T>> _layers = new();
        private ILerper<T> _lerper;
        private float _tAbsMin = 0f;
        private float _tAbsMax = 1f;
        private T _initialWeightTargetReference;

        public LayeredLerp<T> SetTRange(float min = 0f, float max = 1f)
        {
            _tAbsMin = min;
            _tAbsMax = max;
            return this;
        }

        public LayeredLerp<T> Reset()
        {
            _layers.Clear();
            _lerper = null;
            _tAbsMin = 0f;
            _tAbsMax = 1f;
            return this;
        }
        public LayeredLerp<T> SetLerper(ILerper<T> lerper)
        {
            _lerper = lerper;
            return this;
        }
        public LayeredLerp<T> SetInitialWeightTargetReference(T targetReference)
        {
            _initialWeightTargetReference = targetReference;
            return this;
        }

        public LayeredLerp<T> AddLayer(T target, float t = -1f, float weight = 1f)
        {
            _layers.Add(new LerpLayer<T> {
                Target = target,
                RawInterpolationValue = t,
                Weight = weight,
                Inx = _layers.Count
            });
            return this;
        }

        public T Lerp(float targetTAbs)
        {
            float targetT = Mathf.InverseLerp(_tAbsMin, _tAbsMax, targetTAbs);
            T val = _layers[0].Target;
            if (Floatx.IsNotEqual(_layers[0].Weight, 1f))
            {
                if (_initialWeightTargetReference == null)
                {
                    throw new LayeredLerpBuildException("Initial weight target reference must be set if first layer has weight");
                }
                val = _lerper.SuperLerp(_initialWeightTargetReference, _layers[0].Target, _layers[0].Weight);
            }
            for (int i = 1; i < _layers.Count; i++)
            {
                LerpLayer<T> prevLayer = _layers[i - 1];
                LerpLayer<T> layer = _layers[i];
                float tRangeBegin = prevLayer.GetInterpolationValue(_layers.Count);
                float tRangeEnd = layer.GetInterpolationValue(_layers.Count);
                float tRangeVal = Mathf.Min(tRangeEnd, targetT);
                float tRange = Mathf.InverseLerp(tRangeBegin, tRangeEnd, tRangeVal);
                //float t = FloatLerper.Instance.SuperLerp(0f, layer.Weight, tRange);
                T target = _lerper.SuperLerp(val, layer.Target, layer.Weight);
                val = _lerper.Lerp(
                    val,
                    target, // layer.Target,
                    tRange // t
                );
                if (Floatx.IsBetween(targetT, min: tRangeBegin, max: tRangeEnd))
                {
                    return val;
                }
            }
            return val;
        }
    }
}
