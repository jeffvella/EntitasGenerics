using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;

//[GameState]
[Unique]
public sealed class MaxSelectedElementComponent : IUniqueComponent
{
    public int value;
}