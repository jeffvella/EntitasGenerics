using Entitas;
using Events;
using UnityEngine;

public class SelectedListener : MonoBehaviour, IEventListener, ISelectedListener
    , ISelectedRemovedListener, IEventObserver<(GameEntity Entity, PositionComponent Component)>
{
    [SerializeField] private GameObject _selectedEffect;

    private GameEntity _entity;

    public void RegisterListeners(Contexts contexts, IEntity entity)
    {
        _entity = (GameEntity) entity;
        _entity.AddSelectedListener(this);
        _entity.AddSelectedRemovedListener(this);

        //// add EventListener component of generic type IPositionListener onto entity if it doesnt exist
        //// otherwise adds to listener collection.
        //Events.Events.TestEvent.Register<IPositionListener>(this);

        //contexts.GenericTemp.Game.AddEventListener<PositionComponent>(_entity, this);

        SetSelected(_entity, _entity.isSelected);
    }

    public void OnSelected(GameEntity entity)
    {
        SetSelected(entity, true);
    }

    public void OnSelectedRemoved(GameEntity entity)
    {
        SetSelected(entity, false);
    }
    
    private void SetSelected(GameEntity entity, bool value)
    {
        _selectedEffect.SetActive(value);
    }

    public void OnRaised((GameEntity Entity, PositionComponent Component) value)
    {
        Debug.Log("Event Raised");
    }
}
