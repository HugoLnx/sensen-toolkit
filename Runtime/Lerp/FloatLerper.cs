using UnityEngine;

namespace SensenToolkit.Lerp
{
    public class FloatLerper : IFullLerper<float>
    {
        public readonly static FloatLerper Instance = new();
        public float Lerp(float v1, float v2, float t)
        {
            return Mathf.Lerp(v1, v2, t);
        }
        public float LerpUnclamped(float v1, float v2, float t)
        {
            return v1 + ((v2 - v1) * t);
        }
        public float InverseLerp(float v1, float v2, float v)
        {
            return Mathf.InverseLerp(v1, v2, v);
        }
        public float InverseUnclampedLerp(float v1, float v2, float v)
        {
            return (v - v1) / (v2 - v1);
        }
    }
}
