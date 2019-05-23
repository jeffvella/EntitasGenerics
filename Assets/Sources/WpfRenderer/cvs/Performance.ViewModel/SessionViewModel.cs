using Performance.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.ViewModels
{
    public class SessionViewModel : NotifyBase
    {
        private long _score;
        private long _actions;
        private int _maxActions;
        private bool _isGameOver;
        private int _boardRows;
        private int _boardColumns;

        public void ApplySettings(SettingsViewModel settings)
        {
            MaxActions = settings.MaxActionCount;
            BoardRows = settings.BoardSize.Y;
            BoardColumns = settings.BoardSize.X;
        }

        public long Score
        {
            get => _score;
            set => SetField(ref _score, value);
        }

        public long Actions
        {
            get => _actions;
            set => SetField(ref _actions, value, nameof(ActionsText));
        }

        public int MaxActions
        {
            get => _maxActions;
            set => SetField(ref _maxActions, value, nameof(ActionsText));
        }

        public string ActionsText
        {
            get => $"{_actions}/{_maxActions}";
        }

        public bool IsGameOver
        {
            get => _isGameOver;
            set => SetField(ref _isGameOver, value);
        }

        public int BoardRows
        {
            get => _boardRows;
            set => SetField(ref _boardRows, value);
        }

        public int BoardColumns
        {
            get => _boardColumns;
            set => SetField(ref _boardColumns, value);
        }
    }
}


