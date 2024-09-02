#if DOTWEEN
using DG.Tweening;

namespace SensenToolkit.Lerp
{
    public class SpecificEaseLerper : ILerper<float>
    {
        private readonly Ease _ease;
        private readonly EaseLerper _curve;

        public SpecificEaseLerper(Ease ease)
        {
            _ease = ease;
            _curve = new EaseLerper();
        }

        public float Lerp(float v1, float v2, float t)
        {
            return _curve.Lerp(v1, v2, t, _ease);
        }

        public float LerpUnclamped(float v1, float v2, float t)
        {
            return _curve.LerpUnclamped(v1, v2, t, _ease);
        }
    }
}
#endif
