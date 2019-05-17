using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;

[Unique]
public sealed class ComboDefinitionsComponent : IUniqueComponent
{
    public ComboDefinitions value;
}