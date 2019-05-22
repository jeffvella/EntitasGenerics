using UnityEngine;
using Entitas.MatchLine;
using Performance.ViewModels;

public class UIRewardView : IView
{
    private int _lastValue = 0;
    private SessionViewModel _session;

    public void InitializeView(MainViewModel model, Contexts contexts)
    {
        _session = model.Session;
        contexts.GameState.RegisterAddedComponentListener<ScoreComponent>(OnScoreChanged);
    }

    private void OnScoreChanged((GameStateEntity Entity, ScoreComponent Component) obj)
    {
        _session.Score = obj.Component.Value;
    }

    private void Start()
    {
        Contexts.Instance.GameState.RegisterAddedComponentListener<ScoreComponent>(OnScoreAddedEvent);
    }

    private void OnScoreAddedEvent((GameStateEntity Entity, ScoreComponent Component) obj)
    {
        OnScore(obj.Entity, obj.Component.Value);
    }

    public void OnScore(GameStateEntity entity, int value)
    {
        if (value == _lastValue) return;        
        var difference = value - _lastValue;
                
        // todo: show the difference of score
        
        _lastValue = value;
    }


}