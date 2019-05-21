using Entitas;
using UnityEngine;
using Entitas.MatchLine;

public class ColorListener : MonoBehaviour, IEventListener
{
    [SerializeField] private Renderer _renderer;

    private GameEntity _entity;

    public void RegisterListeners(Contexts contexts, IEntity entity)
    {
        _entity = (GameEntity)entity;

        var component = contexts.Game.Get<ColorComponent>(_entity);

        OnColorAdded((_entity, component));

        contexts.Game.RegisterAddedComponentListener<ColorComponent>(_entity, OnColorAdded);
    }

    private void OnColorAdded((GameEntity Entity, ColorComponent Component) obj)
    {
        _renderer.material.color = obj.Component.value;
    }
}
