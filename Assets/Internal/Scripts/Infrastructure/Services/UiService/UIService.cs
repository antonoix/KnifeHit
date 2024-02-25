using System.Collections.Generic;
using System.Linq;
using Internal.Scripts.Infrastructure.Services.Localization;
using Internal.Scripts.Infrastructure.Services.ProgressService;
using Internal.Scripts.Infrastructure.Services.Sound;
using Internal.Scripts.Infrastructure.Services.UiService.Base;
using Internal.Scripts.UI.GamePlay;
using Internal.Scripts.UI.Menu;

namespace Internal.Scripts.Infrastructure.Services.UiService
{
    public class UIService : IUiService
    {
        private readonly IPlayerProgressService _playerProgressService;
        private readonly MenuUIPresenter _menuUiPresenter;
        private readonly GameplayUIPresenter _gameplayUiPresenter;
        private readonly List<IBaseUiView<BaseUIView>> _presenters = new();

        public UIService(UiConfig config, IPlayerProgressService playerProgressService,
            ILocalizationService localizationService, ISoundsService soundsService)
        {
            _playerProgressService = playerProgressService;
            _menuUiPresenter = new MenuUIPresenter(config.MenuUIPrefab, _playerProgressService, localizationService, soundsService);
            _gameplayUiPresenter = new GameplayUIPresenter(config.GameplayUIPrefab, soundsService);
        }

        public void Initialize()
        {
            // TODO: лист презенторов, ковариантность
            _presenters.Add(_menuUiPresenter);
            _presenters.Add(_gameplayUiPresenter);

            foreach (var presenter in _presenters)
            {
                presenter.Initialize();
            }
        }

        public IBaseUiView<BaseUIView> GetPresenter<T>() where T : IBaseUiView<BaseUIView>
        {
            return _presenters.FirstOrDefault(x => x is T);
        }
    }
}