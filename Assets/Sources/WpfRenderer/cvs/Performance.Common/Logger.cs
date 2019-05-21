using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Performance.Common
{
    public class LogMessageArgs : EventArgs
    {
        public Color Color { get; set; }
        public DateTime Timestamp { get; set; }
        public LogMessageType Type { get; set; }
    }

    public enum LogMessageType
    {
        None = 0,
        Normal,
        Debug,
        Error
    }

    public static class Logger
    {
        private static string _path;
        private static string _folder;

        public static string Folder
        {
            get { return _folder; }
            set
            {
                var logFolder = System.IO.Path.Combine(value);
                if (!Directory.Exists(logFolder))
                    Directory.CreateDirectory(logFolder);
                _path = System.IO.Path.Combine(logFolder, $"Log[{DateTime.Now:yyyy-MM-dd_hh-mm-ss}].txt");
                _folder = value;
            }
        }

        public delegate void LogMessageDelegate(string message, LogMessageArgs args);

        public static event LogMessageDelegate OnLogMessage;

        public static string Path => _path;

        public static void Log(string message)
        {
            Log(LogMessageType.Normal, Color.FromArgb(0,80,80,80), message);
        }

        public static void Raw(string message)
        {
            Log(LogMessageType.Normal, Color.FromArgb(0,80, 80, 80), message);
        }

        public static void Debug(string message, [CallerMemberName] string caller = "")
        {
            Log(LogMessageType.Debug, Colors.CadetBlue, message, caller);
        }

        public static void Error(string message, [CallerMemberName] string caller = "")
        {
            Log(LogMessageType.Error, Colors.OrangeRed, message, caller);
        }

        private static void Log(LogMessageType type, Color color, string message, string caller = "")
        {           
            message = !string.IsNullOrEmpty(caller) ? $"[{caller}] " + message : message;

            OnLogMessage?.Invoke(message, new LogMessageArgs
            {
                Type = type,
                Color = color,
                Timestamp = DateTime.UtcNow.ToLocalTime()
            });

            WriteToLog(message);               
        }

        public static void WriteToLog(string message)
        {
            try
            {
                if (string.IsNullOrEmpty(_path) || !Directory.Exists(_folder))
                    return;

                using (var logStringWriter = new StreamWriter(_path, true))
                {
                    logStringWriter.WriteLine($"[ {DateTime.Now.ToString(CultureInfo.InvariantCulture)} ] {message}");
                }
            }
            catch
            {
            }
        }

    }
}

