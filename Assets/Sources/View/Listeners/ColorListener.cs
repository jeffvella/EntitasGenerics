using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;
using Events;
using UnityEngine;

public class ColorListener : MonoBehaviour, IEventListener
// , IColorListener
{
    [SerializeField] private Renderer _renderer;

    private GameEntity _entity;

    public void RegisterListeners(GenericContexts contexts, IEntity entity)
    {
        _entity = (GameEntity)entity;

        //_entity.AddColorListener(this);

        contexts.Game.RegisterAddedComponentListener<ColorComponent>(_entity, OnColorAdded);

        var component = contexts.Game.Get<ColorComponent>(_entity);

        OnColorAdded((_entity, component));

        //OnColor(_entity, _entity.color.value);
    }

    private void OnColorAdded((GameEntity Entity, ColorComponent Component) obj)
    {
        _renderer.material.color = obj.Component.value;
    }

    //public void OnColor(GameEntity entity, Color value)
    //{
    //    _renderer.material.color = value;
    //}

    //public void OnEvent(ColorComponent color)
    //{
    //    _renderer.material.color = color.value
    //}

}
