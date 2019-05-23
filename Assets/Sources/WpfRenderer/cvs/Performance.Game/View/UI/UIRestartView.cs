using Entitas.MatchLine;
using Performance.ViewModels;
using Entitas.Generics;
using Performance.Common;
using System;

public class UIRestartView : IView
{
    private IGenericContext<InputEntity> _input;
    private SettingsViewModel _settings;
    private IGenericContext<ConfigEntity> _config;

    public void InitializeView(MainViewModel model, Contexts contexts, IFactories factories)
    {
        _input = contexts.Input;
        _settings = model.Settings;
        _config = contexts.Config;

        model.Board.Input.RestartClicked += OnPressed;
    }

    public void OnPressed()
    {
        Logger.Log("Game Restarted");

        ApplyConfig();
        TriggerRestart();
    }

    public void ApplyConfig()
    {
        _config.SetUnique<MapSizeComponent>(c => c.Value = _settings.BoardSize);
        _config.SetUnique<TypeCountComponent>(c => c.Value = _settings.TypeCount);
        _config.SetUnique<MaxActionCountComponent>(c => c.Value = _settings.MaxActionCount);
        _config.SetUnique<MinMatchCountComponent>(c => c.Value = _settings.MinMatchCount);
    }

    private void TriggerRestart()
    {
        var e = _input.CreateEntity();
        e.SetFlag<RestartComponent>();
        e.SetFlag<DestroyedComponent>();
    }
}