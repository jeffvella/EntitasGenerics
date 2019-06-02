using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class LastSelectedComponent : IValueComponent<int>, IUniqueComponent
    {
        public int Value { get; set; }
    }
}