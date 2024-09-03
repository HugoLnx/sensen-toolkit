using SensenToolkit.Internal;

namespace SensenToolkit
{
    [TransientSingleton]
    public abstract class ANamedTransientSingleton<T> : ANamedBaseSingleton<T>
    where T : ANamedTransientSingleton<T>
    {
    }
}
