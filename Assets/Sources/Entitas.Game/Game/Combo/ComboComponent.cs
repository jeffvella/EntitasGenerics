using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ComboComponent : IValueComponent<int>, IEventComponent, IUniqueComponent
    {
        public int Value { get; set; }
    }
}