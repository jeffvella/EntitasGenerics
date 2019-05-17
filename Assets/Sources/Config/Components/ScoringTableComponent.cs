using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;

//[Config]
[Unique]
public sealed class ScoringTableComponent : IUniqueComponent
{
    public List<int> value;
}