using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;
using UnityEngine;

//[Input]
[Unique]
public sealed class PointerHoldingPositionComponent : IUniqueComponent
{
    public Vector3 value;
}