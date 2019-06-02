using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class RealtimeSinceStartupComponent : IValueComponent<float>, IUniqueComponent
    {
        public float Value { get; set; }
    }
}