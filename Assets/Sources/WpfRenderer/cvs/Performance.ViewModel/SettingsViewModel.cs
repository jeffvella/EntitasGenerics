using Entitas.MatchLine;
using Performance.Common;
using System.Runtime.Serialization;


namespace Performance.ViewModels
{
    [DataContract]
    public class SettingsViewModel : NotifyBase
    {
        private string _savePath = @"C:\Temp\";
        private GridSize _mapSize;

        [DataMember]
        public string SavePath
        {
            get => _savePath;
            set => SetField(ref _savePath, value);
        }

        [DataMember]
        public GridSize GridSize
        {
            get => _mapSize;
            set => SetField(ref _mapSize, value);
        }
    }


}




