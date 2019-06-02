using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ActionCountComponent : IValueComponent<int>, IUniqueComponent, IEventComponent
    {
        public int Value { get; set; }
    }

}