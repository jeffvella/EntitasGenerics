using Entitas.Generics;
using System;
using System.Diagnostics;

namespace Entitas.MatchLine
{
    //public sealed class IdComponent : IComponent, IEquatable<int>
    //{
    //    public int value;

    //    public bool Equals(int other) => value.Equals(other);
    //}

    [DebuggerDisplay("{DebugDisplay}")]
    public sealed class IdComponent : IValueComponent<int>, ISearchableComponent<IdComponent>, IEquatable<int>
    {
        //public int value;
        //public bool Equals(int other) => value.Equals(other);

        public bool Equals(int other) => Value.Equals(other);

        public bool Equals(IdComponent x, IdComponent y)
        {
            return x.Value.Equals(y.Value);
        }

        public int GetHashCode(IdComponent obj)
        {
            return obj.Value.GetHashCode();
        }

        public string GetDebugDisplay => $"{Value}, Hash={GetHashCode(this)}";

        public int Value { get; set; }
    }

}