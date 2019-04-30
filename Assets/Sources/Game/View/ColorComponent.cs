using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game]
[Event(EventTarget.Any)]
public sealed class ColorComponent : IComponent
{
    public Color value;
}