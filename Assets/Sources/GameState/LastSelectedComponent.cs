using Entitas;
using Entitas.CodeGeneration.Attributes;

//[GameState]
[Unique]
public sealed class LastSelectedComponent : IUniqueComponent
{
    public int value;
}