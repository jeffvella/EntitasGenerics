using Entitas;
using Entitas.CodeGeneration.Attributes;

//[Input]
[Unique]
public sealed class RealtimeSinceStartupComponent : IUniqueComponent
{
    public float value;
}