using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

//[Config]
[Unique]
public sealed class ScoringTableComponent : IComponent, IUniqueComponent
{
    public List<int> value;
}