using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.ShopSystem;
using Internal.Scripts.Infrastructure.Constants;
using Internal.Scripts.Infrastructure.Services.Analytics;
using Internal.Scripts.Infrastructure.Services.UiService;
using Internal.Scripts.UI.ShopUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.States
{
    public class ShopState : IGameState, IInitializable, ILateDisposable
    {
        private readonly IGameStatesMachine _gameStatesMachine;
        private readonly IUiService _uiService;
        private readonly IShopService _shopService;
        private readonly IAnalyticsService _analyticsService;
        private ShopUIPresenter _shopUIPresenter;

        public ShopState(IGameStatesMachine gameStatesMachine, IUiService uiService, IShopService shopService,
            IAnalyticsService analyticsService)
        {
            _gameStatesMachine = gameStatesMachine;
            _uiService = uiService;
            _shopService = shopService;
            _analyticsService = analyticsService;
        }

        public void Initialize()
        {
            _gameStatesMachine.RegisterState<ShopState>(this);
        }

        public void LateDispose()
        {
            _gameStatesMachine.UnRegisterState<ShopState>();
        }

        public void Enter()
        {
            _shopService.StartWork();

            _shopUIPresenter = (ShopUIPresenter)_uiService.GetPresenter<ShopUIPresenter>();
            _shopUIPresenter.OnMenuClicked += HandleMenuClicked;
            
            _analyticsService.SendCustomEvent("ShopEnter", new Dictionary<string, object>(){{"Entered", true}});
        }

        public void Exit()
        {
            _shopService.StopWork();
            _shopUIPresenter.OnMenuClicked -= HandleMenuClicked;
        }

        private async void HandleMenuClicked()
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(ScenesNames.MENU_SCENE_NAME);

            while (!loadSceneAsync.isDone)
                await UniTask.Yield();
            
            _gameStatesMachine.SetState<MenuState>();
        }
    }
}