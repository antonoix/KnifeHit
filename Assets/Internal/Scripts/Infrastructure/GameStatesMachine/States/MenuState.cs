using Cysharp.Threading.Tasks;
using Internal.Scripts.Infrastructure.Constants;
using Internal.Scripts.Infrastructure.Services.UiService;
using Internal.Scripts.UI.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.States
{
    public class MenuState : GameState
    {
        private readonly IUiService _uiService;
        private MenuUIPresenter _menuUIPresenter;
    
        public MenuState(IGameStatesSwitcher gameStatesSwitcher, IUiService uiService) 
            : base(gameStatesSwitcher)
        {
            _uiService = uiService;
        }

        public override async void Enter()
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(ScenesNames.MENU_SCENE_NAME);

            while (!loadSceneAsync.isDone)
                await UniTask.Yield();
            
            _menuUIPresenter = (MenuUIPresenter) _uiService.GetPresenter<MenuUIPresenter>();
            
            _menuUIPresenter.Show();
            _menuUIPresenter.OnStartBtnClicked += _gameStatesSwitcher.SetState<GamePlayState>;
        }

        public override void Exit()
        {
            _menuUIPresenter.Dispose();
            _menuUIPresenter.Hide();
            
            _menuUIPresenter.OnStartBtnClicked -= _gameStatesSwitcher.SetState<GamePlayState>;
        }
    }
}
