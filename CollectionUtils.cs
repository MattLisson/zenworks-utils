using System;
using System.Collections.Generic;
using System.Linq;

namespace Zenworks.Utils {
    public static class CollectionUtils {

        public static IEnumerable<T> OfType<T>(this System.Collections.IEnumerable from) {
            if (from == null) {
                yield break;
            }
            foreach(object obj in from) {
                if (obj is T t) {
                    yield return t;
                }
            }
        }
        public static bool DictEquals<TKey, TValue>(IDictionary<TKey, TValue> a, IDictionary<TKey, TValue> b) {
            return a.Count == b.Count
                && a.Keys.All(key => b.ContainsKey(key)
                    && EqualityComparer<TValue>.Default.Equals(a[key], b[key]));
        }

        public static bool SequenceEqual<T>(this IEnumerable<T>? a, IEnumerable<T>? b) {
            if (a == null) {
                return b == null;
            }
            if (b == null) {
                return false;
            }
            var enumeratorA = a.GetEnumerator();
            var enumeratorB = b.GetEnumerator();
            while(enumeratorA.MoveNext()) {
                if (!enumeratorB.MoveNext()) {
                    return false;
                }
                if (enumeratorA.Current == null) {
                    if (enumeratorB.Current == null) {
                        continue;
                    } else {
                        return false;
                    }
                }
                if (!enumeratorA.Current.Equals(enumeratorB.Current)) {
                    return false;
                }
            }
            if (enumeratorB.MoveNext()) {
                return false;
            }
            return true;
        }

        public static void EditToMatch<T>(this IEnumerable<T>? a, IEnumerable<T>? b, Action<T> add, Action<T> remove)
        {
            if (a == null || b == null)
                return;

            HashSet<T> toAdd = new HashSet<T>(b);
            HashSet<T> toRemove = new HashSet<T>(a);
            toAdd.ExceptWith(a);
            toRemove.ExceptWith(b);
            foreach(T t in toAdd) {
                add(t);
            }
            foreach(T t in toRemove) {
                remove(t);
            }
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> first, params T[] second) {
            if (first != null) {
                foreach (T t in first) {
                    yield return t;
                }
            }
            if (second == null) {
                yield break;
            }
            foreach (T t in second) {
                yield return t;
            }
        }
        public static IEnumerable<T> Append<T>(this IEnumerable<T> first, IEnumerable<T> second) {
            if (first != null) {
                foreach (T t in first) {
                    yield return t;
                }
            }
            if (second == null) {
                yield break;
            }
            foreach (T t in second) {
                yield return t;
            }
        }

        public static T? FirstOrNull<T>(this IEnumerable<T> from) where T : class {
            foreach (T t in from) {
                return t;
            }
            return default;
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> from, T @default) where T : struct {
            foreach (T t in from) {
                return t;
            }
            return @default;
        }

        public static T? NthOrNull<T>(this IEnumerable<T> from, int n) where T : class {
            if (n >= 0) {
                foreach (T t in from) {
                    if (n == 0)
                        return t;
                    n--;
                }
            } else {
                List<T> list = from.ToList();
                if (list.Count < -n) {
                    return null;
                }
                return list[list.Count + n];
            }
            return null;
        }

        public static T FindOrDefault<T>(this IEnumerable<T> from, Func<T, bool> predicate, T @default = default) where T : struct {
            foreach (T t in from) {
                if (predicate(t)) {
                    return t;
                }
            }
            return @default;
        }

        public static T MaxOrDefault<T>(this IEnumerable<T> from) where T : IComparable {
            bool any = false;
            T max = default!;
            foreach (T t in from) {
                if (!any) {
                    max = t;
                    any = true;
                    continue;
                }
                if (max.CompareTo(t) < 0) {
                    max = t;
                }
            }
            if (!any) {
                return default!;
            }
            return max;
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> from, Func<T, bool> predicate) {
            if (from == null) {
                yield break;
            }
            foreach (T u in from) {
                if (predicate(u)) {
                    yield return u;
                }
            }
        }

        public static IEnumerable<T> SkipNulls<T>(this IEnumerable<T?> from) where T : class {
            if (from == null) {
                yield break;
            }
            foreach (T? u in from) {
                if (u != null) {
                    yield return u;
                }
            }
        }

        public static IEnumerable<TTo> Map<TFrom, TTo>(this IEnumerable<TFrom>? from, Func<TFrom, TTo> map) {
            if (from == null) {
                yield break;
            }
            foreach (TFrom u in from) {
                yield return map(u);
            }
        }

        public static IEnumerable<T> Skip<T>(this IEnumerable<T> from, uint count) {
            foreach (T t in from) {
                if (count != 0) {
                    count--;
                    continue;
                }
                yield return t;
            }
        }

        public static List<T> OrderWith<T>(this List<T> from, Comparison<T> comparison) {
            from.Sort(comparison);
            return from;
        }

        public static TVal Collect<T, TVal>(this IEnumerable<T> from, Func<TVal, T, TVal> collector) where TVal : struct {
            TVal u = default;
            foreach(T t in from) {
                u = collector(u, t);
            }
            return u;
        }

        public static List<T> OrderWith<T>(this IEnumerable<T> from, Comparison<T> comparison) {
            if (from is List<T> list) {
                return OrderWith(list, comparison);
            }
            return OrderWith(from.ToList(), comparison);
        }

        public static List<T> ToSortedList<T>(this List<T> from) where T : IComparable<T> {
            from.Sort();
            return from;
        }
        public static List<T> ToSortedList<T>(this IEnumerable<T> from) where T : IComparable<T> {
            if (from is List<T> list) {
                return ToSortedList(list);
            }
            return ToSortedList(from.ToList());
        }

        public static Dictionary<TKey, TValue> ToDict<TKey, TValue>(this IEnumerable<TValue> from,
            Func<TValue, TKey> keyFunc) where TKey : notnull {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            foreach (TValue value in from) {
                result[keyFunc(value)] = value;
            }
            return result;
        }

        public static Dictionary<TKey, TValue2> MapValues<TKey, TValue, TValue2>(this IEnumerable<KeyValuePair<TKey, TValue>> from,
            Func<TValue, TValue2> valueFunc) where TKey : notnull {
            Dictionary<TKey, TValue2> result = new Dictionary<TKey, TValue2>();
            foreach (KeyValuePair<TKey, TValue> keyValue in from) {
                result[keyValue.Key] = valueFunc(keyValue.Value);
            }
            return result;
        }
    }
}
