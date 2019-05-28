using Entitas.Generics;
using UnityEngine;

namespace Entitas.MatchLine
{
    public sealed class PointerHoldingPositionComponent : IValueComponent<GridPosition>, IUniqueComponent
    {
        public GridPosition Value { get; set; }
    }
}