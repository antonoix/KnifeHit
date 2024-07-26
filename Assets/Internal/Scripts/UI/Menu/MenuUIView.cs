using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Internal.Scripts.Infrastructure.Services.UiService.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Internal.Scripts.UI.Menu
{
    public class MenuUIView : BaseUIView
    {
        private const float ONE_SEC = 1;
        
        [SerializeField] private Button startGameBtn;
        [SerializeField] private Button shopBtn;
        [SerializeField] private TMP_Text startText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text coinsCountText;
        [SerializeField] private TMP_Text starsCountText;

        public event Action OnStartBtnClicked;
        public event Action OnShopBtnClicked;

        public override async UniTask Show()
        {
            base.Show().Forget();
            
            shopBtn.transform.position += Vector3.right * 300;
            shopBtn.transform.DOMove(shopBtn.transform.position - Vector3.right * 300, .3f);
            
            await UniTask.WaitForSeconds(.3f);
            
            startGameBtn.onClick.AddListener(HandleStartBtnClicked);
            shopBtn.onClick.AddListener(HandleShopBtnClicked);
        }

        public override UniTask Hide()
        {
            startGameBtn.onClick.RemoveListener(HandleStartBtnClicked);
            shopBtn.onClick.RemoveListener(HandleShopBtnClicked);
            
            return base.Hide();
        }
        
        public void SetCurrentCoins(long count)
        {
            DOTween.To(x => coinsCountText.text = $"{x:0}<sprite=0>", 0, count, ONE_SEC);
        }
        
        public void SetCurrentStars(long count)
        {
            DOTween.To(x => starsCountText.text = $"{x:0}<sprite=1>", 0, count, ONE_SEC);
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
