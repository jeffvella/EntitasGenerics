using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Config]
[Unique]
public sealed class ExplosiveScoringTableComponent : IComponent
{
    public List<int> value;
}