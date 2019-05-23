using Entitas;
using Entitas.MatchLine;
using Performance;
using Performance.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public sealed class TestViewService : Service, IViewService
{

    public TestViewService(Contexts contexts, MainViewModel viewModel, IFactories factories) : base(contexts, viewModel, factories)
    {
        LoadViews();
    }

    public void LoadViews()
    {
        foreach (var view in _viewModel.Views)
            view.InitializeView(_viewModel, _contexts, _factories);
    }

    public void LoadAsset(Contexts contexts, IEntity entity, string assetName, int assetType)
    {
        var element = CreateElement(assetName, assetType);

        var eventListeners = element.GetBehaviors<IEntityListener>();
        foreach (var listener in eventListeners)
            listener.RegisterListeners(_viewModel, element, contexts, _factories, entity);

        _viewModel.Board.AddElement(element);
    }

    public void LoadAsset<TEntity>(Contexts contexts, TEntity entity, string assetName, int assetType) where TEntity : IEntity
    {
        var element = CreateElement(assetName, assetType);        

        foreach (var listener in element.GetBehaviors<IEntityListener>())
            listener.RegisterListeners(_viewModel, element, contexts, _factories, entity);

        foreach (var listener in element.GetBehaviors<IEntityListener<TEntity>>())
            listener.RegisterListeners(_viewModel, element, contexts, _factories, entity);

        _viewModel.Board.AddElement(element);
    }

    public void LoadAssets<TEntity>(Contexts contexts, List<TEntity> entities, string assetName, int assetType) where TEntity : IEntity
    {
        var elements = new List<ElementViewModel>();

        foreach(var entity in entities)
        {
            var element = CreateElement(assetName, assetType);

            foreach (var listener in element.GetBehaviors<IEntityListener>())
                listener.RegisterListeners(_viewModel, element, contexts, _factories, entity);

            foreach (var listener in element.GetBehaviors<IEntityListener<TEntity>>())
                listener.RegisterListeners(_viewModel, element, contexts, _factories, entity);
        }

        _viewModel.Board.AddElements(elements);
    }

    private ElementViewModel CreateElement(string assetName, int assetType)
    {
        var element = _factories.ElementFactory.Create(assetName, assetType);
        return element;
    }
}