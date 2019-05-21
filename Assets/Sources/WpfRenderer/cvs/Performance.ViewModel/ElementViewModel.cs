using Performance.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Input;
using Entitas.MatchLine;
using Performance.Controls;

namespace Performance.ViewModels
{
    public class ElementViewModel : NotifyBase
    {
        private List<IViewBehavior> _behaviors;
        private Color _color;
        public GridPosition _gridPosition;
        private string _assetName;
        private ActorType _actorType;
        private bool _isSelected;

        public ElementViewModel()
        {
            _behaviors = new List<IViewBehavior>();
        }

        public string AssetName
        {
            get => _assetName;
            set => SetField(ref _assetName,value);
        }

        public GridPosition GridPosition
        {
            get => _gridPosition;
            set => SetField(ref _gridPosition, value);
        }

        public ActorType ActorType
        {
            get => _actorType;
            set => SetField(ref _actorType, value);
        }

        public List<IViewBehavior> Behaviors
        {
            get => _behaviors;
            set => SetField(ref _behaviors, value);
        }

        public Color Color
        {
            get => _color;
            set => SetField(ref _color, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetField(ref _isSelected, value);
        }

        public T AddBehavior<T>() where T : IViewBehavior, new()
        {
            var behavior = new T();
            _behaviors.Add(behavior);
            return behavior;
        }

        public T GetBehavior<T>() where T : IViewBehavior
        {
            return _behaviors.OfType<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetBehaviors<T>() where T : IViewBehavior
        {
            return _behaviors.OfType<T>();
        }

        public ICommand OnMouseDownCommand => new RelayCommand<InputViewModel>(input =>
        {
            input.IsMouseDown = true;           
        });

        public ICommand OnMouseEnterCommand => new RelayCommand<InputViewModel>(input =>
        {
            input.MouseGridPosition = GridPosition;
            input.CurrentElement = this;           
        });

        public ICommand OnMouseLeaveCommand => new RelayCommand<InputViewModel>(input =>
        {
            input.CurrentElement = null;        
        });


    }
}


