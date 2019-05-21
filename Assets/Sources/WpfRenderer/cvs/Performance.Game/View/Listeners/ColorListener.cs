using System.Windows.Media;
using Entitas;
using Entitas.Generics;
using Entitas.MatchLine;
using Performance.Controls;
using Performance.ViewModels;
using UnityEngine;
using Color = System.Windows.Media.Color;
using Logger = Performance.Common.Logger;

public class ColorListener : IEventListener
{
    private BoardViewModel _board;

    //[SerializeField] private Renderer _renderer;
    private ElementViewModel _element;
    private GameEntity _entity;

    public void RegisterListeners(MainViewModel model, ElementViewModel element, Contexts contexts, IEntity entity)
    {
        _board = model.Board;
        _element = element;
        _entity = (GameEntity)entity;

        //var component = contexts.Game.Get<ColorComponent>(_entity);
        //OnColorAdded((_entity, component));

        contexts.Game.RegisterAddedComponentListener<ColorComponent>(_entity, OnColorAdded);
    }

    private void OnColorAdded((GameEntity Entity, ColorComponent Component) obj)
    {
        var unityColor = obj.Component.value;
        _element.Color = Color.FromArgb((byte)(unityColor.a * 255), (byte)(unityColor.r * 255), (byte)(unityColor.g * 255), (byte)(unityColor.b * 255));

        //Logger.Log("Color Added");
        //_renderer.material.color = obj.Component.value;
    }
}
