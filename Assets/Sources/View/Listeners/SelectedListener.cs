using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;
using UnityEngine;

public class SelectedListener : MonoBehaviour, IEventListener
{ 
    [SerializeField] private GameObject _selectedEffect;

    public void RegisterListeners(Contexts contexts, IEntity entity)
    {       
        if (entity is GameEntity gameEntity)
        {
            gameEntity.RegisterAddedComponentListener<SelectedComponent>(OnSelected);
            gameEntity.RegisterRemovedComponentListener<SelectedComponent>(OnDeselected);
            _selectedEffect.SetActive(gameEntity.IsFlagged<SelectedComponent>());
        }
    }

    private void OnSelected((IEntity Entity, SelectedComponent Component) obj)
    {
        _selectedEffect.SetActive(true);
    }

    private void OnDeselected(IEntity entity)
    {
        _selectedEffect.SetActive(false);
    }

}