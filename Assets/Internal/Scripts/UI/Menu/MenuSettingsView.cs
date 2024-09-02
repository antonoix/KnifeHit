using System;
using Internal.Scripts.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Internal.Scripts.UI.Menu
{
    public class MenuSettingsView : MonoBehaviour
    {
        [field: SerializeField] public ToggleButton EnableSoundBtn { get; private set; }
        [SerializeField] private Button _exitButton;

        public event Action OnExitClicked;
        
        public void Activate(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnEnable()
        {
            _exitButton.onClick.AddListener(HandleExitClick);
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(HandleExitClick);
        }

        private void HandleExitClick()
        {
            OnExitClicked?.Invoke();
        }
    }
}