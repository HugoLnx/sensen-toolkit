using System.Diagnostics;

namespace SensenToolkit
{
    public struct SafeLoop
    {
        private const int DefaultMaxIterations = 1000;
        private int _maxIterations;
        private int _interactions;

        public SafeLoop(int maxIterations)
        {
            _maxIterations = maxIterations;
            _interactions = 0;
        }

        [Conditional("UNITY_EDITOR")]
        public void Count()
        {
            if (_maxIterations <= 0) _maxIterations = DefaultMaxIterations;
            _interactions++;
            if (_interactions > _maxIterations)
            {
                _interactions = 0;
                UnityEngine.Debug.LogError("SafeLoop: Max iterations reached. Exiting loop.");
                UnityEngine.Debug.Break();
            }
        }

        [Conditional("UNITY_EDITOR")]
        public void Reset()
        {
            _interactions = 0;
        }
    }
}
