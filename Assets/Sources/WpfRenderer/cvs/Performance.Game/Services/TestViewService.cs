using Entitas;
using Entitas.MatchLine;
using Performance.ViewModels;
using static TestElementService;

public sealed class TestViewService : Service, IViewService
{
    public TestViewService(Contexts contexts, MainViewModel viewModel) : base(contexts, viewModel)
    {
        LoadViews();
    }

    public void LoadViews()
    {
        foreach (var view in _viewModel.Views)
            view.InitializeView(_viewModel, _contexts);
    }

    public void LoadAsset(Contexts contexts, IEntity entity, string assetName, int assetType)
    {
        var element = CreateElement(assetName, assetType);

        var eventListeners = element.GetBehaviors<IEventListener>();
        foreach (var listener in eventListeners)
            listener.RegisterListeners(_viewModel, element, contexts, entity);
    }

    public void LoadAsset<TEntity>(Contexts contexts, TEntity entity, string assetName, int assetType) where TEntity : IEntity
    {
        var element = CreateElement(assetName, assetType);        

        foreach (var listener in element.GetBehaviors<IEventListener>())
            listener.RegisterListeners(_viewModel, element, contexts, entity);

        foreach (var listener in element.GetBehaviors<IEventListener<TEntity>>())
            listener.RegisterListeners(_viewModel, element, contexts, entity);
    }

    private ElementViewModel CreateElement(string assetName, int assetType)
    {
        var element = new ElementViewModel();
        element.AddBehavior<DestroyedListener>();
        element.AddBehavior<ColorListener>();
        element.AddBehavior<PositionListener>();
        element.AddBehavior<SelectedListener>();
        element.AssetName = assetName;
        element.ActorType = (ActorType)assetType;

        _viewModel.Board.AddElement(element);

        return element;
    }
}