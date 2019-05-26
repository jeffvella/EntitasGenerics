using System;
using System.Collections.Generic;
using System.Diagnostics;
using Entitas.Generics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Entitas.MatchLine
{
    [DebuggerDisplay("{value} {HashCode}")]
    public sealed class PositionComponent : IEventComponent, IIndexedComponent<PositionComponent>, IEquatable<GridPosition> //, IEqualityComparer<PositionComponent> //IEquatable<PositionComponent>, IEquatable<GridPosition>, 
    {
        public GridPosition value;

        public bool Equals(PositionComponent x, PositionComponent y) => x.value.Equals(y.value);

        public int GetHashCode(PositionComponent obj) => obj.value.GetHashCode();

        public bool Equals(GridPosition other) => other.Equals(value);

        public override string ToString()
        {
            return $"{value}";
        }

        //public bool Equals(PositionComponent x, PositionComponent y) //=> x.value.Equals(y.value);
        //{
        //    var equals = x?.value.Equals(y?.value) ?? false;
        //    Debug.Log($"{x.value}/{x.HashCode} Equals({y.value}/{y.HashCode})={equals}");
        //    return equals;
        //}

        //public int GetHashCode(PositionComponent obj) => obj.value.GetHashCode();

        //public override int GetHashCode() => value.GetHashCode();

        //public bool Equals(PositionComponent other)
        //{
        //    var equals = other != null && Equals(other.value);
        //    Debug.Log($"{value}/{HashCode} Equals({other.value}/{other.HashCode})={equals}");
        //    return equals;
        //}

        //public int HashCode => GetHashCode(this);
    }
}