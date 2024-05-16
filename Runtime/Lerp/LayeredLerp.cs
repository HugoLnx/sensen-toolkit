using System.Collections;
using System.Collections.Generic;
using SensenToolkit.Mathx;
using UnityEngine;

namespace SensenToolkit.Lerp
{
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
                val = _lerper.LerpUnclamped(_initialWeightTargetReference, _layers[0].Target, _layers[0].Weight);
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
                T target = _lerper.LerpUnclamped(val, layer.Target, layer.Weight);
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
