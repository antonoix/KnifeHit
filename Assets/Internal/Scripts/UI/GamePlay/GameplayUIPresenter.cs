namespace Internal.Scripts.UI.GamePlay
{
    public class GameplayUIPresenter
    {
        private readonly GameplayUIView _viewPrefab;
        private GameplayUIView _view;
        
        public GameplayUIPresenter(GameplayUIView viewPrefab)
        {
            _viewPrefab = viewPrefab;
        }
    }
}
