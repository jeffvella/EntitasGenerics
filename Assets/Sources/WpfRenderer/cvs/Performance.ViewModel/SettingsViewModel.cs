using Entitas.MatchLine;
using Performance.Common;
using System.Runtime.Serialization;


namespace Performance.ViewModels
{
    [DataContract]
    public class SettingsViewModel : NotifyBase
    {
        private GridSize _mapSize;
        private int _maxActionCount;
        private int _typeCount;
        private int _minMatchCount;

        [DataMember]
        public GridSize GridSize
        {
            get => _mapSize;
            set => SetField(ref _mapSize, value);
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
    }


}




