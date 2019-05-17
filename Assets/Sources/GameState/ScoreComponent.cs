using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;

[Event(EventTarget.Any)]
public sealed class ScoreComponent : IUniqueComponent 

{
    public int Value;
}