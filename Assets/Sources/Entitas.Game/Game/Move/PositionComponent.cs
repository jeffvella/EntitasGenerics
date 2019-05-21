using System;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class PositionComponent : IComponent, IEquatable<GridPosition>, IEventComponent
    {
        public GridPosition value;

        public bool Equals(GridPosition other) => other.Equals(value);
    }
}