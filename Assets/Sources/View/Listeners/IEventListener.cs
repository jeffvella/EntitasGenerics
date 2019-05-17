using Entitas;
using Entitas.Generics;

public interface IEventListener
{
    void RegisterListeners(Contexts contexts, IEntity entity);
}

public interface IEventListener<in TEntity>
{
    void RegisterListeners(Contexts contexts, TEntity entity);
}

