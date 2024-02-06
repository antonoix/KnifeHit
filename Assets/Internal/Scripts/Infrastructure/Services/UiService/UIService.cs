using System.Collections.Generic;
using System.Linq;
using Internal.Scripts.Infrastructure.Services.Base;
using Internal.Scripts.Infrastructure.Services.UiService.Base;
using Internal.Scripts.UI.GamePlay;
using Internal.Scripts.UI.Menu;

namespace Internal.Scripts.Infrastructure.Services.UiService
{
    public class UIService : IUiService
    {
        private readonly MenuUIPresenter _menuUiPresenter;
        private readonly GameplayUIPresenter _gameplayUiPresenter;
        private readonly List<IBaseUiView<BaseUIView>> _presenters = new();

        public UIService(UiConfig config)
        {
            _menuUiPresenter = new MenuUIPresenter(config.MenuUIPrefab);
            _gameplayUiPresenter = new GameplayUIPresenter(config.GameplayUIPrefab);
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