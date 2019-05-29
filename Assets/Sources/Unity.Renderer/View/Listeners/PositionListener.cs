using Entitas;
using UnityEngine;
using Entitas.MatchLine;

public class PositionListener : MonoBehaviour, IEventListener<GameEntity>
{
    [SerializeField] private float _lerpSpeed = 1f;

    private GameEntity _entity;

    private Vector3 _targetPosition;
    
    //public void RegisterListeners(Contexts contexts, IEntity entity)
    //{
    //    _entity = (GameEntity)entity;

    //    contexts.Game.RegisterAddedComponentListener<PositionComponent>(_entity, OnPositionChanged);

    //    var position = contexts.Game.Get<PositionComponent>(_entity);

    //    OnPositionChanged((_entity, position));
    //}

    public void RegisterListeners(Contexts contexts, GameEntity entity)
    {
        contexts.Game.RegisterAddedComponentListener<PositionComponent>(entity, OnPositionChanged);

        var position = contexts.Game.Get<PositionComponent>(entity);

        OnPositionChanged((entity, position.Component));
    }

    private void OnPositionChanged((GameEntity Entity, PositionComponent Component) obj)
    {
        _targetPosition = obj.Component.Value.ToVector3();
    }
    
    private void Update()
    {
        if (transform == null)
            return;

        transform.position = Vector3.Lerp(transform.position, _targetPosition, _lerpSpeed * Time.deltaTime);
    }
}
