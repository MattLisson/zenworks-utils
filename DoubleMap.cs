using System.Collections.Generic;

namespace Zenworks.Utils {

    public class DoubleMap<TKey, TValue> {
        private readonly Dictionary<TKey, TValue> forward = new Dictionary<TKey, TValue>();
        private readonly Dictionary<TValue, TKey> backward = new Dictionary<TValue, TKey>();
        private readonly object syncLock = new object();

        public void Add(TKey a, TValue b) {
            lock (syncLock) {
                forward[a] = b;
                backward[b] = a;
            }
        }

        public TValue Get(TKey a) {
            lock (syncLock) {
                return forward[a];
            }
        }

        public TKey Get(TValue b) {
            lock (syncLock) {
                return backward[b];
            }
        }

        public bool Has(TKey a) {
            lock (syncLock) {
                return forward.ContainsKey(a);
            }
        }
        public bool Has(TValue b) {
            lock (syncLock) {
                return backward.ContainsKey(b);
            }
        }

        public void Remove(TKey a) {
            lock (syncLock) {
                if (Has(a)) {
                    TValue b = Get(a);
                    forward.Remove(a);
                    backward.Remove(b);
                }
            }
        }

        public void Remove(TValue b) {
            lock (syncLock) {
                if (Has(b)) {
                    TKey a = Get(b);
                    forward.Remove(a);
                    backward.Remove(b);
                }
            }
        }

        public void Clear() {
            lock (syncLock) {
                forward.Clear();
                backward.Clear();
            }
        }
    }
}
