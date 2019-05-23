using Performance.Common;
using Entitas.MatchLine;
using Performance.ViewModels;
using Performance.Controls;
using Entitas;
using System;
using System.Linq;
using System.Collections.Generic;

public class UIComboView : IView
{
    private List<ComboDefinition> _comboDefinitions;

    public void InitializeView(MainViewModel model, Contexts contexts, IFactories factories)
    {
        _comboDefinitions = model.Settings.ComboDefinitions.Definitions;

        contexts.Game.RegisterAddedComponentListener<ComboComponent>(OnComboAdded);
    }

    private void OnComboAdded((GameEntity Entity, ComboComponent Component) obj)
    {
        var def = _comboDefinitions[obj.Component.Value];
        Logger.Log($"'{def.Name}' match Combo! Reward={def.Reward}");
    }
}