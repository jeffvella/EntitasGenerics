using Entitas.MatchLine;
using Performance.Common;
using Performance.ViewModel.Resources;
using System.Runtime.Serialization;


namespace Performance.ViewModels
{
    [DataContract]
    public class SettingsViewModel : NotifyBase
    {
        private BoardSize _boardSize;
        private int _maxActionCount;
        private int _typeCount;
        private int _minMatchCount;
        private ComboDefinitions _comboDefinitions;

        [DataMember]
        public BoardSize BoardSize
        {
            get => _boardSize;
            set => SetField(ref _boardSize, value);
        }

        [DataMember]
        public int MaxActionCount
        {
            get => _maxActionCount;
            set => SetField(ref _maxActionCount, value);
        }

        [DataMember]
        public int TypeCount
        {
            get => _typeCount;
            set => SetField(ref _typeCount, value);
        }

        [DataMember]
        public int MinMatchCount
        {
            get => _minMatchCount;
            set => SetField(ref _minMatchCount, value);
        }

        [DataMember]
        public ComboDefinitions ComboDefinitions
        {
            get => _comboDefinitions;
            set => SetField(ref _comboDefinitions, value);
        }
    }


}




