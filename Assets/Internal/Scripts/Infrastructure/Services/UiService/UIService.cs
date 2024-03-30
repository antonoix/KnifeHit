using System.Collections.Generic;
using System.Linq;
using Internal.Scripts.Infrastructure.Services.UiService.Base;
using Internal.Scripts.UI.GamePlay;
using Internal.Scripts.UI.Menu;
using Internal.Scripts.UI.ShopUI;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Services.UiService
{
    public class UIService : IUiService, IInitializable
    {
        private readonly UiConfig _config;
        private readonly IInstantiator _instantiator;
        private readonly Dictionary<UIWindowType, IBaseUiView<BaseUIView>> _presenters = new();
        
        private MenuUIPresenter _menuUiPresenter;
        private GameplayUIPresenter _gameplayUiPresenter;
        private ShopUIPresenter _shopUiPresenter;

        private Transform _root;

        public UIService(UiConfig config, IInstantiator instantiator)
        {
            _config = config;
            _instantiator = instantiator;
        }

        public void Initialize()
        {
            _root = Object.Instantiate(_config.UIRootPrefab).transform;
            GameObject.DontDestroyOnLoad(_root);

            _menuUiPresenter = _instantiator.Instantiate<MenuUIPresenter>();
            _gameplayUiPresenter = _instantiator.Instantiate<GameplayUIPresenter>();
            _shopUiPresenter = _instantiator.Instantiate<ShopUIPresenter>();
            
            // TODO: лист презенторов, ковариантность
            _presenters.Add(UIWindowType.MenuWindow, _menuUiPresenter);
            _presenters.Add(UIWindowType.GameplayWindow, _gameplayUiPresenter);
            _presenters.Add(UIWindowType.ShopWindow, _shopUiPresenter);

            foreach (var (type, presenter) in _presenters)
            {
                var viewPrefab = _config.AllWindowConfigs.First(x => x.Type == type).ViewPrefab;
                var view = _instantiator.InstantiatePrefab(viewPrefab, _root);
                presenter.Initialize(view);
            }
        }

        public IBaseUiView<BaseUIView> GetPresenter<T>() where T : IBaseUiView<BaseUIView>
        {
            return _presenters.FirstOrDefault(x => x.Value is T).Value;
        }
    }
}