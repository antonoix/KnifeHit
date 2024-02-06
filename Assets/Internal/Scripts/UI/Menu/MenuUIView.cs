using System;
using Internal.Scripts.Infrastructure.Services.UiService.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Internal.Scripts.UI.Menu
{
    public class MenuUIView : BaseUIView
    {
        [SerializeField] private Button startGameBtn;

        public event Action OnStartBtnClicked;
    
        public override void Initialize()
        {
            startGameBtn.onClick.AddListener(HandleStartBtnClicked);
        }

        public override void Dispose()
        {
            startGameBtn.onClick.RemoveListener(HandleStartBtnClicked);
        }

        private void HandleStartBtnClicked()
        {
            OnStartBtnClicked?.Invoke();
        }
    }
}
