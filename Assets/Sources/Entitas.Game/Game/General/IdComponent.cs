using Entitas.Generics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Entitas.MatchLine
{
    public sealed class IdComponent : IValueComponent<int>, IEqualityComparer<IdComponent>
    {
        public int Value { get; set; }

        public bool Equals(IdComponent x, IdComponent y) => x.Value.Equals(y.Value);

        public int GetHashCode(IdComponent obj) => obj.Value.GetHashCode();
    }

}