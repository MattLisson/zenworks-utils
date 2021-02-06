using System;
using System.Collections.Generic;

namespace Zenworks.Utils {
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
    public struct None { }
    public struct Success { }
    public struct Failure : IEquatable<Failure> {
        public string Message { get; }

        public Failure(string message) {
            Message = message;
        }

        public override bool Equals(object obj) {
            return obj is Failure error && Equals(error);
        }

        public bool Equals(Failure other) {
            return Message == other.Message;
        }

        public override int GetHashCode() {
            return 460171812 + EqualityComparer<string>.Default.GetHashCode(Message);
        }

        public static bool operator ==(Failure left, Failure right) {
            return left.Equals(right);
        }

        public static bool operator !=(Failure left, Failure right) {
            return !(left == right);
        }

        public override string ToString() {
            return $"Error({Message})";
        }
    }

    public interface IOneOf {
        object? Value { get; }
    }
    // CA1000 conflicts with CA2225, I sided with CA2225.
    // CA1066 seems to be bugged, these types do implement IEquatable.
#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA1066 // Type {0} should implement IEquatable<T> because it overrides Equals
    public struct OneOf<T0> : IOneOf, IEquatable<OneOf<T0>> {
        private readonly T0 _value0;
        private readonly int _index;

        private OneOf(int index, T0 value0 = default) {
            _index = index;
            _value0 = value0!;
        }

        public object? Value {
            get {
                switch (_index) {
                    case 0:
                        return _value0;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public bool IsT0 => _index == 0;

        public T0 AsT0 {
            get {
                if (_index != 0) {
                    throw new InvalidOperationException($"Cannot return as T0 as result is T{_index}");
                }
                return _value0;
            }
        }

        public static implicit operator OneOf<T0>(T0 t) {
            return new OneOf<T0>(0, value0: t);
        }

        public void Switch(Action<T0> f0) {
            if (_index == 0 && f0 != null) {
                f0(_value0);
                return;
            }
            throw new InvalidOperationException();
        }

        public TResult Match<TResult>(Func<T0, TResult> f0) {
            if (_index == 0 && f0 != null) {
                return f0(_value0);
            }
            throw new InvalidOperationException();
        }

        public static OneOf<T0> FromT0(T0 input) {
            return input;
        }

        public OneOf<TResult> MapT0<TResult>(Func<T0, TResult> mapFunc) {
            if (mapFunc == null) {
                throw new ArgumentNullException(nameof(mapFunc));
            }
            return Match<OneOf<TResult>>(
                input0 => mapFunc(input0)
            );
        }

        public bool Equals(OneOf<T0> other) {
            if (_index != other._index) {
                return false;
            }
            switch (_index) {
                case 0:
                    return Equals(_value0, other._value0);
                default:
                    return false;
            }
        }

        public override bool Equals(object obj) {
            if (obj is null) {
                return false;
            }

            return obj is OneOf<T0> oneOf && Equals(oneOf);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode;
                switch (_index) {
                    case 0:
                        hashCode = _value0?.GetHashCode() ?? 0;
                        break;
                    default:
                        hashCode = 0;
                        break;
                }
                return (hashCode * 397) ^ _index;
            }
        }

        public static bool operator ==(OneOf<T0> left, OneOf<T0> right) {
            return left.Equals(right);
        }

        public static bool operator !=(OneOf<T0> left, OneOf<T0> right) {
            return !(left == right);
        }
    }

    public struct OneOf<T0, T1> : IOneOf, IEquatable<OneOf<T0, T1>> {
        private readonly T0 _value0;
        private readonly T1 _value1;
        private readonly int _index;

        private OneOf(int index, T0 value0 = default, T1 value1 = default) {
            _index = index;
            _value0 = value0!;
            _value1 = value1!;
        }

