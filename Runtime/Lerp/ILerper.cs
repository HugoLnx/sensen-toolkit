namespace SensenToolkit.Lerp
{
    public interface ILerper<T>
    {
        T Lerp(T v1, T v2, float t);
        T LerpUnclamped(T v1, T v2, float t);
    }
}
