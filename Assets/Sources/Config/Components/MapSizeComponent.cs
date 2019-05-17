using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;
using UnityEditor.UIElements;
using UnityEngine;

[Unique]
public sealed class MapSizeComponent : IUniqueComponent
{
    public GridSize value { get; set; }
}

