using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Internal.Scripts.UI.GamePlay
{
    public class GameplayWinPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinsReward;
        [SerializeField] private TMP_Text starsReward;
        [SerializeField] private Button goNextButton;
        [SerializeField] private Button toMenuButton;
        [SerializeField] private List<GameObject> stars;

        public event Action OnNextBtnClick;
        public event Action OnMenuBtnClick;

        public void Show(GameplayResult result)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f);
            var textAnim = DOTween.Sequence();
            textAnim.Append(DOTween
                .To(value => coinsReward.text = $"{value:f0}<sprite=0>", 0, result.CoinsCount, 2));
            textAnim.Join(DOTween
                .To(value => starsReward.text = $"{value:f0}<sprite=0>", 0, result.StarsCount, 2));

            foreach (var star in stars) 
                star.transform.localScale = Vector3.zero;
            var starsAnim = DOTween.Sequence();
            for (int i = 0; i < result.StarsCount; i++)
            {
                starsAnim.Append(stars[i].transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.InOutBack));
            }
            
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