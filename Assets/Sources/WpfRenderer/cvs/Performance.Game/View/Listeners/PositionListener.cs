using Entitas;
using Entitas.Generics;
using Entitas.MatchLine;
using Performance.Controls;
using Performance.ViewModels;
using UnityEngine;

public class PositionListener : IEntityListener<GameEntity>
{
    private SettingsViewModel _settings;
    private BoardViewModel _board;

    //[SerializeField] private float _lerpSpeed = 1f;
    private ElementViewModel _element;
    private GameEntity _entity;

   // private Vector3 _targetPosition;

    public void RegisterListeners(MainViewModel model, ElementViewModel element, Contexts contexts, GameEntity entity)
    {
        _settings = model.Settings;
        _board = model.Board;
        _element = element;
        _entity = entity;

        contexts.Game.RegisterAddedComponentListener<PositionComponent>(_entity, OnPositionChanged);        

        var position = contexts.Game.Get<PositionComponent>(_entity);

        OnPositionChanged((_entity, position));
    }

    private void OnPositionChanged((GameEntity Entity, PositionComponent Component) obj)
    {
        var pos = obj.Component.value;

        // Reverse the grid layout so that it matches the orientation in Unity.
        // The original project was designed with blocks flipped because of camera direction.
        // Note this also needs to be reversed in InputService when a selected position is reported.

        _element.GridPosition = obj.Component.value.Reverse(_settings.GridSize);

        //_targetPosition = obj.Component.value.ToVector3();
    }

    private void Update()
    {
        //if (transform == null)
        //    return;

        //transform.position = Vector3.Lerp(transform.position, _targetPosition, _lerpSpeed * Time.deltaTime);
    }
}

