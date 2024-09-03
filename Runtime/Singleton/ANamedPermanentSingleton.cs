using SensenToolkit.Internal;

namespace SensenToolkit
{
    public abstract class ANamedPermanentSingleton<T> : ANamedBaseSingleton<T>
    where T : ANamedPermanentSingleton<T>
    {
    }
}
