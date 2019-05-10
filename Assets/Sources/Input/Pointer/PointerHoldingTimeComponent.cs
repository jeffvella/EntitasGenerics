using Entitas;
using Entitas.CodeGeneration.Attributes;

//[Input]
[Unique]
public sealed class PointerHoldingTimeComponent : IUniqueComponent
{
    public float value;
}