using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ElementTypeComponent : IValueComponent<int>
    {
        public int Value { get; set; }
    }
}