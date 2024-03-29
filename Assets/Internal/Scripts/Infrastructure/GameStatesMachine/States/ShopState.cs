using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.ShopSystem;
using Internal.Scripts.Infrastructure.Constants;
using Internal.Scripts.Infrastructure.Injection;
using Internal.Scripts.Infrastructure.Services.Analytics;
using Internal.Scripts.Infrastructure.Services.ProgressService;
using Internal.Scripts.Infrastructure.Services.Sound;
using Internal.Scripts.Infrastructure.Services.UiService;
using Internal.Scripts.UI.ShopUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.States
{
    public class ShopState : GameState
    {
        private readonly IUiService _uiService;
        private readonly IShopService _shopService;
        private readonly IAnalyticsManager _analyticsManager;
        private ShopUIPresenter _shopUIPresenter;

        public ShopState(GameStatesMachine gameStatesSwitcher, IUiService uiService, IShopService shopService,
            IAnalyticsManager analyticsManager) : base(gameStatesSwitcher)
        {
            _uiService = uiService;
            _shopService = shopService;
            _analyticsManager = analyticsManager;
        }

        public override async void Enter()
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(ScenesNames.SHOP_SCENE_NAME);

            while (!loadSceneAsync.isDone)
                await UniTask.Yield();
            
            _shopService.StartWork();

            _shopUIPresenter = (ShopUIPresenter)_uiService.GetPresenter<ShopUIPresenter>();
            _shopUIPresenter.OnMenuClicked += HandleMenuClicked;
            
            _analyticsManager.SendCustomEvent("ShopEnter", new Dictionary<string, object>(){{"Entered", true}});
        }

        public override void Exit()
        {
            _shopService.StopWork();
            _shopUIPresenter.OnMenuClicked -= HandleMenuClicked;
        }

        private void HandleMenuClicked()
        {
            _gameStatesSwitcher.SetState<MenuState>();
        }
    }
}