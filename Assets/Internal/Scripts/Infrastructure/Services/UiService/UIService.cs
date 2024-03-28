using System.Collections.Generic;
using System.Linq;
using Internal.Scripts.Infrastructure.Services.Localization;
using Internal.Scripts.Infrastructure.Services.ProgressService;
using Internal.Scripts.Infrastructure.Services.Sound;
using Internal.Scripts.Infrastructure.Services.UiService.Base;
using Internal.Scripts.UI.GamePlay;
using Internal.Scripts.UI.Menu;
using Internal.Scripts.UI.ShopUI;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.UiService
{
    public class UIService : IUiService
    {
        private readonly UiConfig _config;
        private readonly IPlayerProgressService _playerProgressService;
        private readonly ILocalizationService _localizationService;
        private readonly ISoundsService _soundsService;
        private readonly Dictionary<UIWindowType, IBaseUiView<BaseUIView>> _presenters = new();
        private MenuUIPresenter _menuUiPresenter;
        private GameplayUIPresenter _gameplayUiPresenter;
        private ShopUIPresenter _shopUiPresenter;

        private Transform _root;

        public UIService(UiConfig config, IPlayerProgressService playerProgressService,
            ILocalizationService localizationService, ISoundsService soundsService)
        {
            _config = config;
            _playerProgressService = playerProgressService;
            _localizationService = localizationService;
            _soundsService = soundsService;
        }

        public void Initialize()
        {
            _root = Object.Instantiate(_config.UIRootPrefab).transform;
            GameObject.DontDestroyOnLoad(_root);
            
            _menuUiPresenter = new MenuUIPresenter(_playerProgressService, _localizationService, _soundsService);
            _gameplayUiPresenter = new GameplayUIPresenter(_soundsService);
            _shopUiPresenter = new ShopUIPresenter(_localizationService, _soundsService);
            
            // TODO: лист презенторов, ковариантность
            _presenters.Add(UIWindowType.MenuWindow, _menuUiPresenter);
            _presenters.Add(UIWindowType.GameplayWindow, _gameplayUiPresenter);
            _presenters.Add(UIWindowType.ShopWindow, _shopUiPresenter);

            foreach (var (type, presenter) in _presenters)
            {
                var viewPrefab = _config.AllWindowConfigs.First(x => x.Type == type).ViewPrefab;
                var view = GameObject.Instantiate(viewPrefab, _root);
                presenter.Initialize(view);
            }
        }

        public IBaseUiView<BaseUIView> GetPresenter<T>() where T : IBaseUiView<BaseUIView>
        {
            return _presenters.FirstOrDefault(x => x.Value is T).Value;
        }
    }
}