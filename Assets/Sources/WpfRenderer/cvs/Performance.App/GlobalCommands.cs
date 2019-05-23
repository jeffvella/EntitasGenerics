
using Performance.Common;
using Performance.ViewModels;
using Performance.Views;
using Performance.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Performance
{
    public static class GlobalCommands
    {
        private static MainViewModel _model;
        private static MainWindow _mainWindow;

        public static void Bind(MainViewModel model, MainWindow mainWindow)
        {
            _model = model;
            _mainWindow = mainWindow;

            var openSettingsCommandBinding = new CommandBinding(OpenSettingsCommand, OpenSettingsCommandExecute, (s, e) => e.CanExecute = true);

            CommandManager.RegisterClassCommandBinding(typeof(Window), openSettingsCommandBinding);
        }

        public static RoutedUICommand OpenSettingsCommand { get; } = new RoutedUICommand("Open Settings Window", "OpenSettingsCommand", typeof(GlobalCommands));

        internal static void OpenSettingsCommandExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var window = new SettingsWindow();
            window.DataContext = _model.Settings;
            window.Owner = _mainWindow;
            window.Name = "SettingsWindow";
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

    }
    
}
