using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Zenworks.Utils {
    public static class TaskUtils {
        public static ConfiguredTaskAwaitable SameThread(this Task task) {
            return task.ConfigureAwait(true);
        }

        public static ConfiguredTaskAwaitable ReleaseThread(this Task task) {
            return task.ConfigureAwait(false);
        }
        public static ConfiguredTaskAwaitable<T> SameThread<T>(this Task<T> task) {
            return task.ConfigureAwait(true);
        }

        public static ConfiguredTaskAwaitable<T> ReleaseThread<T>(this Task<T> task) {
            return task.ConfigureAwait(false);
        }
    }
}
