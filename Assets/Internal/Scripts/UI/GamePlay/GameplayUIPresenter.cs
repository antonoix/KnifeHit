using System;
using Internal.Scripts.Infrastructure.Services.UiService.Base;

namespace Internal.Scripts.UI.GamePlay
{
    public class GameplayUIPresenter : BaseUIPresenter<GameplayUIView>
    {
        public event Action OnNextBtnClick;
        public event Action OnMenuBtnClick;
        public event Action OnRestartBtnClick;
        
        public GameplayUIPresenter(GameplayUIView baseUIViewPrefab) : base(baseUIViewPrefab)
        {
        }
        
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Show()
        {
            _view.OnMenuBtnClick += HandleMenuBtnClick;
            _view.OnNextBtnClick += HandleNextBtnClick;
            _view.OnRestartBtnClick += HandleRestartBtnClick;
            
            base.Show();
        }

        public override void Hide()
        {
            _view.OnMenuBtnClick -= HandleMenuBtnClick;
            _view.OnNextBtnClick -= HandleNextBtnClick;
            _view.OnRestartBtnClick -= HandleRestartBtnClick;
            
            base.Hide();
        }

        public void ShowWinPanel()
        {
            GameplayResult result = new(1);
            _view.ShowWinPanel(result);
        }

        public void ShowLosePanel()
        {
            GameplayResult result = new(1);
            _view.ShowLosePanel(result);
        }

        private void HandleNextBtnClick()
        {
            OnNextBtnClick?.Invoke();
        }

        private void HandleMenuBtnClick()
        {
            OnMenuBtnClick?.Invoke();
        }

        private void HandleRestartBtnClick()
        {
            OnRestartBtnClick?.Invoke();
        }
    }
}
