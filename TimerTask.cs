using System;
using System.Threading;
using System.Threading.Tasks;
using NodaTime;

namespace Zenworks.Utils {

    public interface IPeriodicTask : IDisposable {
        Duration Period { get; set; }
        Duration RateLimit { get; set; }

        event Action OnFire;

        bool IsRunning { get; }

        void Start();
        void Pause();

        bool MaybeSkipFiring();
    }

    public class TimerTask : IPeriodicTask, IDisposable {

        public event Action? OnFire;
        public Duration Period { get; set; }
        public Duration RateLimit { get; set; } = Duration.FromSeconds(1);
        public bool IsRunning { get; private set; }

        private IClock clock;
        private Instant lastExecuted = Instant.MinValue;
        private readonly object @lock = new object();

        public TimerTask(IClock clock, Duration period, bool startImmediately = false) {
            this.clock = clock;
            Period = period;
            if (startImmediately) {
                Start();
            }
        }

        public async void Start() {
            if (IsRunning) {
                return;
            }
            IsRunning = true;
            while (IsRunning) {
                await Task.Delay((int)Period.TotalMilliseconds).SameThread();
                MaybeFireEvent();
            }
        }


        public bool MaybeSkipFiring() {
            Instant now = clock.GetCurrentInstant();
            if (Monitor.TryEnter(@lock)) {
                try {
                    if (IsRunning && now - lastExecuted > RateLimit) {
                        lastExecuted = now;
                        return true;
                    }
                } finally {
                    Monitor.Exit(@lock);
                }
            }
            return false;
        }

        private void MaybeFireEvent() {
            Instant now = clock.GetCurrentInstant();
            
            if (Monitor.TryEnter(@lock)) {
                try {
                    if (IsRunning && now - lastExecuted > RateLimit) {
                        OnFire?.Invoke();
                        lastExecuted = now;
                    }
                } finally {
                    Monitor.Exit(@lock);
                }
            }
            // Else do nothing, already executing.
        }

        public void Pause() {
            IsRunning = false;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    Pause();
                }
                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
