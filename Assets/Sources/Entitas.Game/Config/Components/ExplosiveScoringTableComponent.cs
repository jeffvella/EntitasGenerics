using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ExplosiveScoringTableComponent : IValueComponent<List<int>>, IUniqueComponent
    {
        public List<int> Value { get; set; }
    }
}