using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

//[Input]
[Unique]
public sealed class PointerHoldingPositionComponent : IUniqueComponent
{
    public Vector3 value;
}