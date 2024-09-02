#if DOTWEEN
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace Sensen.Toolkit
{
    public static class SimpleTweening
    {
        public static Tween FromTo(
            System.Action<float> action,
            float duration,
            float? startValue = null,
            float endValue = 1f,
            bool setImmediately = true
        )
        {
            float gv = startValue ?? 0f;
            TweenerCore<float, float, FloatOptions> tween = DOTween.To(
                getter: () => gv,
                setter: v =>
                {
                    gv = v;
                    action(gv);
                },
                endValue: endValue,
                duration: duration
            );
            if (startValue.HasValue)
            {
                tween = tween.From(startValue.Value, setImmediately: setImmediately);
            }
            return tween;
        }

        public static Sequence Flash(
            System.Action<float> action,
            float durationIn = 0.1f,
            float durationOut = 0.1f,
            float lowValue = 0f,
            float highValue = 1f,
            Ease easeIn = Ease.OutSine,
            Ease easeOut = Ease.InSine
        )
        {
            return DOTween.Sequence()
                .Append(FromTo(action, duration: durationIn, endValue: highValue, startValue: lowValue).SetEase(easeIn))
                .Append(FromTo(action, duration: durationOut, endValue: lowValue, startValue: highValue).SetEase(easeOut))
                .Play();
        }
    }
}
#endif
