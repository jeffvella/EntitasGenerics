using UnityEngine;
using Entitas.MatchLine;
using Performance.ViewModels;

public class UIGameOverView : IView
{
    private SessionViewModel _session;

    public void InitializeView(MainViewModel model, Contexts contexts, IFactories factories)
    {
        _session = model.Board.Session;

        contexts.GameState.RegisterAddedComponentListener<GameOverComponent>(OnGameOverAdded);
        contexts.GameState.RegisterRemovedComponentListener<GameOverComponent>(OnGameOverRemoved);
        SetGameOver(contexts.GameState.IsFlagged<GameOverComponent>());
    }

    private void OnGameOverAdded((GameStateEntity Entity, GameOverComponent Component) obj)
    {
        SetGameOver(true);
    }

    public void OnGameOverRemoved(GameStateEntity entity)
    {
        SetGameOver(false);
    }

    private void SetGameOver(bool value)
    {
        _session.IsGameOver = value;
    }
}