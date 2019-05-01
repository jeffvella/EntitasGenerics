using Entitas.Generics;

public abstract class Service
{
    protected readonly Contexts _contexts;
    protected GenericContexts _genericContexts;

    public Service(Contexts contexts, GenericContexts genericContexts)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
    }

    protected virtual void DropState()
    {
    }
}