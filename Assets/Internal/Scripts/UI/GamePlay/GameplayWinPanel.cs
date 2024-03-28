using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Internal.Scripts.UI.GamePlay
{
    public class GameplayWinPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinsReward;
        [SerializeField] private Button goNextButton;
        [SerializeField] private Button toMenuButton;

        public event Action OnNextBtnClick;
        public event Action OnMenuBtnClick;

        public void Show(GameplayResult result)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f);
            var sequence = DOTween.Sequence();
            sequence.Append(DOTween
                .To(value => coinsReward.text = $"{value:f0}<sprite=0>", 0, result.CoinsCount, 2));
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