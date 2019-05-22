//using Entitas.Unity;
using UnityEngine;
using Performance.Common;
using Entitas.MatchLine;
using Performance.ViewModels;
using Performance.Controls;

public class DestroyedListener : IEventListener<GameEntity>
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
        Performance.Common.Logger.Log("Destroyed Entity");

        _board.RemoveElement(_element);
    }
}


//using Entitas;
//using Entitas.Generics;
//using UnityEngine;

//public class ColorListener : MonoBehaviour, IEventListener
//{
//    [SerializeField] private Renderer _renderer;

//    private GameEntity _entity;

//    public void RegisterListeners(Contexts contexts, IEntity entity)
//    {
//        _entity = (GameEntity)entity;

//        var component = contexts.Game.Get<ColorComponent>(_entity);

//        OnColorAdded((_entity, component));

//        contexts.Game.RegisterAddedComponentListener<ColorComponent>(_entity, OnColorAdded);
//    }

//    private void OnColorAdded((GameEntity Entity, ColorComponent Component) obj)
//    {
//        _renderer.material.color = obj.Component.value;
//    }
//}
