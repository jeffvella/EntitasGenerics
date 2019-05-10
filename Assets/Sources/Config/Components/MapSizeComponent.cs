using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEditor.UIElements;
using UnityEngine;

//[Config]
[Unique]
public sealed class MapSizeComponent : IValueComponent<GridSize>, IUniqueComponent
{
    public GridSize value { get; set; }
}

public interface IValueComponent<T>
{
    T value { get; set; }
}

