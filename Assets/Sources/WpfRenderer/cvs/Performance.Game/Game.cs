using Performance.ViewModels;

namespace Performance
{
    public static class Game
    {
        private static GameController _gameController;
        private static MainViewModel _viewModel;

        public static void Start(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            _gameController = new GameController(_viewModel);
            _gameController.Awake();

        }
    }

}
