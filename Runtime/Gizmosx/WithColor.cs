using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SensenToolkit.Gizmosx
{
    public class WithColor : System.IDisposable
    {
        private Color _originalColor;

        public WithColor UseColor(Color color)
        {
            _originalColor = Gizmos.color;
            Gizmos.color = color;
            return this;
        }

        public void Dispose()
        {
            Gizmos.color = _originalColor;
        }
    }
}
