using Entitas;
using Entitas.CodeGeneration.Attributes;

//[Config]
[Unique]
public sealed class ComboDefinitionsComponent : IUniqueComponent
{
    public ComboDefinitions value;
}