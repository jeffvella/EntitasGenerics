using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Performance;
using Performance.Common;
using Performance.ViewModels;
using Performance.Views;

namespace Performance
{
    public partial class App : Application
    {
        private MainViewModel _viewModel;
        private MainWindow _window;
        private static bool _showingExceptionBox;

        protected override void OnStartup(StartupEventArgs startupEventArgs)
        {
            _viewModel = new MainViewModel();

            LoadSettings();

            _window = new MainWindow();
            _window.DataContext = _viewModel;
            _window.Show();
            Current.Exit += OnExit;

            SetupLogging();

            Game.Start(_viewModel);

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            GlobalCommands.Bind(_viewModel, _window);
        }

        private void LoadSettings()

        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "settings.json";
            if (FileSerializer.TryDeserialize(out SettingsViewModel settings, path))
            {
                _viewModel.Settings = settings;
            }
        }

        private void SetupLogging()
        {
            Logger.Folder = AppDomain.CurrentDomain.BaseDirectory;
            Logger.Log("Initialized");
        }

        private void OnExit(object sender, ExitEventArgs exitEventArgs)
        {
            SaveSettings();

            if (_viewModel.Context.IsProcessing)
            {
                _viewModel.Context.Cancel();

                WaitHandle.WaitAny(new[]
                {
                    _viewModel.Context.CancellationToken.WaitHandle
                });
            }
        }

        private void SaveSettings()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "settings.json";
            FileSerializer.Serialize(_viewModel.Settings, path);
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Console.WriteLine($"Exception: {e.Exception}");
            if (_showingExceptionBox)
            {                
                e.Handled = true;
                return;
            }   
            _showingExceptionBox = true;
            MessageBox.Show(e.Exception.Message);
            _showingExceptionBox = false;
            e.Handled = true;            
        }

        //public void ConfigureBindings()
        //{
        //    GlobalCommands.Initialize(_viewModel);

        //    var binding = new CommandBinding(GlobalCommands.OpenSettingsCommand,
        //        GlobalCommands.OpenSettingsCommandExecute,
        //        GlobalCommands.OpenSettingsCommandCondition);

        //    CommandManager.RegisterClassCommandBinding(typeof(Window), binding);
        //}

    }
}
