using System;
using Internal.Scripts.Infrastructure.Services.Sound;
using Internal.Scripts.Infrastructure.Services.UiService.Base;

namespace Internal.Scripts.UI.GamePlay
{
    public class GameplayUIPresenter : BaseUIPresenter<GameplayUIView>
    {
        private readonly ISoundsService _soundsService;
        public event Action OnNextBtnClick;
        public event Action OnMenuBtnClick;
        public event Action OnRestartBtnClick;
        
        public GameplayUIPresenter(ISoundsService soundsService)
        {
            _soundsService = soundsService;
        }

        public override void Show()
        {
            _view.OnMenuBtnClick += HandleMenuBtnClick;
            _view.OnNextBtnClick += HandleNextBtnClick;
            _view.OnRestartBtnClick += HandleRestartBtnClick;
            _view.SetActiveProgress(true);
            
            base.Show();
        }

        public override void Hide()
        {
            _view.OnMenuBtnClick -= HandleMenuBtnClick;
            _view.OnNextBtnClick -= HandleNextBtnClick;
            _view.OnRestartBtnClick -= HandleRestartBtnClick;
            
            base.Hide();
        }

        public void ShowWinPanel(int rewardCoinsCount)
        {
            GameplayResult result = new(rewardCoinsCount);
            _view.ShowWinPanel(result);
            _view.SetActiveProgress(false);
        }

        public void ShowLosePanel()
        {
            GameplayResult result = new(1);
            _view.ShowLosePanel(result);
            _view.SetActiveProgress(false);
        }

        public void UpdateProgress(float value)
            => _view.SetProgress(value);

        private void HandleNextBtnClick()
        {
            _soundsService.PlaySound(SoundType.ButtonClick);
            OnNextBtnClick?.Invoke();
        }

        private void HandleMenuBtnClick()
        {
            _soundsService.PlaySound(SoundType.ButtonClick);
            OnMenuBtnClick?.Invoke();
        }

        private void HandleRestartBtnClick()
        {
            _soundsService.PlaySound(SoundType.ButtonClick);
            OnRestartBtnClick?.Invoke();
        }
    }
}
