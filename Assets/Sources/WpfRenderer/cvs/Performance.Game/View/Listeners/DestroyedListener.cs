using UnityEngine;
using Performance.Common;
using Entitas.MatchLine;
using Performance.ViewModels;
using Performance.Controls;

public class DestroyedListener : IEntityListener<GameEntity>
{
    private BoardViewModel _board;
    private ElementViewModel _element;

    public DestroyedListener()
    {
    }

    public void InitializeView(MainViewModel model, ElementViewModel element, Contexts contexts, GameEntity entity)
    {
        throw new System.NotImplementedException();
    }

    public void RegisterListeners(MainViewModel model, ElementViewModel element, Contexts contexts, GameEntity entity)
    {
        _board = model.Board;
        _element = element;

        contexts.Game.RegisterAddedComponentListener<DestroyedComponent>(entity, OnEntityDestroyed);
    }

    private void OnEntityDestroyed((GameEntity Entity, DestroyedComponent Component) obj)
    {
        _board.RemoveElement(_element);
    }
}