using System;
using Entitas.MatchLine;
using Performance.ViewModels;
using UnityEngine;

public class UIActionCountView : IView
{
    private SessionViewModel _session;

    public void InitializeView(MainViewModel model, Contexts contexts, IFactories factories)
    {
        contexts.GameState.RegisterAddedComponentListener<ActionCountComponent>(OnActionCountChanged);
        contexts.Config.RegisterAddedComponentListener<MaxActionCountComponent>(OnMaxActionCountChanged);
        contexts.Config.RegisterAddedComponentListener<MapSizeComponent>(OnMapSizeChanged);

        _session = model.Board.Session;
        _session.MaxActions = Contexts.Instance.Config.GetUnique<MaxActionCountComponent>().Value;
    }

    private void OnMapSizeChanged((ConfigEntity Entity, MapSizeComponent Component) obj)
    {
        _session.BoardColumns = obj.Component.Value.x;
        _session.BoardRows = obj.Component.Value.y;
    }

    private void OnActionCountChanged((GameStateEntity Entity, ActionCountComponent Component) obj)
    {        
        _session.Actions = obj.Component.value;
    }

    private void OnMaxActionCountChanged((ConfigEntity Entity, MaxActionCountComponent Component) obj)
    {
        _session.MaxActions = obj.Component.Value;
    }

}