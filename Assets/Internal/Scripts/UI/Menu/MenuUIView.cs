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
        [SerializeField] private TMP_Text startText;
        [SerializeField] private TMP_Text levelText;

        public event Action OnStartBtnClicked;
    
        public override void Initialize()
        {
            startGameBtn.onClick.AddListener(HandleStartBtnClicked);
        }

        public override void Dispose()
        {
            startGameBtn.onClick.RemoveListener(HandleStartBtnClicked);
        }

        public void SetCurrentLevel(string text) 
            => levelText.text = text;
        
        public void SetStartText(string text) 
            => startText.text = text;

        private void HandleStartBtnClicked()
        {
            OnStartBtnClicked?.Invoke();
        }
    }
}
