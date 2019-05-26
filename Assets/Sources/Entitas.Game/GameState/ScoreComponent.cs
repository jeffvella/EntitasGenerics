using Entitas.Generics;
using System;
using System.Collections.Generic;

namespace Entitas.MatchLine
{
    public sealed class ScoreComponent : IUniqueComponent, IEventComponent, IIndexedComponent<ScoreComponent>
    {
        public int Value;

        //public IEqualityComparer<ScoreComponent> IndexComparer { get; } = new InlineEqualityComparer<ScoreComponent>((x, y) => x.Value == y.Value);

        public bool Equals(ScoreComponent x, ScoreComponent y) => x.Value == y.Value;

        public int GetHashCode(ScoreComponent obj) => obj.GetHashCode();
    }
}

public static class Comparers
{
   
}

public class InlineEqualityComparer<T> : IEqualityComparer<T>
{
    public Func<T, T, bool> Comparer { get; set; }

    public InlineEqualityComparer(Func<T, T, bool> comparer)
    {
        Comparer = comparer;
    }

    public bool Equals(T x, T y)
    {
        return Comparer(x, y);
    }

    public int GetHashCode(T value)
    {
        return value.GetHashCode();
    }
}

