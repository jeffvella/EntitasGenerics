using Entitas;
using Entitas.Generics;

public class GenericEntity<T> : Entitas.Entity, IContextLinkedEntity<T> where T : class, IEntity, new()
{
    public IGenericContext<T> Context { get; }
}

public interface IContextLinkedEntity<T> where T : class, IEntity, new()
{
    IGenericContext<T> Context { get; }
}
