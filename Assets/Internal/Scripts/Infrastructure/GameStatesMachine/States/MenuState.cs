using Cysharp.Threading.Tasks;
using Internal.Scripts.Infrastructure.Constants;
using Internal.Scripts.Infrastructure.Services.UiService;
using Internal.Scripts.UI.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.States
{
    public class MenuState : IGameState, IInitializable, ILateDisposable
    {
        private readonly IGameStatesMachine _gameStatesMachine;
        private readonly IUiService _uiService;
        private MenuUIPresenter _menuUIPresenter;

        public MenuState(IGameStatesMachine gameStatesMachine, IUiService uiService)
        {
            _gameStatesMachine = gameStatesMachine;
            _uiService = uiService;
        }

        public void Initialize()
        {
            _gameStatesMachine.RegisterState<MenuState>(this);
        }

        public void LateDispose()
        {
            _gameStatesMachine.UnRegisterState<MenuState>();
        }

        public void Enter()
        {
            _menuUIPresenter = (MenuUIPresenter) _uiService.GetPresenter<MenuUIPresenter>();
            
            _menuUIPresenter.Show();
            _menuUIPresenter.OnStartBtnClicked += HandleStartBtnClick;
            _menuUIPresenter.OnShopBtnClicked += HandleShopBtnClick;
        }

        public void Exit()
        {
            _menuUIPresenter.Hide();
            
            _menuUIPresenter.OnStartBtnClicked -= HandleStartBtnClick;
            _menuUIPresenter.OnShopBtnClicked -= HandleShopBtnClick;
        }

        private async void HandleStartBtnClick()
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(ScenesNames.GAMEPLAY_SCENE_NAME);

            while (!loadSceneAsync.isDone)
                await UniTask.Yield();
            
            _gameStatesMachine.SetState<GamePlayState>();
        }

        private async void HandleShopBtnClick()
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(ScenesNames.SHOP_SCENE_NAME);

            while (!loadSceneAsync.isDone)
                await UniTask.Yield();
            
            _gameStatesMachine.SetState<ShopState>();
        }
    }
}
