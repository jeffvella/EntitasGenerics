using UnityEngine;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ColorComponent : IValueComponent<Color>, IEventComponent
    {
        public Color Value { get; set; }
    }
}