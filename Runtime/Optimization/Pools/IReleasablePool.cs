namespace SensenToolkit.Pools
{
    public interface IReleasablePool<T> : ISimplePool<T>
    {
        void Release(T resource);
    }
}
