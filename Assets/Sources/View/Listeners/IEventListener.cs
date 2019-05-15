using Entitas;
using Entitas.Generics;

public interface IEventListener
{
    void RegisterListeners(Contexts contexts, IEntity entity);
}
