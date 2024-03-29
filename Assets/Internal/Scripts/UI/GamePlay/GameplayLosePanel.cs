using System;
using DG.Tweening;
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

        public void Show()
        {
            gameObject.SetActive(true);
            
            transform.localScale = Vector3.zero;
            restartButton.transform.localScale = Vector3.zero;
            toMenuButton.transform.localScale = Vector3.zero;
            
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(Vector3.one, 0.3f));
            sequence.Append(restartButton.transform.DOScale(Vector3.one, 0.2f));
            sequence.Append(toMenuButton.transform.DOScale(Vector3.one, 0.2f));
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