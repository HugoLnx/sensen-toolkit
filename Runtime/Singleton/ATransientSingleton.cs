using SensenToolkit.Internal;
using UnityEngine;

namespace SensenToolkit
{
    [TransientSingleton]
    public abstract class ATransientSingleton<T> : ABaseSingleton<T>
    where T : ATransientSingleton<T>
    {
    }
}
