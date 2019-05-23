using Performance.Common;
using Entitas.MatchLine;
using Performance.ViewModels;
using Performance.Controls;
using Entitas;
using System;
using System.Linq;
using System.Collections.Generic;
using Entitas.Generics;

/// <summary>
/// Passes user settings changes back into the entities system config components
/// </summary>
public class UISettingsSync : IView
{
    private MainViewModel _model;
    private IGenericContext<ConfigEntity> _config;

    public void InitializeView(MainViewModel model, Contexts contexts, IFactories factories)
    {
        _model = model;
        _config = contexts.Config;
        _model.Settings.PropertyChanged += Settings_PropertyChanged;
    }

    private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {        
        switch(e.PropertyName)
        {
            // Note board size changes are applied on restart, other settings are applied here instantly.

            case nameof(SettingsViewModel.MaxActionCount):
                _config.SetUnique<MaxActionCountComponent>(c => c.Value = _model.Settings.MaxActionCount);
                break;

            case nameof(SettingsViewModel.MinMatchCount):
                _config.SetUnique<MinMatchCountComponent>(c => c.Value = _model.Settings.MinMatchCount);
                break;

            case nameof(SettingsViewModel.TypeCount):
                _config.SetUnique<TypeCountComponent>(c => c.Value = _model.Settings.TypeCount);
                break;
        }
    }

}