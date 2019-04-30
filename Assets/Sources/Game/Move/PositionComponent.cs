using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game]
[Event(EventTarget.Any)]
public sealed class PositionComponent : IComponent
{
    [PrimaryEntityIndex] public GridPosition value;
}