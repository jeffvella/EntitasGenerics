using Entitas;
using Entitas.CodeGeneration.Attributes;

//[Input]
[Unique]
public sealed class DeltaTimeComponent : IUniqueComponent
{
    public float value;
}