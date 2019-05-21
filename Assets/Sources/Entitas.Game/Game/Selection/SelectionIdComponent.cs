using System;

namespace Entitas.MatchLine
{
    public sealed class SelectionIdComponent : IComponent, IEquatable<int>
    {
        public int value;

        public bool Equals(int other)
        {
            return value == other;
        }
    }
}