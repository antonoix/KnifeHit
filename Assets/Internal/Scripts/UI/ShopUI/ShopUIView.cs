using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Internal.Scripts.Infrastructure.Services.Localization;
using Internal.Scripts.Infrastructure.Services.UiService.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Internal.Scripts.UI.ShopUI
{
    public class ShopUIView : BaseUIView
    {
        [SerializeField] private TMP_Text currentCoins;
        [SerializeField] private Button nextBtn;
        [SerializeField] private Button prevBtn;
        [SerializeField] private Button menuBtn;
        [SerializeField] private BuySelectButton buyBtn;

        public event Action OnNextClicked;
        public event Action OnPrevClicked;
        public event Action OnBuyClicked;
        public event Action OnSelectClicked;
        public event Action OnMenuClicked;

        public void Construct(ILocalizationService localizationService)
        {
            buyBtn.Construct(localizationService);
        }

        public override async UniTask Show()
        {
            base.Show().Forget();
            
            nextBtn.transform.position += Vector3.right * 300;
            nextBtn.transform.DOMove(nextBtn.transform.position - Vector3.right * 300, .3f);
            
            prevBtn.transform.position += Vector3.left * 300;
            prevBtn.transform.DOMove(prevBtn.transform.position - Vector3.left * 300, .3f);
            
            await UniTask.WaitForSeconds(.3f);

            nextBtn.onClick.AddListener(HandleNextClicked);
            prevBtn.onClick.AddListener(HandlePrevClicked);
            menuBtn.onClick.AddListener(HandleMenuClicked);
            buyBtn.OnBuyClicked += HandleBuyClicked;
            buyBtn.OnSelectClicked += HandleSelectClicked;
        }

        public override UniTask Hide()
        {
            nextBtn.onClick.RemoveListener(HandleNextClicked);
            prevBtn.onClick.RemoveListener(HandlePrevClicked);
            menuBtn.onClick.RemoveListener(HandleMenuClicked);
            buyBtn.OnBuyClicked -= HandleBuyClicked;
            buyBtn.OnSelectClicked -= HandleSelectClicked;
            
            return base.Hide();
        }

        public void SetSelectState(bool isSelected)
        {
            buyBtn.SetSelectState(isSelected);
        }

        public void SetBuyState(long cost, bool canBuy)
        {
            buyBtn.SetBuyState(cost, canBuy);
        }

        public void SetCurrentCoins(long count)
        {
            currentCoins.text = $"{count}<sprite=0>";
        }

        private void HandleMenuClicked()
        {
            OnMenuClicked?.Invoke();
        }

        private void HandleNextClicked()
        {
            OnNextClicked?.Invoke();
        }

        private void HandlePrevClicked()
        {
            OnPrevClicked?.Invoke();
        }

        private void HandleSelectClicked()
        {
            OnSelectClicked?.Invoke();
        }

        private void HandleBuyClicked()
        {
            OnBuyClicked?.Invoke();
        }
    }
}