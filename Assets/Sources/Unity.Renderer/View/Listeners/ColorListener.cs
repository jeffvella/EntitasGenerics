using Entitas;
using UnityEngine;
using Entitas.MatchLine;
using Entitas.Generics;
using System;

public class ColorListener : MonoBehaviour, IAddedComponentListener<GameEntity, ColorComponent>, IEventListener<GameEntity>
{
    [SerializeField] private Renderer _renderer;

    public void RegisterListeners(Contexts contexts, GameEntity entity)
    {
        OnComponentAdded(entity);

        entity.RegisterComponentListener(this);
    }

    public void OnComponentAdded(GameEntity entity)
    {
        _renderer.material.color = entity.Get<ColorComponent>().Component.Value;
    }
}
