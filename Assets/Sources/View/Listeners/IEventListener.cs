using Entitas;

public interface IEventListener
{
    void RegisterListeners(Contexts contexts, IEntity entity);
}
