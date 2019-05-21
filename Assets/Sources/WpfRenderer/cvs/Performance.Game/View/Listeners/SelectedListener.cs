using Entitas;
using Entitas.Generics;
using Entitas.MatchLine;
using Performance.Controls;
using Performance.ViewModels;
using UnityEngine;

public class SelectedListener : IEventListener
{
    private BoardViewModel _board;
    private ElementViewModel _element;

    public void RegisterListeners(MainViewModel model, ElementViewModel element, Contexts contexts, IEntity entity)
    {
        _board = model.Board;
        _element = element;

        if (entity is GameEntity gameEntity)
        {
            gameEntity.RegisterAddedComponentListener<SelectedComponent>(OnSelected);
            gameEntity.RegisterRemovedComponentListener<SelectedComponent>(OnDeselected);
        }
    }

    private void OnSelected((IEntity Entity, SelectedComponent Component) obj)
    {                
        _element.IsSelected = true;
    }

    private void OnDeselected(IEntity entity)
    {
        _element.IsSelected = false;
    }

}