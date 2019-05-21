using Entitas.MatchLine;
using Performance.ViewModels;

public abstract class Service  : IService
{
    protected readonly Contexts _contexts;
    protected readonly MainViewModel _viewModel;

    public Service(Contexts contexts, MainViewModel viewModel)
    {
        _contexts = contexts;
        _viewModel = viewModel;
    }

    public virtual void DropState()
    {
    }
}