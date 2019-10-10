
using System.Diagnostics;

namespace Zenworks.Utils {
    public interface ILogger {
        void Info(string log);
    }

    public class Logger : ILogger {
        public void Info(string log) {
            Debug.Print(log);
        }
    }
}
