using Entitas.Generics;
using Entitas.MatchLine;
using Entitas;
using Performance.Controls;
using Performance.ViewModels;
using System;
using Performance.Common;

public class UIScoreView : IView
{
    private SessionViewModel _session;

    public void InitializeView(MainViewModel model, Contexts contexts)
    {
        _session = model.Session;
        contexts.GameState.RegisterAddedComponentListener<ScoreComponent>(OnScoreChanged);
    }

    private void OnScoreChanged((GameStateEntity Entity, ScoreComponent Component) obj)
    {
        Logger.Log($"Score changed to {obj.Component.Value}");

        _session.Score = obj.Component.Value;
    }
}
