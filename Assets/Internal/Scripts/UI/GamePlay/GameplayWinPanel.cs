using System;
using System.Collections.Generic;
using DG.Tweening;
using Internal.Scripts.Infrastructure.Services.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Internal.Scripts.UI.GamePlay
{
    public class GameplayWinPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinsReward;
        [SerializeField] private TMP_Text starsReward;
        [SerializeField] private Button goNextButton;
        [SerializeField] private Button toMenuButton;
        [SerializeField] private List<GameObject> stars;

        private ISoundsService _soundsService;
        private Sequence _showAnim;

        public event Action OnNextBtnClick;
        public event Action OnMenuBtnClick;

        [Inject]
        private void Construct(ISoundsService soundsService)
        {
            _soundsService = soundsService;
        }

        public void Show(GameplayResult result)
        {
            _showAnim?.Kill();
            
            transform.localScale = Vector3.zero;
            foreach (var star in stars) 
                star.transform.localScale = Vector3.zero;
            
            _showAnim = DOTween.Sequence();
            _showAnim.Append(transform.DOScale(Vector3.one, 0.5f));
            
            for (int i = 0; i < result.StarsCount; i++)
                _showAnim.Append(stars[i].transform.DOScale(Vector3.one, 0.45f).SetEase(Ease.OutBack));

            _showAnim.Append(DOTween
                .To(value => coinsReward.text = $"{value:f0}<sprite=0>", 0, result.CoinsCount, 2));
            _showAnim.Join(DOTween
                .To(value => starsReward.text = $"{value:f0}<sprite=0>", 0, result.StarsCount, 2));

            gameObject.SetActive(true);
            _soundsService.PlaySound(SoundType.Zajebaty);
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