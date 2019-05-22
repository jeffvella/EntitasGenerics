using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using Entitas.MatchLine;
using Performance.Common;
using Performance.ViewModels;

namespace Performance
{
    public class GameController 
    {
        private Contexts _contexts;
        private RootSystems _rootSystems;
        private IServices _services;
        private MainViewModel _viewModel;

        public GameController(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Awake()
        {
            _contexts = Contexts.Instance;

            Configure(_viewModel.Settings, _contexts);

            _viewModel.Views.Add<UIScoreView>();
            _viewModel.Views.Add<UIRewardView>();
            _viewModel.Views.Add<UIActionCountView>();
            _viewModel.Views.Add<UIRestartView>();
            _viewModel.Views.Add<UIGameOverView>();

            _services = new TestServices
            {
                ViewService = new TestViewService(_contexts, _viewModel),
                InputService = new TestInputService(_contexts, _viewModel),
                TimeService = new TestTimeService(_contexts, _viewModel),
                ElementService = new TestElementService(_contexts, _viewModel),
            };

            _rootSystems = new RootSystems(_contexts, _services);
            _rootSystems.Initialize();

            StartUpdateTimer();
        }

        private void StartUpdateTimer()
        {
            var timer = new System.Timers.Timer(50);
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

        private void Configure(SettingsViewModel settings, Contexts contexts)
        {
            settings.GridSize = new GridSize(6, 6);

            contexts.Config.SetUnique<MapSizeComponent>(c => c.value = settings.GridSize);
            contexts.Config.SetUnique<TypeCountComponent>(c => c.Value = 4);
            contexts.Config.SetUnique<MaxActionCountComponent>(c => c.Value = 20);
            contexts.Config.SetUnique<MinMatchCountComponent>(c => c.value = 3);

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

            contexts.Config.SetUnique<ComboDefinitionsComponent>(c =>
            {
                c.value = value;
            });
        }


    }

}
