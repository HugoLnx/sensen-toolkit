namespace SensenToolkit
{
    public interface IReleasablePool<T> : ISimplePool<T>
    {
        void Release(T resource);
    }
}
