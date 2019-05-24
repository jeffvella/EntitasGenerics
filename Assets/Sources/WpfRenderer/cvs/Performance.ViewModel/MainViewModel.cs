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
        private ViewsViewModel _views;

        public MainViewModel()
        {
            _settings = new SettingsViewModel();
            _logViewer = new LogViewerViewModel();
            _board = new BoardViewModel();
            _context = new WorkContext();
            _views = new ViewsViewModel();            
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

        public ViewsViewModel Views
        {
            get => _views;
            set => SetField(ref _views, value);
        }

        public void Pulse()
        {
            
        }

    }
}
