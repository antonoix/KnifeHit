using System;
using UnityEngine;
using UnityEngine.UI;

namespace Internal.Scripts.UI.GamePlay
{
    public class GameplayWinPanel : MonoBehaviour
    {
        [SerializeField] private Button goNextButton;
        [SerializeField] private Button toMenuButton;

        public event Action OnNextBtnClick;
        public event Action OnMenuBtnClick;

        public void Show(GameplayResult result)
        {
            gameObject.SetActive(true);
        }

        public void Hide()
            => gameObject.SetActive(false);

        private void OnEnable()
        {
            goNextButton.onClick.AddListener(HandleNextClicked);
            toMenuButton.onClick.AddListener(HandleToMenuClicked);
        }

        private void OnDisable()
        {
            goNextButton.onClick.RemoveListener(HandleNextClicked);
            toMenuButton.onClick.RemoveListener(HandleToMenuClicked);
        }

        private void HandleNextClicked()
        {
            OnNextBtnClick?.Invoke();
        }

        private void HandleToMenuClicked()
        {
            OnMenuBtnClick?.Invoke();
        }
    }
}