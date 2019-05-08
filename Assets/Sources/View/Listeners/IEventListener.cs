using Entitas;
using Entitas.Generics;

public interface IEventListener
{
    void RegisterListeners(GenericContexts contexts, IEntity entity);
}
