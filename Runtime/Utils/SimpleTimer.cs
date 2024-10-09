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
        public bool IsRunning => !IsPaused && RemainingTime > 0;
        public bool IsPaused => _delayedResetCancellation == null && RemainingTime > 0;
        public float RemainingTime { get; private set; } = 0f;

        private CancellationTokenSource _delayedResetCancellation;

        public SimpleTimer(float cooldownSecs)
        {
            _durationSecs = cooldownSecs;
        }

        public void Restart()
        {
            RemainingTime = _durationSecs;
            Resume();
        }

        public void Stop()
        {
            Pause();
            RemainingTime = 0f;
        }

        public void Resume()
        {
            if (_delayedResetCancellation != null) return;
            if (RemainingTime <= 0) throw new InvalidOperationException("Timer is not running");

            _delayedResetCancellation = new CancellationTokenSource();
            UniTask.Void(DelayedReset, _delayedResetCancellation.Token);
        }

        public void Pause()
        {
            _delayedResetCancellation?.Cancel();
            _delayedResetCancellation = null;
        }

        private async UniTaskVoid DelayedReset(CancellationToken cancelToken)
        {
            while (RemainingTime > 0)
            {
                await UniTask.NextFrame(cancelToken);
                RemainingTime -= Time.deltaTime;
            }
            Stop();
        }

        public void SetDurationSecs(float durationSecs)
        {
            _durationSecs = durationSecs;
        }
    }
}
