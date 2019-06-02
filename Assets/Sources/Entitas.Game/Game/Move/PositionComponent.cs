using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class PositionComponent : IValueComponent<GridPosition>, IEqualityComparer<PositionComponent>, IEventComponent
    {
        public GridPosition Value { get; set; }

        public bool Equals(PositionComponent x, PositionComponent y) => x != null && y != null && x.Value.Equals(y.Value);

        public int GetHashCode(PositionComponent obj) => obj.Value.GetHashCode();
    }
}