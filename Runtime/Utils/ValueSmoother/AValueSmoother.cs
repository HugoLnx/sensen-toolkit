using System;
using MyBox;
using UnityEngine;

namespace SensenToolkit
{
    public abstract class AValueSmoother<T>
    {
        private const int SlotDurationMillis = 16; // MAX 60 FPS
        private T[] _buffer;
        private int _timeMillis;
        private int _bufferDurationMillis;
        private float _bufferDurationSecs;
        private T _sum;
        private T _slotOriginalValue;
        private float _increaseTimeMultiplier;
        private float _decreaseTimeMultiplier;
        private float? _decreaseThreshold;
        private SafeLoop _safeLoop = new();

        public T SmoothedValue => Multiply(_sum, 1f / _bufferDurationMillis);

        public AValueSmoother(
            float increaseDurationSecs,
            float? decreaseDurationSecs = null,
            float? decreaseThreshold = null
        )
        {
            SetConfig(increaseDurationSecs, decreaseDurationSecs, decreaseThreshold);
            object x = Vector3.right;
        }
        protected abstract T Plus(T a, T b);
        protected abstract T Minus(T a, T b);
        protected abstract T Multiply(T a, float b);
        protected abstract float ToFloat(T a);

        public T Push(T val, float deltaTimeSecs)
        {
            float valFloat = ToFloat(val);
            bool isDecreasing = Mathf.Approximately(valFloat, 0f);
            if (!isDecreasing && _decreaseThreshold.HasValue)
            {
                float currentValFloat = ToFloat(Multiply(_sum, 1f / _bufferDurationMillis));
                isDecreasing = valFloat - currentValFloat < -_decreaseThreshold.Value;
            }
            float multiplier = isDecreasing ? _decreaseTimeMultiplier : _increaseTimeMultiplier;
            if (Mathf.Approximately(multiplier, 0f))
            {
                multiplier = _bufferDurationSecs / deltaTimeSecs;
            }

            int deltaTimeMillis = Mathf.FloorToInt(deltaTimeSecs * 1000f * multiplier);
            int tmpDeltaTimeMillis = deltaTimeMillis;
            _safeLoop.Reset();
            while (tmpDeltaTimeMillis > 0)
            {
                _safeLoop.Count();
                int slot = Mathf.FloorToInt(_timeMillis / (float)SlotDurationMillis);
                int usedSlotMillis = _timeMillis % SlotDurationMillis;
                int missingSlotMillis = SlotDurationMillis - usedSlotMillis;

                bool isBeginningOfSlot = usedSlotMillis == 0;
                if (isBeginningOfSlot) _slotOriginalValue = _buffer[slot];

                int stepMillis = Mathf.Min(tmpDeltaTimeMillis, missingSlotMillis);

                T consumed = Multiply(_slotOriginalValue, stepMillis / (float)SlotDurationMillis);
                _sum = Minus(_sum, consumed);
                _buffer[slot] = Minus(_buffer[slot], consumed);

                T stepValue = Multiply(val, stepMillis);
                _sum = Plus(_sum, stepValue);
                _buffer[slot] = Plus(_buffer[slot], stepValue);

                _timeMillis = (_timeMillis + stepMillis) % _bufferDurationMillis;
                tmpDeltaTimeMillis -= stepMillis;
            }
            return SmoothedValue;
        }

        public void SetConfig(
            float increaseDurationSecs,
            float? decreaseDurationSecs = null,
            float? decreaseThreshold = null
        )
        {
            _decreaseThreshold = decreaseThreshold;

            decreaseDurationSecs ??= increaseDurationSecs;
            float durationSecs = Mathf.Max(increaseDurationSecs, decreaseDurationSecs.Value);

            _increaseTimeMultiplier = Mathf.Approximately(increaseDurationSecs, 0f)
                ? 0f
                : durationSecs / increaseDurationSecs;
            _decreaseTimeMultiplier = Mathf.Approximately(decreaseDurationSecs.Value, 0f)
                ? 0f
                : durationSecs / decreaseDurationSecs.Value;
            int bufferTotalSlots = Mathf.RoundToInt(durationSecs * 1000f / (float)SlotDurationMillis);
            if (_buffer != null && bufferTotalSlots == _buffer.Length) return;
            int bufferDurationMillis = SlotDurationMillis * bufferTotalSlots;
            _bufferDurationMillis = bufferDurationMillis;
            _bufferDurationSecs = bufferDurationMillis / 1000f;
            _buffer = new T[bufferTotalSlots];
            _timeMillis = 0;
        }

        public void Reset()
        {
            _buffer.FillBy((_) => default);
            _timeMillis = 0;
            _sum = default;
        }
    }
}
