using System.Collections.Generic;

namespace SensenToolkit
{
    public interface ISimplePool<T>
    {
        T Get();
        HashSet<T> Creations { get; }
    }
}
