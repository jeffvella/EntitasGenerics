using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;

//[GameState]
[Unique]
public sealed class LastSelectedComponent : IUniqueComponent
{
    public int value;
}