using Entitas;
using Entitas.MatchLine;
using Performance.Controls;
using Performance.ViewModels;

public interface IEventListener : IViewBehavior
{
    void RegisterListeners(MainViewModel model, ElementViewModel element, Contexts contexts, IEntity entity);
}

public interface IEventListener<in TEntity> : IViewBehavior
{
    void RegisterListeners(MainViewModel model, ElementViewModel element, Contexts contexts, TEntity entity);
}

