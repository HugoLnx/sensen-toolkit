using System;
using SensenToolkit.Internal;

namespace SensenToolkit
{
    public abstract class APermanentSingleton<T> : ABaseSingleton<T>
    where T : APermanentSingleton<T>
    {
    }
}
