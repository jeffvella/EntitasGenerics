using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;
using Events;
using UnityEngine;

public class SelectedListener : MonoBehaviour, IEventListener
    //ISelectedListener, ISelectedRemovedListener, 
    //IEventObserver<GameEntity, PositionComponent>
{
    [SerializeField] private GameObject _selectedEffect;

    private GameEntity _entity;

    public void RegisterListeners(Contexts contexts, IEntity entity)
    {
        _entity = (GameEntity) entity;

        contexts.Game.RegisterAddedComponentListener<SelectedComponent>(_entity, OnSelected);
        contexts.Game.RegisterRemovedComponentListener<SelectedComponent>(_entity, OnDeselected);

        //contexts.GenericTemp.Game.RegisterAddedComponentListener<SelectedComponent>(OnSelectedComponentRemoved);

        //_entity.AddSelectedListener(this);
        //_entity.AddSelectedRemovedListener(this);

        //// add EventListener component of generic type IPositionListener onto entity if it doesnt exist
        //// otherwise adds to listener collection.
        //Events.Events.TestEvent.Register<IPositionListener>(this);

        //contexts.GenericTemp.Game.RegisterAddedComponentListener<PositionComponent>(_entity, this);

        var isSelected = contexts.Game.IsFlagged<SelectedComponent>(_entity);
        SetSelected(_entity, isSelected);
    }

    private void OnDeselected(GameEntity entity)
    {
        //Debug.Log("Entity Deselected");
        SetSelected(entity, false);
    }

    private void OnSelected((GameEntity Entity, SelectedComponent Component) obj)
    {
        //Debug.Log("Entity Selected");
        SetSelected(obj.Entity, true);
    }

    //public void OnSelected(GameEntity entity)
    //{
    //    SetSelected(entity, true);
    //}

    //public void OnSelectedRemoved(GameEntity entity)
    //{
    //    SetSelected(entity, false);
    //}
    
    private void SetSelected(GameEntity entity, bool value)
    {
        _selectedEffect.SetActive(value);
    }

    //public void OnEvent((GameEntity Entity, PositionComponent Component) value)
    //{
    //    Debug.Log("Event Raised");
    //}
}
