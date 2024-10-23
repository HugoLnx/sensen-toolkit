#if DOTWEEN
using System;
using DG.Tweening;
using UnityEngine;

namespace SensenToolkit
{
    public class EaseLerper
    {
        private float _value;
        private Tween _tween;

        public EaseLerper()
        {
            _tween = DOTween.To(
                getter: () => _value,
                setter: (x) => _value = x,
                endValue: 1f,
                duration: 1f
            )
            .From(0f)
            .SetUpdate(UpdateType.Manual, isIndependentUpdate: true)
            .SetAutoKill(false);
        }

        public float Lerp(float a, float b, float t, Ease ease = Ease.Linear)
        {
            float easedT = ApplyEase(t, ease);
            return Mathf.Lerp(a, b, easedT);
        }

        public float LerpUnclamped(float a, float b, float t, Ease ease = Ease.Linear)
        {
            float easedT = ApplyEase(t, ease);
            return Mathf.LerpUnclamped(a, b, easedT);
        }

        private float ApplyEase(float t, Ease ease)
        {
            return ease switch
            {
                Ease.Unset => t,
                Ease.Linear => t,
                _ => ApplyEaseWithTweener(t, ease)
            };
        }

        private float ApplyEaseWithTweener(float t, Ease ease)
        {
            _tween.Restart();
            _tween.SetEase(ease);
            _tween.ManualUpdate(t, t);
            return _value;
        }
    }
}
#endif
