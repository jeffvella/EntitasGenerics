using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;

//[Input]
[Unique]
public sealed class DeltaTimeComponent : IUniqueComponent
{
    public float value;
}