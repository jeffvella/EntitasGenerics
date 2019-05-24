using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using Entitas.MatchLine;
using Performance.Common;
using Performance.ViewModel.Resources;
using Performance.ViewModels;

namespace Performance
{
    public class GameController 
    {
        private Contexts _contexts;
        private RootSystems _rootSystems;
        private IServices _services;
        private MainViewModel _viewModel;
        private IFactories _factories;

        public GameController(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Awake()
        {
            _contexts = Contexts.Instance;

            Configure(_viewModel, _contexts);

            // The difference between a 'View' and an 'EntityListener' is that listeners are created to monitor a specific (already created) entity
            // whereas Views are being initialized once here and can monitor against any entity that may be created at any point.

            _viewModel.Views.Add<UIScoreView>();
            _viewModel.Views.Add<UIRewardView>();
            _viewModel.Views.Add<UIActionCountView>();
            _viewModel.Views.Add<UIRestartView>();
            _viewModel.Views.Add<UIGameOverView>();
            _viewModel.Views.Add<UIComboView>();
            _viewModel.Views.Add<UISettingsSync>();

            _factories = new TestFactories
            {
                ElementFactory = new ElementPool()
            };

            _services = new TestServices
            {
                ViewService = new TestViewService(_contexts, _viewModel, _factories),
                InputService = new TestInputService(_contexts, _viewModel, _factories),
                TimeService = new TestTimeService(_contexts, _viewModel, _factories),
                ElementService = new TestElementService(_contexts, _viewModel, _factories),
            };

            _rootSystems = new RootSystems(_contexts, _services);
            _rootSystems.Initialize();

            StartUpdateTimer();

            StartGame();
        }

        private void StartGame()
        {
            _viewModel.Board.Session.ApplySettings(_viewModel.Settings);
        }

        private void StartUpdateTimer()
        {
            var timer = new System.Timers.Timer(25);
            timer.Elapsed += TimerTick;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void TimerTick(object sender, ElapsedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            _rootSystems.Execute();
            _rootSystems.Cleanup();
        }

        private void OnDestroy()
        {
            _rootSystems.DeactivateReactiveSystems();
            _rootSystems.ClearReactiveSystems();
            _rootSystems.TearDown();
        }

        private void Configure(MainViewModel model, Contexts contexts)
        {
            var settings = model.Settings;

            if (settings.BoardSize == null)
                settings.BoardSize = new BoardSize(6, 8);

            if (settings.TypeCount == 4)
                settings.TypeCount = 4;

            if (settings.MaxActionCount == default)
                settings.MaxActionCount = 15;

            if(settings.MinMatchCount == default)
                settings.MinMatchCount = 3;            
           
            contexts.Config.SetUnique<MapSizeComponent>(c => c.Value = settings.BoardSize);
            contexts.Config.SetUnique<TypeCountComponent>(c => c.Value = settings.TypeCount);
            contexts.Config.SetUnique<MaxActionCountComponent>(c => c.Value = settings.MaxActionCount);
            contexts.Config.SetUnique<MinMatchCountComponent>(c => c.Value = settings.MinMatchCount);

            contexts.Config.SetUnique<ScoringTableComponent>(c =>
            {
                c.value = new List<int> { 0, 10, 30, 90, 200, 500, 1200, 2500 };
            });

            contexts.Config.SetUnique<ExplosiveScoringTableComponent>(c =>
            {
                c.value = new List<int> { 300, 900, 1200, 2000 };
            });

            const string definitionsPath = "Assets/Resources/ComboDefinitions.json";
            var projectPath = AppDomain.CurrentDomain.BaseDirectory;
            var basePath = string.Concat(new Uri(projectPath).Segments.Skip(1).TakeWhile(s => s != "Assets/"));
            var filePath = Path.Combine(basePath, definitionsPath);
            var json = File.ReadAllText(filePath);
            var value = JsonSerializer.Deserialize<ComboDefinitions>(json);
            settings.ComboDefinitions = value;

            contexts.Config.SetUnique<ComboDefinitionsComponent>(c =>
            {
                c.value = value;
            });
        }


    }

}
