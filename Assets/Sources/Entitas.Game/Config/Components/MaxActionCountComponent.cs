using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class MaxActionCountComponent : IUniqueComponent, IEventComponent
    {
        public int Value;
    }
}