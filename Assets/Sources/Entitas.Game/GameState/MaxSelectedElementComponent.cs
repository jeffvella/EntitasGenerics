using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class MaxSelectedElementComponent : IValueComponent<int>, IUniqueComponent
    {
        public int Value { get; set; }
    }
}