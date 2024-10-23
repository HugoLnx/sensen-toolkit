using UnityEngine;

namespace SensenToolkit
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
