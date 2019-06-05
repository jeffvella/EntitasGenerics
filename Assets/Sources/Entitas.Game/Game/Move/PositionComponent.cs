using System;
using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    //public sealed class PositionComponent : IValueComponent<GridPosition>, IEqualityComparer<PositionComponent>, IEventComponent
    //{
    //    public GridPosition Value { get; set; }

    //    public bool Equals(PositionComponent x, PositionComponent y) => x != null && y != null && x.Value.Equals(y.Value);

    //    public int GetHashCode(PositionComponent obj) => obj.Value.GetHashCode();
    //}

    public struct PositionComponent : IValueComponent<GridPosition>, IEquatable<PositionComponent>, IComparable<PositionComponent>, IEventComponent //, IEqualityComparer<PositionComponent>, IEventComponent
    {
        public GridPosition Value { get; set; }

        public int CompareTo(PositionComponent other)
        {
            return other.Value.Equals(Value) ? 0 : -1;
        }

        //public bool Equals(PositionComponent x, PositionComponent y) => x.Value.Equals(y.Value);

        public bool Equals(PositionComponent other)
        {
            return other.Value.Equals(Value);
        }

        public override int GetHashCode() => Value.GetHashCode();


        //public int GetHashCode(PositionComponent obj) => obj.Value.GetHashCode();

        //public bool Equals(PositionComponent other)
        //{
        //    return other.Value.Equals(Value) && other.Value.Equals(Value);
        //}

        //public override bool Equals(object other)
        //{
        //    if (other is PositionComponent position)
        //    {
        //        return position.Value.Equals(Value);
        //    }
        //    return false;
        //}
    }
}