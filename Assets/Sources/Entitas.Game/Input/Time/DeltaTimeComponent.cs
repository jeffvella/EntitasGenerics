using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class DeltaTimeComponent : IValueComponent<float>, IUniqueComponent
    {
        public float Value { get; set; }
    }
}