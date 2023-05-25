using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SensenToolkit.Gizmosx
{
    public static class Gizmosx
    {
        public static readonly WithColor WithColorInstance = new();
        public static WithColor Color(Color color)
        {
            // TODO: Use a pool of WithColorInstance instead of a single instance
            // to avoid problem with multiple objects / coroutines using it
            return WithColorInstance.UseColor(color);
        }
    }
}
