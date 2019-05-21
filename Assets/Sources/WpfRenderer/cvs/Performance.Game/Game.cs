using Performance.ViewModels;

namespace Performance
{
    public static class Game
    {
        private static TestGameController _gameController;
        private static MainViewModel _viewModel;

        public static void Start(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            _gameController = new TestGameController(_viewModel);
            _gameController.Awake();

        }
    }

}
