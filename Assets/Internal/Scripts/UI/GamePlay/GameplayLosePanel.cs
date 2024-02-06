using System;
using UnityEngine.UI;
using UnityEngine;

namespace Internal.Scripts.UI.GamePlay
{
    public class GameplayLosePanel : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button toMenuButton;

        public event Action OnRestartBtnClick;
        public event Action OnMenuBtnClick;

        public void Show(GameplayResult result)
        {
            gameObject.SetActive(true);
        }

        public void Hide()
            => gameObject.SetActive(false);
        
        private void OnEnable()
        {
            restartButton.onClick.AddListener(HandleRestartClicked);
            toMenuButton.onClick.AddListener(HandleToMenuClicked);
        }

        private void OnDisable()
        {
            restartButton.onClick.RemoveListener(HandleRestartClicked);
            toMenuButton.onClick.RemoveListener(HandleToMenuClicked);
        }

        private void HandleRestartClicked()
        {
            OnRestartBtnClick?.Invoke();
        }

        private void HandleToMenuClicked()
        {
            OnMenuBtnClick?.Invoke();
        }
    }
}