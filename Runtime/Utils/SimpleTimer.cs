using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SensenToolkit
{
    public class SimpleTimer
    {
        private float _durationSecs;

        public bool IsStopped => !IsRunning;
        public bool HasEnded { get; private set; } = false;
        public bool IsRunning => !IsPaused && RemainingTime > 0;
        public bool IsPaused => _delayedResetCancellation == null && RemainingTime > 0;
        public float RemainingTime { get; private set; } = 0f;

        private CancellationTokenSource _delayedResetCancellation;

        public SimpleTimer(float cooldownSecs)
        {
            _durationSecs = cooldownSecs;
        }

        public SimpleTimer Restart()
        {
            HasEnded = false;
            RemainingTime = _durationSecs;
            Resume();
            return this;
        }

        public SimpleTimer Stop()
        {
            HasEnded = false;
            Pause();
            RemainingTime = 0f;
            return this;
        }

        public SimpleTimer Reset() => Stop();

        public SimpleTimer Resume()
        {
            if (_delayedResetCancellation != null) return this;
            if (RemainingTime <= 0) throw new InvalidOperationException("Timer is not running");

            _delayedResetCancellation = new CancellationTokenSource();
            UniTask.Void(DelayedReset, _delayedResetCancellation.Token);
            return this;
        }

        public SimpleTimer Pause()
        {
            _delayedResetCancellation?.Cancel();
            _delayedResetCancellation = null;
            return this;
        }

        private async UniTaskVoid DelayedReset(CancellationToken cancelToken)
        {
            while (RemainingTime > 0)
            {
                await UniTask.NextFrame(cancelToken);
                RemainingTime -= Time.deltaTime;
            }
            Stop();
            HasEnded = true;
        }

        public void SetDurationSecs(float durationSecs)
        {
            _durationSecs = durationSecs;
        }
    }
}
