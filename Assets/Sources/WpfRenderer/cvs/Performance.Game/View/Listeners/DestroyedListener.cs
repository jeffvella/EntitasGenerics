using UnityEngine;
using Performance.Common;
using Entitas.MatchLine;
using Performance.ViewModels;
using Performance.Controls;
using Performance;

public class DestroyedListener : IEntityListener<GameEntity>
{
    private BoardViewModel _board;
    private ElementViewModel _element;
    private IElementFactory _elementFactory;

    public void RegisterListeners(MainViewModel model, ElementViewModel element, Contexts contexts, IFactories factories, GameEntity entity)
    {
        _board = model.Board;
        _element = element;
        _elementFactory = factories.ElementFactory;

        contexts.Game.RegisterAddedComponentListener<DestroyedComponent>(entity, OnEntityDestroyed);
    }

    private void OnEntityDestroyed((GameEntity Entity, DestroyedComponent Component) obj)
    {
        _board.RemoveElement(_element);
        _elementFactory.Return(_element);
    }
}