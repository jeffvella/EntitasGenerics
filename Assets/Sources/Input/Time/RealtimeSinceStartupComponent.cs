using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;

//[Input]
[Unique]
public sealed class RealtimeSinceStartupComponent : IUniqueComponent
{
    public float value;
}