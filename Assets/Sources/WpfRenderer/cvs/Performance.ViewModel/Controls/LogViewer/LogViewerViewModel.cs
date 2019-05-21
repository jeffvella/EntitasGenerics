using System;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Media;
using Performance.Controls;
using Performance.Common;

namespace Performance.Controls
{
    public class LogEntry
    {
        public DateTime DateTime { get; set; }

        public string Message { get; set; }

        public Color Color { get; set; } = Color.FromArgb(0, 220, 220, 220);
    }

    public class LogViewerViewModel
    {
        private readonly SynchronizationContext _syncContext;

        public LogViewerViewModel()
        {
            _syncContext = SynchronizationContext.Current;
            Logger.OnLogMessage += OnLogMessage;
        }

        public LogViewerViewModel(SynchronizationContext syncContext)
        {
            _syncContext = syncContext;
            Logger.OnLogMessage += OnLogMessage;

        }

        private const int MaxLogMessages = 500;

        public ObservableCollection<LogEntry> Entries { get; set; } = new ObservableCollection<LogEntry>();

        private void OnLogMessage(string message, LogMessageArgs args)
        {
            _syncContext.Post(state =>
            {
                if (Entries.Count > MaxLogMessages)
                    Entries.RemoveAt(0);

                Entries.Add(new LogEntry
                {
                    Message = message,
                    DateTime = args.Timestamp,
                    Color = args.Color
                });

            }, null);
        }
    }
}