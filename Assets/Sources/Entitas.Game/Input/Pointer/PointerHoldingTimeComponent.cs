using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class PointerHoldingTimeComponent : IValueComponent<float>, IUniqueComponent
    {
        public float Value { get; set; }
    }
}