        public object? Value {
            get {
                switch (_index) {
                    case 0:
                        return _value0;
                    case 1:
                        return _value1;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public bool IsT0 => _index == 0;

        public T0 AsT0 {
            get {
                if (_index != 0) {
                    throw new InvalidOperationException($"Cannot return as T0 as result is T{_index}");
                }
                return _value0;
            }
        }

        public static implicit operator OneOf<T0, T1>(T0 t) {
            return new OneOf<T0, T1>(0, value0: t);
        }

        public bool IsT1 => _index == 1;

        public T1 AsT1 {
            get {
                if (_index != 1) {
                    throw new InvalidOperationException($"Cannot return as T1 as result is T{_index}");
                }
                return _value1;
            }
        }

        public static implicit operator OneOf<T0, T1>(T1 t) {
            return new OneOf<T0, T1>(1, value1: t);
        }

        public void Switch(Action<T0> f0, Action<T1> f1) {
            if (_index == 0 && f0 != null) {
                f0(_value0);
                return;
            }
            if (_index == 1 && f1 != null) {
                f1(_value1);
                return;
            }
            throw new InvalidOperationException();
        }

        public TResult Match<TResult>(Func<T0, TResult> f0, Func<T1, TResult> f1) {
            if (_index == 0 && f0 != null) {
                return f0(_value0);
            }
            if (_index == 1 && f1 != null) {
                return f1(_value1);
            }
            throw new InvalidOperationException();
        }

        public static OneOf<T0, T1> FromT0(T0 input) {
            return input;
        }

        public static OneOf<T0, T1> FromT1(T1 input) {
            return input;
        }

        public OneOf<TResult, T1> MapT0<TResult>(Func<T0, TResult> mapFunc) {
            if (mapFunc == null) {
                throw new ArgumentNullException(nameof(mapFunc));
            }
            return Match<OneOf<TResult, T1>>(
                input0 => mapFunc(input0),
                input1 => input1
            );
        }

        public OneOf<T0, TResult> MapT1<TResult>(Func<T1, TResult> mapFunc) {
            if (mapFunc == null) {
                throw new ArgumentNullException(nameof(mapFunc));
            }
            return Match<OneOf<T0, TResult>>(
                input0 => input0,
                input1 => mapFunc(input1)
            );
        }

        /// <summary>
        /// Either parameter could be returned as null, obey the if statement.
        /// </summary>
        public bool TryPickT0(out T0 value, out T1 remainder) {
            value = IsT0 ? AsT0 : default!;
            remainder = IsT0 ? default! : AsT1;
            return IsT0;
        }

        /// <summary>
        /// Either parameter could be returned as null, obey the if statement.
        /// </summary>
        public bool TryPickT1(out T1 value, out T0 remainder) {
            value = IsT1 ? AsT1 : default!;
            remainder = IsT1 ? default! : AsT0;
            return IsT1;
        }

        public bool Equals(OneOf<T0, T1> other) {
            if (_index != other._index) {
                return false;
            }
            switch (_index) {
                case 0:
                    return Equals(_value0, other._value0);
                case 1:
                    return Equals(_value1, other._value1);
                default:
                    return false;
            }
        }

        public override bool Equals(object obj) {
            if (obj is null) {
                return false;
            }

            return obj is OneOf<T0, T1> && Equals((OneOf<T0, T1>)obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode;
                switch (_index) {
                    case 0:
                        hashCode = _value0?.GetHashCode() ?? 0;
                        break;
                    case 1:
                        hashCode = _value1?.GetHashCode() ?? 0;
                        break;
                    default:
                        hashCode = 0;
                        break;
                }
                return (hashCode * 397) ^ _index;
            }
        }

        public static bool operator ==(OneOf<T0, T1> left, OneOf<T0, T1> right) {
            return left.Equals(right);
        }

        public static bool operator !=(OneOf<T0, T1> left, OneOf<T0, T1> right) {
            return !(left == right);
        }
    }

    public struct OneOf<T0, T1, T2> : IOneOf, IEquatable<OneOf<T0, T1, T2>> {
        private readonly T0 _value0;
        private readonly T1 _value1;
        private readonly T2 _value2;
        private readonly int _index;

        private OneOf(int index, T0 value0 = default, T1 value1 = default, T2 value2 = default) {
            _index = index;
            _value0 = value0!;
            _value1 = value1!;
            _value2 = value2!;
        }

        public OneOf(T0 value) : this(0, value0: value) { }
        public OneOf(T1 value) : this(1, value1: value) { }
        public OneOf(T2 value) : this(2, value2: value) { }

        public object? Value {
            get {
                switch (_index) {
                    case 0:
                        return _value0;
                    case 1:
                        return _value1;
                    case 2:
                        return _value2;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public bool IsT0 => _index == 0;

        public T0 AsT0 {
            get {
                if (_index != 0) {
                    throw new InvalidOperationException($"Cannot return as T0 as result is T{_index}");
                }
                return _value0;
            }
        }

        public static implicit operator OneOf<T0, T1, T2>(T0 t) {
            return new OneOf<T0, T1, T2>(0, value0: t);
        }

        public bool IsT1 => _index == 1;

        public T1 AsT1 {
            get {
                if (_index != 1) {
                    throw new InvalidOperationException($"Cannot return as T1 as result is T{_index}");
                }
                return _value1;
            }
        }

        public static implicit operator OneOf<T0, T1, T2>(T1 t) {
            return new OneOf<T0, T1, T2>(1, value1: t);
        }

        public bool IsT2 => _index == 2;

        public T2 AsT2 {
            get {
                if (_index != 2) {
                    throw new InvalidOperationException($"Cannot return as T2 as result is T{_index}");
                }
                return _value2;
            }
        }

        public static implicit operator OneOf<T0, T1, T2>(T2 t) {
            return new OneOf<T0, T1, T2>(2, value2: t);
        }

        public static OneOf<T0, T1, T2> FromT0(T0 input) {
            return input;
        }

        public static OneOf<T0, T1, T2> FromT1(T1 input) {
            return input;
        }

        public static OneOf<T0, T1, T2> FromT2(T2 input) {
            return input;
        }

        public void Switch(Action<T0> f0, Action<T1> f1, Action<T2> f2) {
            if (_index == 0 && f0 != null) {
                f0(_value0);
                return;
            }
            if (_index == 1 && f1 != null) {
                f1(_value1);
                return;
            }
            if (_index == 2 && f2 != null) {
                f2(_value2);
                return;
            }
            throw new InvalidOperationException();
        }

        public TResult Match<TResult>(Func<T0, TResult> f0, Func<T1, TResult> f1, Func<T2, TResult> f2) {
            if (_index == 0 && f0 != null) {
                return f0(_value0);
            }
            if (_index == 1 && f1 != null) {
                return f1(_value1);
            }
            if (_index == 2 && f2 != null) {
                return f2(_value2);
            }
            throw new InvalidOperationException();
        }

        public OneOf<TResult, T1, T2> MapT0<TResult>(Func<T0, TResult> mapFunc) {
            if (mapFunc == null) {
                throw new ArgumentNullException(nameof(mapFunc));
            }
            return Match<OneOf<TResult, T1, T2>>(
                input0 => mapFunc(input0),
                input1 => input1,
                input2 => input2
            );
        }

        public OneOf<T0, TResult, T2> MapT1<TResult>(Func<T1, TResult> mapFunc) {
            if (mapFunc == null) {
                throw new ArgumentNullException(nameof(mapFunc));
            }
            return Match<OneOf<T0, TResult, T2>>(
                input0 => input0,
                input1 => mapFunc(input1),
                input2 => input2
            );
        }

        public OneOf<T0, T1, TResult> MapT2<TResult>(Func<T2, TResult> mapFunc) {
            if (mapFunc == null) {
                throw new ArgumentNullException(nameof(mapFunc));
            }
            return Match<OneOf<T0, T1, TResult>>(
                input0 => input0,
                input1 => input1,
                input2 => mapFunc(input2)
            );
        }

        /// <summary>
        /// Value could be null, obey the if statement.
        /// </summary>
        public bool TryPickT0(out T0 value, out OneOf<T1, T2> remainder) {
            value = IsT0 ? AsT0 : default!;
            remainder = IsT0
                ? default
                : Match<OneOf<T1, T2>>(t0 => throw new InvalidOperationException(), t1 => t1, t2 => t2);
            return IsT0;
        }

        /// <summary>
        /// Value could be null, obey the if statement.
        /// </summary>
        public bool TryPickT1(out T1 value, out OneOf<T0, T2> remainder) {
            value = IsT1 ? AsT1 : default!;
            remainder = IsT1
                ? default
                : Match<OneOf<T0, T2>>(t0 => t0, t1 => throw new InvalidOperationException(), t2 => t2);
            return IsT1;
        }

        /// <summary>
        /// Value could be null, obey the if statement.
        /// </summary>
        public bool TryPickT2(out T2 value, out OneOf<T0, T1> remainder) {
            value = IsT2 ? AsT2 : default!;
            remainder = IsT2
                ? default
                : Match<OneOf<T0, T1>>(t0 => t0, t1 => t1, t2 => throw new InvalidOperationException());
            return IsT2;
        }

        public bool Equals(OneOf<T0, T1, T2> other) {
            if (_index != other._index) {
                return false;
            }
            switch (_index) {
                case 0:
                    return Equals(_value0, other._value0);
                case 1:
                    return Equals(_value1, other._value1);
                case 2:
                    return Equals(_value2, other._value2);
                default:
                    return false;
            }
        }

        public override bool Equals(object obj) {
            if (obj is null) {
                return false;
            }

            return obj is OneOf<T0, T1, T2> && Equals((OneOf<T0, T1, T2>)obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode;
                switch (_index) {
                    case 0:
                        hashCode = _value0?.GetHashCode() ?? 0;
                        break;
                    case 1:
                        hashCode = _value1?.GetHashCode() ?? 0;
                        break;
                    case 2:
                        hashCode = _value2?.GetHashCode() ?? 0;
                        break;
                    default:
                        hashCode = 0;
                        break;
                }
                return (hashCode * 397) ^ _index;
            }
        }

        public static bool operator ==(OneOf<T0, T1, T2> left, OneOf<T0, T1, T2> right) {
            return left.Equals(right);
        }

        public static bool operator !=(OneOf<T0, T1, T2> left, OneOf<T0, T1, T2> right) {
            return !(left == right);
        }
    }
#pragma warning restore CA1065 // Do not raise exceptions in unexpected locations
#pragma warning restore CA1000 // Do not declare static members on generic types
#pragma warning restore CA1066 // Type {0} should implement IEquatable<T> because it overrides Equals
}
