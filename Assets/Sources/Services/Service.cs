using Entitas.Generics;

public abstract class Service
{
    protected readonly GenericContexts _contexts;

    public Service(GenericContexts contexts)
    {
        _contexts = contexts;
    }

    protected virtual void DropState()
    {
    }
}