using Entitas;
using UnityEngine;
using Entitas.MatchLine;
using Entitas.Generics;
using System;

public class ColorListener : MonoBehaviour, IAddedComponentListener<GameEntity, ColorComponent>, IEventListener<GameEntity>
{
    [SerializeField] private Renderer _renderer;

    GameEntity _entity;

    public void RegisterListeners(Contexts contexts, GameEntity entity)
    {
        _entity = entity;

        OnComponentAdded(_entity);

        //_entity.RegisterComponentListener(this);

        _entity.RegisterComponentListener<ColorComponent>(OnColorChanged, GroupEvent.Added);
    }

    private void OnColorChanged(GameEntity entity)
    {
        _renderer.material.color = entity.Get<ColorComponent>().Component.Value;
    }

    public void OnComponentAdded(GameEntity entity)
    {
        _renderer.material.color = entity.Get<ColorComponent>().Component.Value;
    }


}
