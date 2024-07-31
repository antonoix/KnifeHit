using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Internal.Scripts.Infrastructure.Services.UiService.Base;
using TMPro;
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

        public override async UniTask Show()
        {
            base.Show().Forget();
            gameplayWinPanel.Hide();
            gameplayLosePanel.Hide();
            
            Debug.Log("Show");
            progress.transform.position += Vector3.down * 300;
            progress.transform.DOMove(progress.transform.position - Vector3.down * 300, .3f);
            
            await UniTask.WaitForSeconds(.3f);

            menuButton.onClick.AddListener(HandleMenuBtnClick);

            gameplayWinPanel.OnMenuBtnClick += HandleMenuBtnClick;
            gameplayWinPanel.OnNextBtnClick += HandleNextBtnClick;

            gameplayLosePanel.OnMenuBtnClick += HandleMenuBtnClick;
            gameplayLosePanel.OnRestartBtnClick += HandleRestartBtnClick;
        }

        public override UniTask Hide()
        {
            menuButton.onClick.RemoveListener(HandleMenuBtnClick);
            
            gameplayWinPanel.OnMenuBtnClick -= HandleMenuBtnClick;
            gameplayWinPanel.OnNextBtnClick -= HandleNextBtnClick;
            
            gameplayLosePanel.OnMenuBtnClick -= HandleMenuBtnClick;
            gameplayLosePanel.OnRestartBtnClick -= HandleRestartBtnClick;
            
            return base.Hide();
        }

        public void ShowWinPanel(GameplayResult result)
        {
            gameplayWinPanel.Show(result);
        }

        public void ShowLosePanel()
        {
            gameplayLosePanel.Show();
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
