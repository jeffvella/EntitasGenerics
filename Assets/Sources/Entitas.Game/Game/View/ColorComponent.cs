using UnityEngine;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ColorComponent : IComponent, IEventComponent
    {
        public Color value;
    }
}