using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ComboDefinitionsComponent : IValueComponent<ComboDefinitions>, IUniqueComponent
    {
        public ComboDefinitions Value { get; set; }
    }
}