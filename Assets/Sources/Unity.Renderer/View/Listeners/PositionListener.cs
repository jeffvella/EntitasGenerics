using Entitas;
using UnityEngine;
using Entitas.MatchLine;
using System;

public class PositionListener : MonoBehaviour, IEventListener<GameEntity>
{
    [SerializeField] private float _lerpSpeed = 1f;
    private GameEntity _entity;
    private Vector3 _targetPosition;
    
    public void RegisterListeners(Contexts contexts, GameEntity entity)
    {
        entity.RegisterComponentListener2<PositionComponent>(OnPositionChanged, GroupEvent.Added);

        //OnPositionChanged(entity);
    }

    private void OnPositionChanged(GameEntity entity)
    {        
        _targetPosition = entity.Get2<PositionComponent>().Value.ToVector3();
    }
    
    private void Update()
    {
        if (transform == null)
            return;

        transform.position = Vector3.Lerp(transform.position, _targetPosition, _lerpSpeed * Time.deltaTime);
    }
}
