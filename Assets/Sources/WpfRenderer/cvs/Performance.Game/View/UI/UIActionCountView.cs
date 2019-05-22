using Entitas.MatchLine;
using Performance.ViewModels;
using UnityEngine;

public class UIActionCountView : IView
{
    private SessionViewModel _session;

    public void InitializeView(MainViewModel model, Contexts contexts)
    {
        contexts.GameState.RegisterAddedComponentListener<ActionCountComponent>(OnActionCountChanged);
        contexts.Config.RegisterAddedComponentListener<MaxActionCountComponent>(OnMaxActionCountChanged);

        _session = model.Session;
        _session.MaxActions = Contexts.Instance.Config.GetUnique<MaxActionCountComponent>().Value;
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