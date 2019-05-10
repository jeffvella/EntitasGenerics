using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

//[Game]
[Event(EventTarget.Any)]
public sealed class PositionComponent : IComponent, IEquatable<GridPosition>
{
    [PrimaryEntityIndex] public GridPosition value;

    public bool Equals(GridPosition other) => other.Equals(value);
}