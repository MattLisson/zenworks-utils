using System;
using System.Collections.Generic;
using NodaTime;

namespace Zenworks.Utils {

    public interface IProfiler {

        void MarkEvent(string name);

        void StartBlock(string name);

        void EndBlock(string name);
    }

    public struct Sample : IEquatable<Sample> {
        Instant Timestamp { get; }
        string Name { get; }
        int Indent { get; }

        public Sample(Instant timestamp, string name, int indent) {
            Timestamp = timestamp;
            Name = name;
            Indent = indent;
        }

        public override string ToString() {
            return $"{Timestamp.ToString("HH:mm:ss:fff", null)}{":".PadRight(Indent * 4)}{Name}";
        }
        #region Equality
        public override bool Equals(object? obj) {
            return obj is Sample sample && Equals(sample);
        }

        public bool Equals(Sample other) {
            return Timestamp.Equals(other.Timestamp) &&
                   Name == other.Name &&
                   Indent == other.Indent;
        }

        public override int GetHashCode() {
            var hashCode = -1765381564;
            hashCode = hashCode * -1521134295 + EqualityComparer<Instant>.Default.GetHashCode(Timestamp);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Indent.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Sample left, Sample right) {
            return left.Equals(right);
        }

        public static bool operator !=(Sample left, Sample right) {
            return !(left == right);
        }
        #endregion
    }

    public class NodaTimeProfiler : IProfiler {
        public static NodaTimeProfiler Instance { get; } = new NodaTimeProfiler(SystemClock.Instance);

        private readonly IClock clock;

        private readonly List<Sample> samples = new List<Sample>(1000);
        private int currentIndent = 0;
        public NodaTimeProfiler(IClock clock) {
            this.clock = clock;
        }
        public void MarkEvent(string name) {
            samples.Add(new Sample(clock.GetCurrentInstant(), name, currentIndent));
        }

        public void EndBlock(string name) {
            samples.Add(new Sample(clock.GetCurrentInstant(), $"End: {name}", currentIndent));
            currentIndent--;
        }

        public void StartBlock(string name) {
            currentIndent++;
            samples.Add(new Sample(clock.GetCurrentInstant(), $"Start: {name}", currentIndent));
        }

        public override string ToString() {
            return string.Join("\n", samples);
        }
    }
}
