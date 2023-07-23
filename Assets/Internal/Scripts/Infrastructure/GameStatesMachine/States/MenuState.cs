using Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies;
using Internal.Scripts.UI;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.States
{
    public class MenuState : GameState
    {
        private readonly MenuUIPresenter _uiPresenter;
    
        public MenuState(IGameStatesSwitcher gameStatesSwitcher, IGameStateDepedency gameStateDependency) 
            : base(gameStatesSwitcher, gameStateDependency)
        {
            _uiPresenter = new MenuUIPresenter(((MenuStateDependency)gameStateDependency).MenuUIPrefab);
        }

        public override void Enter()
        {
            _uiPresenter.Initialize();

            _uiPresenter.OnStartBtnClicked += _gameStatesSwitcher.SetState<GamePlayState>;
        }

        public override void Exit()
        {
            _uiPresenter.Dispose();
            
            _uiPresenter.OnStartBtnClicked -= _gameStatesSwitcher.SetState<GamePlayState>;
        }
    }
}
