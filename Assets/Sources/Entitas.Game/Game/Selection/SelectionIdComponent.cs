using System;
using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class SelectionIdComponent : IValueComponent<int>, IEqualityComparer<SelectionIdComponent>
    {
        public int Value { get; set; }

        public bool Equals(SelectionIdComponent x, SelectionIdComponent y) 
            => x != null && y != null && x.Value.Equals(y.Value);

        public int GetHashCode(SelectionIdComponent obj) 
            => obj.Value.GetHashCode();
    }
}