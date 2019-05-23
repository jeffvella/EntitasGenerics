using Entitas.MatchLine;
using Performance.ViewModels;

public abstract class Service  : IService
{
    protected readonly Contexts _contexts;
    protected readonly IFactories _factories;
    protected readonly MainViewModel _viewModel;

    public Service(Contexts contexts, MainViewModel viewModel, IFactories factories)
    {
        _contexts = contexts;
        _factories = factories;
        _viewModel = viewModel;
    }

    public virtual void DropState()
    {
    }
}
