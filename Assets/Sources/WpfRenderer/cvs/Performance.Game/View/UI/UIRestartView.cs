using Entitas.MatchLine;
using Performance.ViewModels;
using Entitas.Generics;
using Performance.Common;

public class UIRestartView : IView
{
    private IGenericContext<InputEntity> _input;

    public void InitializeView(MainViewModel model, Contexts contexts)
    {
        _input = contexts.Input;

        model.Board.Input.RestartClicked += OnPressed;
    }

    public void OnPressed()
    {
        Logger.Log("Game Restarted");

        var e = _input.CreateEntity();    
        e.SetFlag<RestartComponent>();
        e.SetFlag<DestroyedComponent>();
    }
}