using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ScoreComponent : IUniqueComponent, IEventComponent

    {
        public int Value;
    }
}