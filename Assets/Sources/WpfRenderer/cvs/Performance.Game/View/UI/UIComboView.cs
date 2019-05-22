using Performance.Common;
using Entitas.MatchLine;
using Performance.ViewModels;
using Performance.Controls;
using Entitas;
using System;

public class UIComboView : IView
{
    public void InitializeView(MainViewModel model, Contexts contexts)
    {
        contexts.Game.RegisterAddedComponentListener<ComboComponent>(OnComboAdded);
    }

    private void OnComboAdded((GameEntity Entity, ComboComponent Component) obj)
    {
        Logger.Log($"{obj.Component.Value} match Combo!");
    }
}