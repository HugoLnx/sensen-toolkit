using System.Collections.Generic;

namespace SensenToolkit.Pools
{
    public interface ISimplePool<T>
    {
        T Get();
        HashSet<T> Creations { get; }
    }
}
