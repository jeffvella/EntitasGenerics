using System;

namespace Entitas.MatchLine
{
    public sealed class IdComponent : IComponent, IEquatable<int>
    {
        public int value;

        public bool Equals(int other) => value.Equals(other);
    }

}