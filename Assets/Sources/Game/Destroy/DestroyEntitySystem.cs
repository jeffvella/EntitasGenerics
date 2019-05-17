using System.Collections.Generic;
using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;

public sealed class DestroyEntitySystem : ICleanupSystem
{
    private readonly IGroup<GameEntity> _gameGroup;
    private readonly List<GameEntity> _gameBuffer;

    private readonly IGroup<InputEntity> _inputGroup;
    private readonly List<InputEntity> _inputBuffer;

    public DestroyEntitySystem(Contexts contexts)
    {
        _gameGroup = contexts.Game.GetGroup<DestroyedComponent>();
        _gameBuffer = new List<GameEntity>();

        _inputGroup = contexts.Input.GetGroup<DestroyedComponent>();
        _inputBuffer = new List<InputEntity>();
    }

    public void Cleanup()
    {
        foreach (GameEntity e in _gameGroup.GetEntities(_gameBuffer))
        {
            e.Destroy();
        }

        foreach (var e in _inputGroup.GetEntities(_inputBuffer))
        {
            e.Destroy();
        }
    }
}