using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEditor.UIElements;
using UnityEngine;

[Config]
[Unique]
public sealed class MapSizeComponent : IComponent
{
    public GridSize value;
}
