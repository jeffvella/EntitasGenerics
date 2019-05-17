using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;

//[GameState]
[Unique]
[Event(EventTarget.Any)]
public sealed class ActionCountComponent : IUniqueComponent
{
    public int value;
}


