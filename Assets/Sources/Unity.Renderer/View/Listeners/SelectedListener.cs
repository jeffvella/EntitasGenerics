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
        entity.RegisterComponentListener<SelectedComponent>(OnSelected, GroupEvent.Added);
        entity.RegisterComponentListener<SelectedComponent>(OnDeselected, GroupEvent.Removed);

        _selectedEffect.SetActive(entity.IsFlagged<SelectedComponent>());
    }

    private void OnSelected(GameEntity entity)
    {
        _selectedEffect.SetActive(true);
    }

    private void OnDeselected(IEntity entity)
    {
        _selectedEffect.SetActive(false);
    }
}
