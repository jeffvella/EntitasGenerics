using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ScoringTableComponent : IUniqueComponent
    {
        public List<int> value;
    }
}