using System;
using System.Collections.Generic;

namespace Zenworks.Utils {

    public interface IMapper {
        TTo Map<TFrom, TTo>(TFrom from);
    }
    public interface IMappingsProvider<TContext> {
        MapFuncSet<TContext> GetMappings();
    }

    public class MapFuncSet {
        public delegate TTo MapFunc<TFrom, TTo>(TFrom from);
        public delegate TTo MapFuncWithMapper<TFrom, TTo>(TFrom from, Mapper mapper);

        private readonly Dictionary<Type, Dictionary<Type, object>> mappersFromTo =
            new Dictionary<Type, Dictionary<Type, object>>();
        public void AddMapping<TFrom, TTo>(MapFunc<TFrom, TTo> func) {
            AddMappingOneOf<TFrom, TTo>(func);
        }
        public void AddMapping<TFrom, TTo>(MapFuncWithMapper<TFrom, TTo> func) {
            AddMappingOneOf<TFrom, TTo>(func);
        }
        private void AddMappingOneOf<TFrom, TTo>(OneOf<MapFunc<TFrom, TTo>,
                                                MapFuncWithMapper<TFrom, TTo>> func) {
            Type to = typeof(TTo);
            Type from = typeof(TFrom);
            if (!mappersFromTo.ContainsKey(from)) {
                mappersFromTo[from] = new Dictionary<Type, object>();
            }
            Dictionary<Type, object> mappersTo = mappersFromTo[from];
            mappersTo[to] = func;
        }

        public OneOf<
            MapFunc<TFrom, TTo>,
            MapFuncWithMapper<TFrom, TTo>> GetMapping<TFrom, TTo>() {
            if (!mappersFromTo.TryGetValue(typeof(TFrom), out Dictionary<Type, object>? toMappings)) {
                throw new ArgumentOutOfRangeException($"No mapping from {typeof(TFrom)} to anything.");
            }
            if (!toMappings.TryGetValue(typeof(TTo), out object? mapperFunc)) {
                throw new ArgumentOutOfRangeException($"No mapping from {typeof(TFrom)} to {typeof(TTo)}.");
            }
            if (mapperFunc is OneOf<MapFunc<TFrom, TTo>,
                                    MapFuncWithMapper<TFrom, TTo>> func) {
                return func;
            }
            throw new ArgumentOutOfRangeException($"Unexpected mapping function type: {mapperFunc}.");
        }
    }

    public class MapFuncSet<TContext> {
        public delegate TTo MapFunc<TFrom, TTo>(TFrom from);
        public delegate TTo MapFuncWithContext<TFrom, TTo>(TFrom from, TContext context);
        public delegate TTo MapFuncWithMapper<TFrom, TTo>(TFrom from, TContext context, Mapper<TContext> mapper);

        private readonly Dictionary<Type, Dictionary<Type, object>> mappersFromTo =
            new Dictionary<Type, Dictionary<Type, object>>();
        public void AddMapping<TFrom, TTo>(MapFunc<TFrom, TTo> func) {
            AddMappingOneOf<TFrom, TTo>(func);
        }
        public void AddMapping<TFrom, TTo>(MapFuncWithContext<TFrom, TTo> func) {
            AddMappingOneOf<TFrom, TTo>(func);
        }
        public void AddMapping<TFrom, TTo>(MapFuncWithMapper<TFrom, TTo> func) {
            AddMappingOneOf<TFrom, TTo>(func);
        }
        private void AddMappingOneOf<TFrom, TTo>(OneOf<MapFunc<TFrom, TTo>,
                                                MapFuncWithContext<TFrom, TTo>,
                                                MapFuncWithMapper<TFrom, TTo>> func) {
            Type to = typeof(TTo);
            Type from = typeof(TFrom);
            if (!mappersFromTo.ContainsKey(from)) {
                mappersFromTo[from] = new Dictionary<Type, object>();
            }
            Dictionary<Type, object> mappersTo = mappersFromTo[from];
            mappersTo[to] = func;
        }

        public OneOf<
            MapFunc<TFrom, TTo>,
            MapFuncWithContext<TFrom, TTo>,
            MapFuncWithMapper<TFrom, TTo>> GetMapping<TFrom, TTo>() {
            if (!mappersFromTo.TryGetValue(typeof(TFrom), out Dictionary<Type, object>? toMappings)) {
                throw new ArgumentOutOfRangeException($"No mapping from {typeof(TFrom)} to anything.");
            }
            if (!toMappings.TryGetValue(typeof(TTo), out object? mapperFunc)) {
                throw new ArgumentOutOfRangeException($"No mapping from {typeof(TFrom)} to {typeof(TTo)}.");
            }
            if (mapperFunc is OneOf<MapFunc<TFrom, TTo>,
                                    MapFuncWithContext<TFrom, TTo>,
                                    MapFuncWithMapper<TFrom, TTo>> func) {
                return func;
            }
            throw new ArgumentOutOfRangeException($"Unexpected mapping function type: {mapperFunc}.");
        }

    }

    public class Mapper<TContext> : IMapper {
        private readonly TContext context;
        private readonly MapFuncSet<TContext> mapFuncSet;

        public Mapper(MapFuncSet<TContext> mapFuncSet, TContext context) {
            this.context = context;
            this.mapFuncSet = mapFuncSet;
        }

        public TTo Map<TFrom, TTo>(TFrom from) {
            OneOf<MapFuncSet<TContext>.MapFunc<TFrom, TTo>,
                MapFuncSet<TContext>.MapFuncWithContext<TFrom, TTo>,
                MapFuncSet<TContext>.MapFuncWithMapper<TFrom, TTo>> mapperFunc = mapFuncSet.GetMapping<TFrom, TTo>();
            return mapperFunc.Match(
                f => f(from),
                f => f(from, context),
                f => f(from, context, this));
        }
    }

    public class Mapper : IMapper {
        private readonly MapFuncSet mapFuncSet;

        public Mapper(MapFuncSet mapFuncSet) {
            this.mapFuncSet = mapFuncSet;
        }

        public TTo Map<TFrom, TTo>(TFrom from) {
            OneOf<MapFuncSet.MapFunc<TFrom, TTo>,
                MapFuncSet.MapFuncWithMapper<TFrom, TTo>> mapperFunc = mapFuncSet.GetMapping<TFrom, TTo>();
            return mapperFunc.Match(
                f => f(from),
                f => f(from, this));
        }
    }
}
