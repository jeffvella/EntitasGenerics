using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;

//[Input]
[Unique]
public sealed class PointerHoldingTimeComponent : IUniqueComponent
{
    public float value;
}