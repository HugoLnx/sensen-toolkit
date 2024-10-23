using UnityEngine;

namespace SensenToolkit
{
    public class Vector2RotationLerper : IFullLerper<Vector2>
    {
        public readonly static Vector2RotationLerper Instance = new();
        public Vector2 Lerp(Vector2 v1, Vector2 v2, float t)
        {
            return LerpUnclamped(v1, v2, Mathf.Clamp01(t));
        }
        public Vector2 LerpUnclamped(Vector2 v1, Vector2 v2, float t)
        {
            float fullRotation = Vector2.SignedAngle(v1, v2);
            float rotation = FloatLerper.Instance.LerpUnclamped(0f, fullRotation, t);
            return Math2Dx.RotateBy(rotation, v1);
        }
        public float InverseLerp(Vector2 v1, Vector2 v2, Vector2 v)
        {
            return Mathf.Clamp01(InverseUnclampedLerp(v1, v2, v));
        }
        public float InverseUnclampedLerp(Vector2 v1, Vector2 v2, Vector2 v)
        {
            float v1ToV2 = Anglex.NormalizeAngle(Vector2.SignedAngle(v1, v2));
            float v1ToV = Anglex.NormalizeAngle(Vector2.SignedAngle(v1, v));
            return v1ToV / v1ToV2;
        }
    }
}
