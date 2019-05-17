using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;

public sealed class ScoringTableComponent : IUniqueComponent
{
    public List<int> value;
}