using System;
using UnityEngine;
using UnityEngine.UI;

namespace Internal.Scripts.UI.Menu
{
    public class MenuUIView : MonoBehaviour
    {
        [SerializeField] private Button startGameBtn;

        public event Action OnStartBtnClicked;
    
        public void Initialize()
        {
            startGameBtn.onClick.AddListener(HandleStartBtnClicked);
        }

        public void Dispose()
        {
            startGameBtn.onClick.RemoveListener(HandleStartBtnClicked);
        }

        private void HandleStartBtnClicked()
        {
            OnStartBtnClicked?.Invoke();
        }
    }
}
