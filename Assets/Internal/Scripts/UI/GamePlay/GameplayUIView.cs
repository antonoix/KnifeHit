using System;
using Internal.Scripts.Infrastructure.Services.UiService.Base;
using UnityEngine;

namespace Internal.Scripts.UI.GamePlay
{
    public class GameplayUIView : BaseUIView
    {
        [SerializeField] private GameplayWinPanel gameplayWinPanel;
        [SerializeField] private GameplayLosePanel gameplayLosePanel;
        
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
            
            gameplayWinPanel.OnMenuBtnClick += HandleMenuBtnClick;
            gameplayWinPanel.OnNextBtnClick += HandleNextBtnClick;

            gameplayLosePanel.OnMenuBtnClick += HandleMenuBtnClick;
            gameplayLosePanel.OnRestartBtnClick += HandleRestartBtnClick;
            
            base.Show();
        }

        public override void Hide()
        {
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
