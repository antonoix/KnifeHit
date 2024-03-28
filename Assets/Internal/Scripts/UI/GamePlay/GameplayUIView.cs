using System;
using Internal.Scripts.Infrastructure.Services.UiService.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Internal.Scripts.UI.GamePlay
{
    public class GameplayUIView : BaseUIView
    {
        [SerializeField] private GameplayWinPanel gameplayWinPanel;
        [SerializeField] private GameplayLosePanel gameplayLosePanel;
        [SerializeField] private GameObject progress;
        [SerializeField] private Image progressImage;
        [SerializeField] private Button menuButton;
        
        public event Action OnNextBtnClick;
        public event Action OnMenuBtnClick;
        public event Action OnRestartBtnClick;
        
        public override void Initialize()
        {
            //throw new System.NotImplementedException();
        }

        public override void Dispose()
        {
            //throw new System.NotImplementedException();
        }

        public override void Show()
        {
            gameplayWinPanel.Hide();
            gameplayLosePanel.Hide();
            
            menuButton.onClick.AddListener(HandleMenuBtnClick);
            
            gameplayWinPanel.OnMenuBtnClick += HandleMenuBtnClick;
            gameplayWinPanel.OnNextBtnClick += HandleNextBtnClick;

            gameplayLosePanel.OnMenuBtnClick += HandleMenuBtnClick;
            gameplayLosePanel.OnRestartBtnClick += HandleRestartBtnClick;
            
            base.Show();
        }

        public override void Hide()
        {
            menuButton.onClick.RemoveListener(HandleMenuBtnClick);
            
            gameplayWinPanel.OnMenuBtnClick -= HandleMenuBtnClick;
            gameplayWinPanel.OnNextBtnClick -= HandleNextBtnClick;
            
            gameplayLosePanel.OnMenuBtnClick -= HandleMenuBtnClick;
            gameplayLosePanel.OnRestartBtnClick -= HandleRestartBtnClick;
            
            base.Hide();
        }

        public void ShowWinPanel(GameplayResult result)
        {
            gameplayWinPanel.Show(result);
        }

        public void ShowLosePanel(GameplayResult result)
        {
            gameplayLosePanel.Show(result);
        }

        public void SetActiveProgress(bool isActive)
            => progress.gameObject.SetActive(isActive);

        public void SetProgress(float progress)
            => progressImage.fillAmount = progress;

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
