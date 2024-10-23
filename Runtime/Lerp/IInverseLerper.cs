namespace SensenToolkit
{
    public interface IInverseLerper<T>
    {
        float InverseLerp(T v1, T v2, T v);
        float InverseUnclampedLerp(T v1, T v2, T v);
    }
}
