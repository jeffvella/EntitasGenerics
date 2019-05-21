using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class MapSizeComponent : IUniqueComponent
    {
        public GridSize value { get; set; }
    }

}