using Performance.Common;
using System;
using System.Threading;
using System.Windows.Input;

namespace Performance.ViewModels
{
    public class WorkContext : NotifyBase, IProgress<float>, IProgress<string>
    {
        public WorkContext()
        {
            Reset();
        }

        private float _progress;
        private bool _isProcessing;
        private SynchronizationContext _sync;   
        private CancellationTokenSource _cts;
        private PauseTokenSource _pts;

        public CancellationToken CancellationToken { get; private set; }

        public PauseToken PauseToken { get; private set; }

        public ICommand CancelCommand => new RelayCommand(param =>
        {
            Cancel();
        });

        public ICommand TogglePauseCommand => new RelayCommand(param =>
        {
            _pts.IsPaused = !_pts.IsPaused;
        });

        public void Cancel()
        {
            _cts.Cancel();
        }

        public void Pause() => _pts.IsPaused = true;
        public void UnPause() => _pts.IsPaused = false;

        public void Reset()
        {
            _sync = SynchronizationContext.Current;

            _cts = new CancellationTokenSource();
            CancellationToken = _cts.Token;

            _pts = new PauseTokenSource();
            PauseToken = _pts.Token;

            Progress = 0f;
            IsProcessing = false;
        }

        public float Progress
        {
            get => _progress;
            set => SetField(ref _progress, value);
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set => SetField(ref _isProcessing, value);
        }

        public SynchronizationContext Sync
        {
            get => _sync;
            set => SetField(ref _sync, value);
        }

        /// <summary>
        /// Informs of the current progress percentage
        /// </summary>
        /// <param name="percentage">A percentage (out of 100) e.g. 25.36</param>
        public void Report(float percentage)
        {
            if (percentage >= 100)
            {
                IsProcessing = false;
                Progress = 100;
                return;
            }
            if (!IsProcessing)
            {
                IsProcessing = true;
            }
            Progress = percentage;
        }

        public void Report(string value)
        {
            _sync.Post(state =>
            {
                Logger.Log(value);

            }, null);
        }
    }

}

