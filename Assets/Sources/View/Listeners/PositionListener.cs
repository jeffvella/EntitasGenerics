using Entitas;
using Entitas.Generics;
using Events;
using UnityEngine;

public class PositionListener : MonoBehaviour, IEventListener //, IEventObserver<PositionComponent>
// , IPositionListener
{
    [SerializeField] private float _lerpSpeed = 1f;

    private GameEntity _entity;

    private Vector3 _targetPosition;
    
    public void RegisterListeners(GenericContexts contexts, IEntity entity)
    {
        _entity = (GameEntity) entity;
        //_entity.AddPositionListener(this);

        contexts.Game.RegisterAddedComponentListener<PositionComponent>(_entity, OnPositionChanged);

        var position = contexts.Game.Get<PositionComponent>(_entity);

        OnPositionChanged((_entity, position));
    }

    private void OnPositionChanged((GameEntity Entity, PositionComponent Component) obj)
    {
        _targetPosition = obj.Component.value.ToVector3();
    }

    //public void OnPosition(GameEntity entity, GridPosition value)
    //{
    //    _targetPosition = value.ToVector3();
    //}
    
    private void Update()
    {
        if (transform == null)
            return;

        transform.position = Vector3.Lerp(transform.position, _targetPosition, _lerpSpeed * Time.deltaTime);
    }

    //public void OnEvent(PositionComponent value)
    //{
    //    throw new System.NotImplementedException();
    //}
}
