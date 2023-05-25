namespace SensenToolkit.Pools
{
    public interface ISimplePool<T>
    {
        T Get();
        void Release(T resource);
    }
}
