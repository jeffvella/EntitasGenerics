using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class MapSizeComponent : IUniqueComponent, IEventComponent
    {
        public GridSize Value { get; set; }
    }

}