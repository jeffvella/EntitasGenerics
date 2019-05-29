using Entitas;
using UnityEngine;
using Entitas.MatchLine;
using Entitas.Generics;
using System;

public class SelectedListener : MonoBehaviour, IEventListener<GameEntity>
{ 
    [SerializeField] private GameObject _selectedEffect;

    public void RegisterListeners(Contexts contexts, GameEntity entity)
    {
        contexts.Game.RegisterAddedComponentListener<SelectedComponent>(entity, OnSelected);
        contexts.Game.RegisterRemovedComponentListener<SelectedComponent>(entity, OnDeselected);

        //entity.RegisterAddedComponentListener<SelectedComponent>(OnSelected);
        //entity.RegisterRemovedComponentListener<SelectedComponent>(OnDeselected);

        var isSelected = contexts.Game.IsFlagged<SelectedComponent>(entity);

        _selectedEffect.SetActive(isSelected);
    }

    private void OnSelected((GameEntity Entity, SelectedComponent Component) obj)
    {
        _selectedEffect.SetActive(true);
    }

    private void OnDeselected(IEntity entity)
    {
        _selectedEffect.SetActive(false);
    }
}