using Entitas;
using Entitas.CodeGeneration.Attributes;

//[GameState]
[Unique]
public sealed class MaxSelectedElementComponent : IUniqueComponent
{
    public int value;
}