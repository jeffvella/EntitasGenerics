using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class AssetComponent : IValueComponent<string>
    {
        public string Value { get; set; }

        public int id;
    }
}