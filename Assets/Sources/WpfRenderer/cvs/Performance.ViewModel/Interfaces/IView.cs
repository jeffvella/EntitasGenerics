using Entitas;
using Entitas.MatchLine;
using Performance.Controls;
using Performance.ViewModels;

public interface IViewBehavior
{

}

public interface IEntityListener : IViewBehavior
{
    void RegisterListeners(MainViewModel model, ElementViewModel element, Contexts contexts, IFactories factories, IEntity entity);
}

public interface IEntityListener<in TEntity> : IViewBehavior
{
    void RegisterListeners(MainViewModel model, ElementViewModel element, Contexts contexts, IFactories factories, TEntity entity);
}

public interface IView
{
    void InitializeView(MainViewModel model, Contexts contexts, IFactories factories);
}
