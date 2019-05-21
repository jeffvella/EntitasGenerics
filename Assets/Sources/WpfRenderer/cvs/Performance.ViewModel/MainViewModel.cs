using System.Threading;
using Entitas.MatchLine;
using Performance.Common;
using Performance.Controls;

namespace Performance.ViewModels
{
    public class MainViewModel : NotifyBase, IPulsatingViewModel
    {
        private LogViewerViewModel _logViewer;
        private BoardViewModel _board;
        private SettingsViewModel _settings;
        private WorkContext _context;
        private SessionViewModel _session;

        public MainViewModel()
        {
            _settings = new SettingsViewModel();
            _logViewer = new LogViewerViewModel();
            _board = new BoardViewModel();
            _context = new WorkContext();
            _session = new SessionViewModel();
        }

        public SessionViewModel Session
        {
            get => _session;
            set => SetField(ref _session, value);
        }

        public WorkContext Context
        {
            get => _context;
            set => SetField(ref _context, value);
        }

        public SettingsViewModel Settings
        {
            get => _settings;
            set => SetField(ref _settings, value);
        }

        public LogViewerViewModel LogViewer
        {
            get => _logViewer;
            set => SetField(ref _logViewer, value);
        }

        public BoardViewModel Board
        {
            get => _board;
            set => SetField(ref _board, value);
        }

        public void Pulse()
        {
            
        }



        //public ICommand DownloadCommand => new RelayCommand(param =>
        //{
        //    if (!_context.IsProcessing)
        //    {
        //        _context.Reset();

        //        // do stuff
        //    }
        //});

        //private void OnDownloadFinished(DownloadManager.DownloadResult result)
        //{
        //    if (result.Exception != null)
        //    {
        //        Logger.Error(result.Exception.ToString());
        //    }
        //    Logger.Log("Download Finished");
        //}
    }
}
