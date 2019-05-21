using UnityEngine;
using Entitas.MatchLine;
using Performance.ViewModels;
using Logger = Performance.Common.Logger;

public sealed class TestInputService : Service, IInputService
{
    public TestInputService(Contexts context, MainViewModel viewModel) : base(context, viewModel)
    {

    }

    private bool _isHolding = false;
    private GridPosition _holdingPosition;
    private bool _isStartedHolding = false;
    private bool _isReleased = false;
    private float _holdingTime = 0f;

    public bool IsHolding()
    {
        return _isHolding;
    }

    public GridPosition HoldingPosition()
    {
        return _holdingPosition;
    }

    public bool IsStartedHolding()
    {
        return _isStartedHolding;
    }

    public float HoldingTime()
    {
        return _holdingTime;
    }

    public bool IsReleased()
    {
        return _isReleased;
    }

    public void Update(float delta)
    {
        if (_viewModel.Board.Input.IsMouseDown)
        {
            if (_isHolding)
            {
                _holdingTime += delta;
                _isStartedHolding = false;
            }
            else
            {
                _holdingTime = 0f;
                _isStartedHolding = true;
            }

            // Reverse the grid layout so that it matches the orientation in Unity.
            // The original project was designed with blocks flipped because of camera direction.
            // Note: this also needs to be taken into account in PositionListener when a position changes.

            var pos = _viewModel.Board.Input.MouseGridPosition.Reverse(_viewModel.Settings.GridSize);
            //Logger.Log($"_holdingPosition={_viewModel.Board.Input.MouseGridPosition} Reverse={pos}");

            _holdingPosition = pos;
            _isHolding = true;
            _isReleased = false;
        }
        else
        {
            if (_isHolding)
            {
                _isHolding = false;
                _isReleased = true;
            }
            else
            {
                _isReleased = false;
            }
        }
    }
}
