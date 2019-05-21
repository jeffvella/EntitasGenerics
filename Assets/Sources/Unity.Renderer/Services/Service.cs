using Entitas.MatchLine;

public abstract class Service  : IService
{
    protected readonly Contexts _contexts;

    public Service(Contexts contexts)
    {
        _contexts = contexts;
    }

    public virtual void DropState()
    {
    }
}