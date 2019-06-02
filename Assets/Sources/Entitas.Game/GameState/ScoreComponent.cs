using Entitas.Generics;
using System;
using System.Collections.Generic;

namespace Entitas.MatchLine
{
    public sealed class ScoreComponent : IValueComponent<int>, IUniqueComponent, IEventComponent, IEqualityComparer<ScoreComponent>
    {
        public int Value { get; set; }

        public bool Equals(ScoreComponent x, ScoreComponent y) => x.Value == y.Value;

        public int GetHashCode(ScoreComponent obj) => obj.GetHashCode();
    }
}
