using System;
using Internal.Scripts.Infrastructure.Services.UiService.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Internal.Scripts.UI.Menu
{
    public class MenuUIView : BaseUIView
    {
        [SerializeField] private Button startGameBtn;
        [SerializeField] private Button shopBtn;
        [SerializeField] private TMP_Text startText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text coinsCountText;

        public event Action OnStartBtnClicked;
        public event Action OnShopBtnClicked;

        public override void Show()
        {
            base.Show();
            
            startGameBtn.onClick.AddListener(HandleStartBtnClicked);
            shopBtn.onClick.AddListener(HandleShopBtnClicked);
        }

        public override void Hide()
        {
            base.Hide();
            
            startGameBtn.onClick.RemoveListener(HandleStartBtnClicked);
            shopBtn.onClick.RemoveListener(HandleShopBtnClicked);
        }
        
        public void SetCurrentCoins(int count)
        {
            coinsCountText.text = $"{count}<sprite=0>";
        }

        public void SetCurrentLevel(string text) 
            => levelText.text = text;
        
        public void SetStartText(string text) 
            => startText.text = text;

        private void HandleStartBtnClicked()
        {
            OnStartBtnClicked?.Invoke();
        }
        
        private void HandleShopBtnClicked()
        {
            OnShopBtnClicked?.Invoke();
        }
    }
}
