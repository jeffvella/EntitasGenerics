using Entitas.MatchLine;
using Performance.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Performance.ViewModels
{
    public class InputViewModel : NotifyBase
    {
        private bool _isMouseDown;
        private GridPosition _mousePosition;
        private ElementViewModel _currentElement;

        public bool IsMouseDown
        {
            get => _isMouseDown;
            set => SetField(ref _isMouseDown, value);
        }

        public GridPosition MouseGridPosition
        {
            get => _mousePosition;
            set => SetField(ref _mousePosition, value);
        }

        public ElementViewModel CurrentElement
        {
            get => _currentElement;
            set => SetField(ref _currentElement, value);
        }

    }
}

