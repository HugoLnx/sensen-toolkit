namespace SensenToolkit
{
    public abstract class AValueSmoother<T>
    {
        private int _bufferSize;
        private T[] _buffer;
        private int _bufferIndex;
        public T SmoothedValue { get; private set; } = default;

        protected abstract T Plus(T a, T b);
        protected abstract T Minus(T a, T b);
        protected abstract T Divide(T a, int b);

        public AValueSmoother(int bufferSize, T defaultSmoothedValue = default)
        {
            SetBufferSize(bufferSize);
            SmoothedValue = defaultSmoothedValue;
        }

        public void SetBufferSize(int bufferSize)
        {
            if (_bufferSize == bufferSize) return;
            _bufferSize = bufferSize;
            _buffer = new T[_bufferSize];
            _bufferIndex = 0;
        }

        public void AddValue(T value)
        {
            T valAvg = Divide(value, _bufferSize);
            T oldAvg = _buffer[_bufferIndex];
            SmoothedValue = Plus(SmoothedValue, valAvg);
            SmoothedValue = Minus(SmoothedValue, oldAvg);
            _buffer[_bufferIndex] = valAvg;
            _bufferIndex = (_bufferIndex + 1) % _buffer.Length;
        }
    }
}
