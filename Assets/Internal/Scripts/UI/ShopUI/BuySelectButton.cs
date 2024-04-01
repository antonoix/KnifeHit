using System;
using Internal.Scripts.Infrastructure.Services.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Internal.Scripts.UI.ShopUI
{
    public class BuySelectButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image mainImage;
        [SerializeField] private GameObject buyStateRoot;
        [SerializeField] private GameObject selectStateRoot;
        [SerializeField] private TMP_Text buyPrice;
        [SerializeField] private TMP_Text selectText;

        private ILocalizationService _localizationService;
        private bool _isclickable;
        private bool _isBuyState;

        public event Action OnBuyClicked;
        public event Action OnSelectClicked;

        public void Construct(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }
        
        public void SetSelectState(bool isSelected)
        {
            buyStateRoot.SetActive(false);
            selectStateRoot.SetActive(true);
            _isBuyState = false;
            
            if (isSelected)
            {
                selectText.text = _localizationService.GetLocalized(LocalizationKeys.Selected);
            }
            else
            {
                selectText.text = _localizationService.GetLocalized(LocalizationKeys.Select);
            }

            SetClickable(!isSelected);
        }

        public void SetBuyState(long cost, bool canBuy)
        {
            buyStateRoot.SetActive(true);
            selectStateRoot.SetActive(false);
            buyPrice.text = $"{cost}<sprite=0>";
            _isBuyState = true;
            
            SetClickable(canBuy);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isclickable)
                return;
            
            if (_isBuyState)
                OnBuyClicked?.Invoke();
            else
            {
                OnSelectClicked?.Invoke();
            }
        }

        private void SetClickable(bool isClickable)
        {
            _isclickable = isClickable;
            mainImage.color = isClickable ? Color.white : Color.gray;
        }
    }
}