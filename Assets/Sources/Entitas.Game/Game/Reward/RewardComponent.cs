using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class RewardComponent : IValueComponent<int>
    {
        public int Value { get;set; }
    }
}