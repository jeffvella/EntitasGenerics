using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ScoringTableComponent : IValueComponent<List<int>>, IUniqueComponent
    {
        public List<int> Value { get; set; }
    }
}