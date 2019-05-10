using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

//[Config]
[Unique]
public sealed class ExplosiveScoringTableComponent : IUniqueComponent
{
    public List<int> value;
}